using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using ObjectPrinter.TypeInspectors;
using ObjectPrinter.Utilties;

namespace ObjectPrinter
{
	public class ObjectPrinter
	{
		private const string NullValue = "{NULL}";

		/// <summary>The default config to use.</summary>
		public static Func<IObjectPrinterConfig> GetDefaultContext = () => new ObjectPrinterConfig();

		private readonly object _obj;
		private readonly IndentAwareStringBuilder _sb;
		private readonly IObjectPrinterConfig _config;
		private int _currentDepth = -1;
		private readonly List<object> _objsAlreadyAppended = new List<object>();

		public ObjectPrinter(object obj)
			: this(obj, GetDefaultContext())
		{
		}

		public ObjectPrinter(object obj, string tab, string newline)
			: this(obj, new ObjectPrinterConfig { Tab = tab, NewLine = newline })
		{
		}

		public ObjectPrinter(object obj, IObjectPrinterConfig config)
		{
			_obj = obj;
			_config = config;
			_sb = new IndentAwareStringBuilder(config.Tab, config.NewLine);
		}

		public string Print()
		{
			try
			{
				AppendObject(new ObjectInfo {Value = _obj, Inspector = _config.GetInspector(_obj, _obj.GetType())});
			}
			catch (Exception e)
			{
				string partialOutput;
				try
				{
					partialOutput = _sb.ToString();
				}
				catch (Exception exception)
				{
					partialOutput = "Unable to build partial output due to error: " + exception.Message;
				}
				e.Data["ObjectPrinter Partial Output"] = partialOutput;
				throw;
			}
			return _sb.ToString();
		}

		private void AppendObject(ObjectInfo objectInfo)
		{
			_currentDepth++;
			AppendObjectImpl(objectInfo);
			_currentDepth--;
		}

		private void AppendObjectImpl(ObjectInfo objectInfo)
		{
			//Assumption: the label has already been printed on the line.
			//            if you _sb.AppendLine, the value will not appear
			//            on same line as the label

			var objToAppend = objectInfo.Value;

			if (objToAppend == null)
			{
				_sb.Append(NullValue);
				return;
			}

			Type typeOfOjbToAppend = objectInfo.Type;

			//Avoid referential loops by not letting an object be dumped as a descendent in the graph
			//TODO: when value types create circular references, this fails 
			//		because ReferenceEquals will box the value type and thus never have the same reference
			if (_objsAlreadyAppended.Any(o => ReferenceEquals(objToAppend, o)))
			{
				_sb.Append("avoid circular loop for this [" + typeOfOjbToAppend.Name + "]: hashcode { " + objToAppend.GetHashCode() + " }");
				return;
			}

			//Avoid StackOverflow caused by objects like ConfigurationException.Errors
			if (_currentDepth >= _config.MaxDepth)
			{
				_sb.Append("Maximum recursion depth reached");
				return;
			}


			//*** continue recursive printing of the object

			_objsAlreadyAppended.Add(objToAppend);

			if (objToAppend is IDictionary)
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
					_sb.Append(singleValue ?? NullValue);
				}
				else
				{
					_sb.Append("[" + typeOfOjbToAppend.Name + "]: hashcode { " + objToAppend.GetHashCode() + " }");
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

			objectInfo.Inspector = objectInfo.Inspector ?? _config.GetInspector(_obj, _obj.GetType());
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

		private void AppendProperties(List<ObjectInfo> objectInfos)
		{
			if (objectInfos.IsNullOrEmpty())
			{
				_sb.EndLineWith("{}");
				return;
			}

			_sb.EndLine();
			_sb.AppendLine("{");
			_sb.IncrementTabDepth();

			foreach (var objectInfo in objectInfos)
			{
				AppendValue(objectInfo);
			}

			_sb.DecrementTabDepth();
			_sb.AppendLine("}");
		}

		private void AppendEnumerable(IEnumerable objToAppend)
		{
			if (objToAppend == null)
			{
				_sb.EndLineWith("{}");
				return;
			}

			//makes the print prettier.  needed because IEnumerable has no Count or Length property
			bool countIsZero = true;

			foreach (var obj in objToAppend)
			{
				if (countIsZero)
				{
					countIsZero = false;
					_sb.EndLine();
					_sb.AppendLine("{");
					_sb.IncrementTabDepth();
				}
				AppendValue(null, obj);
			}

			if (countIsZero)
			{
				_sb.EndLineWith("{}");
			}
			else
			{
				_sb.DecrementTabDepth();
				_sb.AppendLine("}");
			}
		}

		private void AppendDictionary(IDictionary objToAppend)
		{
			if (objToAppend == null || objToAppend.Count == 0)
			{
				_sb.EndLineWith("{}");
				return;
			}

			_sb.EndLine();
			_sb.AppendLine("{");
			_sb.IncrementTabDepth();
			foreach (DictionaryEntry de in objToAppend)
			{
				AppendValue(de.Key.ToString(), de.Value);
			}
			_sb.DecrementTabDepth();
			_sb.AppendLine("}");
		}

		private void AppendNameObjectCollectionBase(NameValueCollection objToAppend)
		{
			if (objToAppend == null || objToAppend.Count == 0)
			{
				_sb.EndLineWith("{}");
				return;
			}

			_sb.EndLine();
			_sb.AppendLine("{");
			_sb.IncrementTabDepth();
			for (int i = 0; i < objToAppend.Count; i++)
			{
				AppendValue(objToAppend.Keys[i], objToAppend[i]);
			}
			_sb.DecrementTabDepth();
			_sb.AppendLine("}");
		}

		private void AppendValue(ObjectInfo objectInfo)
		{
			_sb.StartLine();
			if (!string.IsNullOrEmpty(objectInfo.Name))
			{
				_sb.Append(objectInfo.Name);
				_sb.Append(" : ");
			}

			if (objectInfo.Value == null)
			{
				_sb.Append(NullValue);
			}
			else if (typeof(string).IsAssignableFrom(objectInfo.Type))
			{
				_sb.Append(objectInfo.Value);
			}
			else
			{
				AppendObject(objectInfo);	
			}
			_sb.EndLine();
		}

		private void AppendValue(string key, object value)
		{
			_sb.StartLine();
			if (!string.IsNullOrEmpty(key))
			{
				_sb.Append(key);
				_sb.Append(" : ");
			}
			if (value == null)
			{
				_sb.Append(NullValue);
			}
			else
			{
				var type = value.GetType();
				if (typeof(string).IsAssignableFrom(type))
				{
					_sb.Append(value);
				}
				else
				{
					AppendObject(new ObjectInfo
					             	{
					             		Name = key,
					             		Value = value,
					             		Inspector = _config.GetInspector(value, type)
					             	});
				}
			}
			_sb.EndLine();
		}
	}
}
