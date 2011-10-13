using System;
using ObjectPrinter.TypeInspectors;

namespace ObjectPrinter.Log4Net
{
	/// <summary>
	/// returns the ToString() represenation for every object in the "log4net*" namespaces, 
	/// as log4net intended.  all bow to log4net.
	/// </summary>
	public class Log4NetTypeInspector : ToStringTypeInspector
	{
		public override bool ShouldInspect(object objectToInspect, Type typeOfObjectToInspect)
		{
			return typeOfObjectToInspect.Namespace != null
			       && typeOfObjectToInspect.Namespace.StartsWith("log4net");
		}
	}
}