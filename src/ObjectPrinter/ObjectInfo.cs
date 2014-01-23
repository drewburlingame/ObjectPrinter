using System;
using ObjectPrinter.TypeInspectors;

namespace ObjectPrinter
{
	public class ObjectInfo
	{
		private Type _type;
		private object _value;
		public string Name { get; set; }
        public ITypeInspector Inspector { get; set; }

        public ObjectInfo(object value)
        {
            _value = value;
        }

	    public ObjectInfo(string name, object value)
        {
            Name = name;
	        _value = value;
	    }

	    public object Value
		{
			get { return _value; } 
			set { _value = value; _type = null; }
		}
		public Type Type
		{
			get { return _type ?? (_type = Value.GetType()); }
		}
	}
}