namespace ObjectPrinter.TypeInspectors
{
	/// <summary>
	/// returns the ToString() represenation for every object in the "System*" and "Microsoft*" namespaces.
	/// who really wants the char array or length from a string?
	/// </summary>
	public class IgnoreMsBuiltInTypesTypeInspector : IgnoreNamespacesTypeInspector
	{
		public IgnoreMsBuiltInTypesTypeInspector() : base("System", "Microsoft")
		{
		}
	}
}