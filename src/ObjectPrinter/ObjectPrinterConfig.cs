using System;
using System.Collections.Generic;
using System.Linq;
using ObjectPrinter.TypeInspectors;

namespace ObjectPrinter
{
	public class ObjectPrinterConfig : IObjectPrinterConfig
	{
		public static ITypeInspector DefaultTypeInspector = new InspectAllTypeInspector();

		public static string DefaultTab = "\t";
        public static string DefaultNewLine = Environment.NewLine;
        public static int DefaultMaxDepth = 10;
		public static bool DefaultIncludeLogging = false;

		private static ITypeInspector[] _inspectorsWithAllTypesInspected = new []
		            {
		                new EnumTypeInspector(),
		                new ExceptionTypeInspector(),
		                DefaultTypeInspector
		            };

		private static ITypeInspector[] _inspectorsWithMsTypesToStringed = new []
			      	{
			      		new EnumTypeInspector(),
			      		new ExceptionTypeInspector(),
			      		new ToStringTypeInspector { ShouldInspectType = Funcs.IncludeMsBuiltInNamespaces },
			      		DefaultTypeInspector
			      	};

		public static IEnumerable<ITypeInspector> InspectorsWithAllTypesInspected
		{
			get { return _inspectorsWithAllTypesInspected; }
		}

		public static IEnumerable<ITypeInspector> InspectorsWithMsTypesToStringed
		{
			get { return _inspectorsWithMsTypesToStringed; }
		}

		/// <summary>
		/// Delegate to return the inspectors to use and the order to use them in.
		/// Change this at runtime to update the inspectors used and their order.
		/// </summary>
		public static Func<IEnumerable<ITypeInspector>> GetInspectors = () => InspectorsWithMsTypesToStringed;

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
        /// If none return true, the DefaultTypeInspector is used
        /// </summary>
		public ITypeInspector GetInspector(object objectToInspect, Type typeOfObjectToInspect)
		{
			return Inspectors
			       	.FirstOrDefault(inspector => inspector.ShouldInspect(objectToInspect, typeOfObjectToInspect))
			       ?? DefaultTypeInspector;
		}
	}
}