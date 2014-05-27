using System;
using ObjectPrinter.TypeInspectors;

namespace ObjectPrinter
{
	/// <summary>
	/// The info used by the ObjectPrinter to inspect and print items
	/// </summary>
	public class ObjectInfo
	{
		private Type _type;
		private object _value;

		/// <summary>
		/// The name of the value to be printed.
		/// </summary>
		public string Name { get; set; }

        /// <summary>
        /// The inspector to use to inspect the object.  This overrides any configured inspectors.
        /// </summary>
        public ITypeInspector Inspector { get; set; }

        /// <summary>
        /// Create an instance of an ObjectInfo without a name.
        /// </summary>
        public ObjectInfo(object value)
        {
            _value = value;
        }

        /// <summary>
        /// Create an instance of an ObjectInfo with a name.
        /// </summary>
	    public ObjectInfo(string name, object value)
        {
            Name = name;
	        _value = value;
	    }

	    /// <summary>
	    /// The value to be printed
	    /// </summary>
	    public object Value
		{
			get { return _value; } 
			set { _value = value; _type = null; }
		}

		/// <summary>
		/// The Type of the value to be printed
		/// </summary>
		public Type Type
		{
			get { return _type ?? (_type = Value.GetType()); }
		}
	}
}