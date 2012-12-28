using System;
using System.Collections.Generic;
using System.Linq;
using ObjectPrinter.Utilties;

namespace ObjectPrinter.TypeInspectors
{
	/// <summary>
	/// Returns properties in a well known order to make it easier to scan the data
	/// </summary>
	public class ExceptionTypeInspector : InspectAllTypeInspector
	{
		public override bool ShouldInspect(object objectToInspect, Type typeOfObjectToInspect)
		{
			return base.ShouldInspect(objectToInspect, typeOfObjectToInspect)
			       && Funcs.IsException(objectToInspect, typeOfObjectToInspect);
		}

		public override IEnumerable<ObjectInfo> GetMemberList(object objectToInspect, Type typeOfObjectToInspect)
		{
			return base.GetMemberList(objectToInspect, typeOfObjectToInspect)
				.Where(p => !p.Name.StartsWith("ToString"))
				.OrderBy(Rank);
		}

		private static int Rank(ObjectInfo objectInfo)
		{
			switch (objectInfo.Name)
			{
				case "Message":
					return 10;
				case "StackTrace":
					return 20;
				case "Source":
					return 30;
				case "TargetSite":
					return 40;
				case "HelpLink":
					return 50;
					//exception specific properties are 60 & 70
				case "Data":
					return 80;
				case "InnerException":
					return 90;
			}

			return objectInfo.Value == null || objectInfo.Type.IsPrimitive ? 60 : 70;
		}
	}
}