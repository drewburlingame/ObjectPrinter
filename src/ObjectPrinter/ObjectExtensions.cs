using ObjectPrinter.Utilties;

namespace ObjectPrinter
{
	public static class ObjectExtensions
	{
		///<summary>Uses the ObjectPrinter to loop through the properties of the object, dumping them to a string.</summary>
		public static string DumpToString(this object obj)
		{
			return new ObjectPrinter(obj).PrintToString();
		}

		///<summary>Uses the ObjectPrinter to loop through the properties of the object, dumping them to a string.</summary>
		public static string DumpToString(this object obj, string tab, string newline)
		{
            return new ObjectPrinter(obj, tab, newline).PrintToString();
		}

		///<summary>Uses the ObjectPrinter to loop through the properties of the object, dumping them to a string.</summary>
		public static string DumpToString(this object obj, IObjectPrinterConfig config)
		{
            return new ObjectPrinter(obj, config).PrintToString();
		}

		///<summary>
		/// Creates a delegate that will use the ObjectPrinter ont the obj when ToString() is called on the delegate.
		/// Useful for delaying execution of the ObjectPrinter until absolutely needed. i.e. logging
		/// </summary>
		public static LazyString Dump(this object obj)
		{
            return new LazyString(() => new ObjectPrinter(obj).PrintToString());
		}

		///<summary>
		/// Creates a delegate that will use the ObjectPrinter ont the obj when ToString() is called on the delegate.
		/// Useful for delaying execution of the ObjectPrinter until absolutely needed. i.e. logging
		/// </summary>
		public static LazyString Dump(this object obj, string tab, string newline)
		{
            return new LazyString(() => new ObjectPrinter(obj, tab, newline).PrintToString());
		}

		///<summary>
		/// Creates a delegate that will use the ObjectPrinter ont the obj when ToString() is called on the delegate.
		/// Useful for delaying execution of the ObjectPrinter until absolutely needed. i.e. logging
		/// </summary>
		public static LazyString Dump(this object obj, IObjectPrinterConfig config)
		{
            return new LazyString(() => new ObjectPrinter(obj, config).PrintToString());
		}
	}
}
