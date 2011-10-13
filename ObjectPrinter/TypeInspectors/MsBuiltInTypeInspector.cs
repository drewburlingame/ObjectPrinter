using System;

namespace ObjectPrinter.TypeInspectors
{
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