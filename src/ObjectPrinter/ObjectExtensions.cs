using System.IO;
using ObjectPrinter.Utilties;

namespace ObjectPrinter
{
	/// <summary>
	/// Extensions for dumping an Object to a TextWriter or String
	/// </summary>
	public static class ObjectExtensions
    {
        ///<summary>Uses the ObjectPrinter to loop through the properties of the object, dumping them to the provided TextWriter.</summary>
        public static void DumpTo(this object obj, TextWriter output, IObjectPrinterConfig config = null)
        {
            new Printers.ObjectPrinter(obj, config ?? Printers.ObjectPrinter.GetDefaultContext()).PrintTo(output);
        }

		///<summary>Uses the ObjectPrinter to loop through the properties of the object, dumping them to a string.</summary>
		public static string DumpToString(this object obj, IObjectPrinterConfig config = null)
		{
		    return obj.Dump(config).ToString();
		}

		///<summary>
		/// Creates a delegate that will use the ObjectPrinter ont the obj when ToString() is called on the delegate.
		/// Useful for delaying execution of the ObjectPrinter until absolutely needed. i.e. logging
		/// </summary>
		public static LazyString Dump(this object obj, IObjectPrinterConfig config = null)
		{
            return new LazyString(() => new Printers.ObjectPrinter(obj, config ?? Printers.ObjectPrinter.GetDefaultContext()).PrintToString());
		}

        ///<summary>
        /// Same as Dump, but less likely to conflict with Dump extension methods from other assemblies.
        /// </summary>
        public static LazyString LazyDump(this object obj, IObjectPrinterConfig config = null)
        {
            return new LazyString(() => new Printers.ObjectPrinter(obj, config ?? Printers.ObjectPrinter.GetDefaultContext()).PrintToString());
        }

        internal static object RemoveNonSerializableWrapper(this object objectInfo)
        {
            var wrapper = objectInfo as NonSerializableWrapper;
            if (wrapper == null)
            {
                return objectInfo;
            }

            return wrapper.Context;
        }
	}
}
