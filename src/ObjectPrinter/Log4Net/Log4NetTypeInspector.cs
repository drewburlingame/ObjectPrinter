using System;
using ObjectPrinter.TypeInspectors;

namespace ObjectPrinter.Log4Net
{
	public class Log4NetTypeInspector : ToStringTypeInspector
	{
		public override bool ShouldInspect(object objectToInspect, Type typeOfObjectToInspect)
		{
			return typeOfObjectToInspect.Namespace != null
			       && typeOfObjectToInspect.Namespace.StartsWith("log4net");
		}
	}
}