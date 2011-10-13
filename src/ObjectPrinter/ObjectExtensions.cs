namespace ObjectPrinter
{
	public static class ObjectExtensions
	{
		///<summary>Uses the ObjectPrinter to loop through the properties of the object, dumping them to a string.</summary>
		public static string DumpToString(this object obj)
		{
			return new ObjectPrinter(obj).Print();
		}

		///<summary>Uses the ObjectPrinter to loop through the properties of the object, dumping them to a string.</summary>
		public static string DumpToString(this object obj, string tab, string newline)
		{
			return new ObjectPrinter(obj, tab, newline).Print();
		}
	}
}
