using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using ObjectPrinter.Printers;
using ObjectPrinter.TypeInspectors;
using ObjectPrinter.Utilties;

namespace ObjectPrinter
{
	public class ObjectPrinter
	{
		private const string NullValue = "{NULL}";

		/// <summary>The default config to use.</summary>
		public static Func<IObjectPrinterConfig> GetDefaultContext = () => new ObjectPrinterConfig();

        private readonly object _rootObject;
        private readonly IObjectPrinterConfig _config;
        private readonly string _tab;
        private readonly string _newline;

        private IndentableTextWriter _output;

		private int _currentDepth = -1;
		private readonly Stack<object> _objStack = new Stack<object>();
	    private ObjectInfosPrinter _objectInfosPrinter;
	    private DictionaryPrinter _dictionaryPrinter;
	    private NameValueCollectionPrinter _nameValueCollectionPrinter;
	    private EnumerablePrinter _enumerablePrinter;

	    public ObjectPrinter(object obj)
			: this(obj, GetDefaultContext())
		{
		}

		public ObjectPrinter(object obj, IObjectPrinterConfig config)
		{
            _rootObject = obj;
            _config = config;
            _tab = config.Tab;
            _newline = config.NewLine;
        }

        public void PrintTo(TextWriter output)
        {
            _output = new IndentableTextWriter(output, _tab, _newline);
            _objectInfosPrinter = new ObjectInfosPrinter(_output, AppendValue);
            _dictionaryPrinter = new DictionaryPrinter(_output, AppendValue);
            _nameValueCollectionPrinter = new NameValueCollectionPrinter(_output, AppendValue);
            _enumerablePrinter = new EnumerablePrinter(_output, AppendValue);
            WriteObject(new ObjectInfo { Value = _rootObject });
        }

	    public string PrintToString()
        {
            var sb = new StringBuilder();
			try
            {
                using (var sw = new StringWriter(sb))
                {
                    PrintTo(sw);
                }
			}
			catch (Exception e)
			{
				string partialOutput;
				try
				{
					partialOutput = sb.ToString();
				}
				catch (Exception exception)
				{
					partialOutput = "Unable to build partial output due to error: " + exception.Message;
				}
				e.Data["ObjectPrinter Partial Output"] = partialOutput;
				throw;
			}
			return sb.ToString();
		}

		private void WriteObject(ObjectInfo objectInfo)
		{
			_currentDepth++;
			WriteObjectImpl(objectInfo);
			_currentDepth--;
		}

		private void WriteObjectImpl(ObjectInfo objectInfo)
		{
			//Assumption: the label has already been printed on the line.
			//            if you _output.WriteLine, the value will not appear
			//            on same line as the label

			var objToAppend = objectInfo.Value;

			if (objToAppend == null)
			{
				_output.Write(NullValue);
				return;
			}

			Type typeOfOjbToAppend = objectInfo.Type;

			//Avoid referential loops by not letting an object be dumped as a descendent in the graph
			//TODO: when value types create circular references, this fails 
			//		because ReferenceEquals will box the value type and thus never have the same reference
			if (_objStack.Any(o => ReferenceEquals(objToAppend, o)))
			{
				_output.Write("avoid circular loop for this [" + typeOfOjbToAppend.Name + "]: hashcode { " + objToAppend.GetHashCode() + " }");
				return;
			}

			//Avoid StackOverflow caused by recursive value types like Linq2Sql or ConfigurationException.Errors
			if (_currentDepth >= _config.MaxDepth)
			{
				_output.Write("Maximum recursion depth (" + _config.MaxDepth + ") reached");
				return;
			}


			//*** continue recursive printing of the object

			_objStack.Push(objToAppend);

            if (objToAppend is string)
            {
                //in case the string contains line returns, the next line will be indented from the member name
                _output.Indent();
                _output.Write((string)objToAppend);
                _output.Outdent();
            }
			else if (objToAppend is XmlNode)
			{
				_output.Write(((XmlNode)objToAppend).InnerXml);
			}
			else if (objToAppend is IDictionary)
			{
				_dictionaryPrinter.Write((IDictionary)objToAppend);
			}
			else if (objToAppend is NameValueCollection)
			{
				_nameValueCollectionPrinter.Write((NameValueCollection)objToAppend);
			}
			else if (objToAppend is IEnumerable)
			{
				_enumerablePrinter.Write((IEnumerable)objToAppend);
			}
			else
			{
				object singleValue;
				List<ObjectInfo> properties;

				if (TryGetSingleValue(objectInfo, out singleValue, out properties))
				{
					_output.Write(singleValue ?? NullValue);
				}
				else
				{
					_output.Write("[" + typeOfOjbToAppend.Name + "]: hashcode { " + objToAppend.GetHashCode() + " }");
					if (_config.IncludeLogging)
					{
					    var inspectorName = objectInfo.Inspector == null
					                            ? NullValue
					                            : objectInfo.Inspector.GetType().Name;
					    _output.Write(" - Inspector { " + inspectorName + " } ");
					}
				    _objectInfosPrinter.Write(properties);
				}
			}

			//we are done printing the descendents of this object.  
            //free it to be printed in another section.
            _objStack.Pop();
		}

		private bool TryGetSingleValue(ObjectInfo objectInfo, out object singleValue, out List<ObjectInfo> members)
		{
			singleValue = null;
			members = null;

            if (objectInfo.Inspector == null)
            {
                objectInfo.Inspector = _config.GetInspector(objectInfo.Value, objectInfo.Type);
            }

			if (objectInfo.Inspector == null)
			{
				singleValue = objectInfo.Value;
				return true;
			}

			members = objectInfo.Inspector.GetMemberList(objectInfo).ToList();
			if (members.Count == 1 && (members[0].Value == null || members[0].Value is string))
			{
				singleValue = members[0].Value;
				return true;
			}

			return false;
        }

		private void AppendValue(ObjectInfo objectInfo)
		{
			if (!string.IsNullOrEmpty(objectInfo.Name))
			{
				_output.Write(objectInfo.Name);
				_output.Write(" : ");
			}

			if (objectInfo.Value == null)
			{
				_output.Write(NullValue);
			}
			else
			{
				WriteObject(objectInfo);	
			}
			_output.WriteLine();
		}
	}
}
