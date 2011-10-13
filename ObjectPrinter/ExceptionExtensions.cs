using System;

namespace ObjectPrinter
{
	public static class ExceptionExtensions
	{
		public const string CacheKey = "Cached_DetailedToString";

		///<summary>
		/// Uses the ObjectPrinter to loop through the properties of the object, dumping them to a string.  
		/// Includes special formatting to put all exception properties before the Exception.Data contents.
		/// </summary>
		public static string DumpToString(this Exception e)
		{
			return e.DumpToString(ObjectPrinterConfig.DefaultTab, ObjectPrinterConfig.DefaultNewLine);
		}

		///<summary>
		/// Uses the ObjectPrinter to loop through the properties of the object, dumping them to a string.  
		/// Includes special formatting to put all exception properties before the Exception.Data contents.
		/// </summary>
		public static string DumpToString(this Exception e, string tab, string newline)
		{
			if (!e.Data.Contains(CacheKey))
			{
				var output = new ObjectPrinter(e, tab, newline).Print();
				e.Data[CacheKey] = output;
			}
			return (string)e.Data[CacheKey];
		}

		public static void SetContext(this Exception e, string name, object context, bool ifNotSerializablePrintOnSerialize = false)
		{
			if (!context.GetType().IsSerializable)
			{
				context = new NonSerializableWrapper {Context = context, PrintOnSerialize = ifNotSerializablePrintOnSerialize};
			}
			e.Data[name] = context;
		}
	}
}