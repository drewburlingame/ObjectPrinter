using ObjectPrinter.Utilties;

namespace ObjectPrinter
{
	public static class ObjectExtensions
	{
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
	}
}
