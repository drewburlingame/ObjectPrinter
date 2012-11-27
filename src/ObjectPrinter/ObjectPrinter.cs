using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
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
        private string _tab;
        private string _newline;

        private IndentableTextWriter _output;

		private int _currentDepth = -1;
		private readonly List<object> _objsAlreadyAppended = new List<object>();

	    public ObjectPrinter(object obj)
			: this(obj, GetDefaultContext())
		{
		}

		public ObjectPrinter(object obj, string tab, string newline)
			: this(obj, GetDefaultContext(), tab, newline)
		{
		}

		public ObjectPrinter(object obj, IObjectPrinterConfig config)
            : this(obj, config, config.Tab, config.NewLine)
		{
		}

        private ObjectPrinter(object obj, IObjectPrinterConfig config, string tab, string newline)
        {
            _rootObject = obj;
            _config = config;
            _tab = tab;
            _newline = newline;
        }

        public void PrintTo(TextWriter output)
        {
            _output = new IndentableTextWriter(output, _tab, _newline);
            WriteObject(new ObjectInfo { Value = _rootObject, Inspector = _config.GetInspector(_rootObject, _rootObject.GetType()) });
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
			if (_objsAlreadyAppended.Any(o => ReferenceEquals(objToAppend, o)))
			{
				_output.Write("avoid circular loop for this [" + typeOfOjbToAppend.Name + "]: hashcode { " + objToAppend.GetHashCode() + " }");
				return;
			}

			//Avoid StackOverflow caused by objects like ConfigurationException.Errors
			if (_currentDepth >= _config.MaxDepth)
			{
				_output.Write("Maximum recursion depth (" + _config.MaxDepth + ") reached");
				return;
			}


			//*** continue recursive printing of the object

			_objsAlreadyAppended.Add(objToAppend);

			if (objToAppend is XmlNode)
			{
				_output.Write(((XmlNode)objToAppend).InnerXml);
			}
			else if (objToAppend is IDictionary)
			{
				AppendDictionary((IDictionary)objToAppend);
			}
			else if (objToAppend is NameValueCollection)
			{
				AppendNameObjectCollectionBase((NameValueCollection)objToAppend);
			}
			else if (objToAppend is IEnumerable && !(objToAppend is string))
			{
				AppendEnumerable((IEnumerable)objToAppend);
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
						_output.Write(" - Inspector { " + objectInfo.Inspector.GetType().Name + " } ");
					}
					AppendProperties(properties);
				}
			}

			//we are done printing the descendents of this object.  
			//free it to be printed in another section.
			try
			{
				_objsAlreadyAppended.Remove(objToAppend);
			}
			catch
			{
				//not critcal so swallow.  Occurs when a class doesn't have a safe Equals override.
			}
		}

		private bool TryGetSingleValue(ObjectInfo objectInfo, out object singleValue, out List<ObjectInfo> properties)
		{
			singleValue = null;
			properties = null;

			objectInfo.Inspector = objectInfo.Inspector ?? _config.GetInspector(objectInfo.Value, objectInfo.Type);
			if (objectInfo.Inspector == null)
			{
				singleValue = objectInfo.Value;
				return true;
			}

			properties = objectInfo.Inspector.GetPropertyList(objectInfo).ToList();
			if (properties.Count == 1 && (properties[0].Value == null || properties[0].Value is string))
			{
				singleValue = properties[0].Value;
				return true;
			}

			return false;
        }


        private void WriteEmptyChildren()
        {
            _output.WriteLine("{}");
        }

        private void StartChildWrapper()
        {
            _output.WriteLine();
            _output.WriteLine("{");
            _output.Indent();
        }

        private void EndChildWrapper()
        {
            _output.Outdent();
            //no need to writeline because the object itself will 
            _output.Write("}");
        }

		private void AppendProperties(List<ObjectInfo> objectInfos)
		{
			if (objectInfos.IsNullOrEmpty())
			{
			    WriteEmptyChildren();
			    return;
			}

		    StartChildWrapper();

		    foreach (var objectInfo in objectInfos)
			{
				AppendValue(objectInfo);
			}

			EndChildWrapper();
		}

	    private void AppendEnumerable(IEnumerable objToAppend)
		{
			if (objToAppend == null)
			{
                WriteEmptyChildren();
				return;
			}

			//makes the print prettier.  needed because IEnumerable has no Count or Length property
			bool countIsZero = true;

			foreach (var obj in objToAppend)
			{
				if (countIsZero)
				{
					countIsZero = false;
					StartChildWrapper();
				}
				AppendValue(null, obj);
			}

			if (countIsZero)
			{
                WriteEmptyChildren();
			}
			else
			{
				EndChildWrapper();
			}
		}

		private void AppendDictionary(IDictionary objToAppend)
		{
			if (objToAppend == null || objToAppend.Count == 0)
			{
				WriteEmptyChildren();
				return;
			}

			StartChildWrapper();

			foreach (DictionaryEntry de in objToAppend)
			{
				AppendValue(de.Key.ToString(), de.Value);
			}
			
            EndChildWrapper();
		}

		private void AppendNameObjectCollectionBase(NameValueCollection objToAppend)
		{
			if (objToAppend == null || objToAppend.Count == 0)
			{
				WriteEmptyChildren();
				return;
			}

			StartChildWrapper();

			for (int i = 0; i < objToAppend.Count; i++)
			{
				AppendValue(objToAppend.Keys[i], objToAppend[i]);
			}
			
            EndChildWrapper();
		}

        private void AppendValue(string key, object value)
        {
            AppendValue(new ObjectInfo
            {
                Name = key,
                Value = value,
                //if value is null, we won't need the inspector
                Inspector = value == null ? null : _config.GetInspector(value, value.GetType())
            });
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
			else if (typeof(string).IsAssignableFrom(objectInfo.Type))
			{
                //in case the string contains line returns, the next line will be indented from the member name
                _output.Indent();
				_output.Write(objectInfo.Value);
                _output.Outdent();
			}
			else
			{
				WriteObject(objectInfo);	
			}
			_output.WriteLine();
		}
	}
}
