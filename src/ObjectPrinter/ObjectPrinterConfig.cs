using System;
using System.Collections.Generic;
using System.Linq;
using ObjectPrinter.TypeInspectors;

namespace ObjectPrinter
{
    public class ObjectPrinterConfig : IObjectPrinterConfig
	{
	    ///<summary>
	    /// The catch-all inspector to use if no inspector is found to inspect a given type.  
	    /// "Default" as in the default in a switch statement.
	    /// This will be appended to all configs.
	    ///</summary>
		public static ITypeInspector CatchAllTypeInspector = new InspectAllTypeInspector();

	    ///<summary>The default tab character</summary>
		public static string DefaultTab = "\t";

	    ///<summary>The default newline character</summary>
        public static string DefaultNewLine = Environment.NewLine;

	    ///<summary>
	    /// The default depth to recurse into the object graph.  Default is 10.
	    /// This guards against self-referencing value types, like those found in Linq2Sql data definitions.
	    ///</summary>
        public static int DefaultMaxDepth = 10;

	    ///<summary>
	    /// The default setting for including ObjectPrinter logging in the output
	    ///</summary>
		public static bool DefaultIncludeLogging = false;

        public static IEnumerable<ITypeInspector> DefaultTypeInspectors =
            new TypeInspectorsRegistration().GetRegisteredInspectors();

		/// <summary>
		/// Delegate to return the inspectors to use and the order to use them in.
		/// Change this at runtime to update the inspectors used and their order.
		/// </summary>
		public static Func<IEnumerable<ITypeInspector>> GetInspectors = () => DefaultTypeInspectors;

	    private IEnumerable<ITypeInspector> _inspectors;

	    ///<summary>
	    /// The string to use as a tab.  
	    /// Use html characters if outputting to html without the 'pre' element.
	    /// If not specified, DefaultTab (\t) is used.
	    ///</summary>
        public string Tab { get; set; }

        ///<summary>
        /// The string to use as a newline.  
        /// Use html characters if outputting to html without the 'pre' element.
        /// If not specified, DefaultNewLine (\n) is used.
        ///</summary>
		public string NewLine { get; set; }

        ///<summary>
        /// The maximum depth of recursions into the object graph.
        /// This is useful for escaping an issue with value types 
        /// where we can't reliably determine same instance.
        /// If not specified, DefaultMaxDepth (10) is used.
        ///</summary>
        public int MaxDepth { get; set; }

        ///<summary>
        /// When true, the inspector used for an object is specified in the output.
        /// If not specified, DefaultIncludeLogging (false) is used.
        ///</summary>
		public bool IncludeLogging { get; set; }

		/// <summary>
		/// The type inspectors to be used during the printing of an object.
        /// If not specified, will use the GetInspectors static factory delegate.
		/// </summary>
		public IEnumerable<ITypeInspector> Inspectors
		{
		    get { return _inspectors ?? (_inspectors = GetInspectors()); }
		    set { _inspectors = value; }
		}

	    public ObjectPrinterConfig()
		{
			Tab = DefaultTab;
			NewLine = DefaultNewLine;
			MaxDepth = DefaultMaxDepth;
			IncludeLogging = DefaultIncludeLogging;
		}


        /// <summary>
        /// Returns the first ITypeInspector from Inspectors where ShouldInspect returns true.
        /// If none return true, the CatchAllTypeInspector is used
        /// </summary>
		public ITypeInspector GetInspector(object objectToInspect, Type typeOfObjectToInspect)
		{
			return Inspectors
			       	.FirstOrDefault(inspector => inspector.ShouldInspect(objectToInspect, typeOfObjectToInspect))
			       ?? CatchAllTypeInspector;
		}
	}
}