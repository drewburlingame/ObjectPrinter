using System;
using ObjectPrinter.TypeInspectors;

namespace ObjectPrinter
{
	public interface IObjectPrinterConfig
    {
        ///<summary>
        /// The string to use as a tab.  
        /// Use html characters if outputting to html without the 'pre' element.
        ///</summary>
        string Tab { get; }

        ///<summary>
        /// The string to use as a newline.  
        /// Use html characters if outputting to html without the 'pre' element.
        /// If not specified, DefaultNewLine (\n) is used.
        ///</summary>
        string NewLine { get; }

        ///<summary>
        /// The maximum depth of recursions into the object graph.
        /// This is useful for escaping an issue with value types 
        /// where we can't reliably determine same instance.
        /// If not specified, DefaultMaxDepth (10) is used.
        ///</summary>
        int MaxDepth { get; }

        ///<summary>
        /// When true, the inspector used for an object is specified in the output.
        /// If not specified, DefaultIncludeLogging (false) is used.
        ///</summary>
		bool IncludeLogging { get; }
		ITypeInspector GetInspector(object objectToInspect, Type typeOfObjectToInspect);
	}
}