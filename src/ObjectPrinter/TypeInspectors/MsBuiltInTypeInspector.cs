using System;

namespace ObjectPrinter.TypeInspectors
{
	/// <summary>
	/// returns the ToString() represenation for every object in the "System*" and "Microsoft*" namespaces.
	/// who really wants the char array or length from a string?
	/// </summary>
	public class MsBuiltInTypeInspector : ToStringTypeInspector
	{
		public override bool ShouldInspect(object objectToInspect, Type typeOfObjectToInspect)
		{
			return typeOfObjectToInspect.Namespace != null &&
			       (typeOfObjectToInspect.Namespace.StartsWith("System")
			        || typeOfObjectToInspect.Namespace.StartsWith("Microsoft"));
		}
	}
}