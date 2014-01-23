using System;
using System.Collections.Generic;
using System.Linq;
using ObjectPrinter.TypeInspectors;

namespace ObjectPrinter
{
    public class ObjectPrinterConfig : IObjectPrinterConfig
	{
	    private IEnumerable<ITypeInspector> _inspectors;

	    ///<summary>
	    /// The string to use as a tab.  
	    /// Use html characters if outputting to html without the 'pre' element.
        /// If not specified, Config.Printer.Tab (\t) is used.
	    ///</summary>
        public string Tab { get; set; }

        ///<summary>
        /// The string to use as a newline.  
        /// Use html characters if outputting to html without the 'pre' element.
        /// If not specified, Config.Printer.NewLine (\n) is used.
        ///</summary>
		public string NewLine { get; set; }

        ///<summary>
        /// The maximum depth of recursions into the object graph.
        /// This is useful for escaping an issue with value types 
        /// where we can't reliably determine same instance.
        /// If not specified, Config.Printer.MaxDepth (10) is used.
        ///</summary>
        public int MaxDepth { get; set; }

        ///<summary>
        /// When true, the inspector used for an object is specified in the output.
        /// If not specified, Config.Printer.IncludeLogging (false) is used.
        ///</summary>
        public bool IncludeLogging { get; set; }

        /// <summary>
        /// When true an exception is being dumped to string, 
        /// the output will be cached in the Data property of the exception.
        /// For short lived exceptions with a lot of context, this can save considerable time.
        /// For long lived exceptions, it may be preferable to not take up the memory.
        /// For exceptions with little context, it may be cheap enough to dump again.
        /// If not specified, Config.Printer.EnableExceptionCaching (false) is used.
        /// </summary>
        public bool EnableExceptionCaching { get; set; }

        /// <summary>
        /// The type inspectors to be used during the printing of an object.
        /// If not specified, will use the GetInspectors static factory delegate.
        /// </summary>
        public IEnumerable<ITypeInspector> Inspectors
        {
            get { return _inspectors ?? (_inspectors = Config.Inspectors.DefaultInspectors); }
            set { _inspectors = value; }
        }

        public ObjectPrinterConfig()
		{
			Tab = Config.Printer.Tab;
            NewLine = Config.Printer.NewLine;
            MaxDepth = Config.Printer.MaxDepth;
            IncludeLogging = Config.Printer.IncludeLogging;
            EnableExceptionCaching = Config.Printer.EnableExceptionCaching;
		}


        /// <summary>
        /// Returns the first ITypeInspector from Inspectors where ShouldInspect returns true.
        /// If none return true, the CatchAllTypeInspector is used
        /// </summary>
		public ITypeInspector GetInspector(object objectToInspect, Type typeOfObjectToInspect)
		{
			return Inspectors
			       	.FirstOrDefault(inspector => inspector.ShouldInspect(objectToInspect, typeOfObjectToInspect))
			       ?? Config.Inspectors.CatchAllTypeInspector;
		}
	}
}