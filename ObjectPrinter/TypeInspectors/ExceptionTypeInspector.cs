using System;
using System.Collections.Generic;
using System.Linq;
using ObjectPrinter.Utilties;

namespace ObjectPrinter.TypeInspectors
{
	public class ExceptionTypeInspector : DefaultTypeInspector
	{
		public override bool ShouldInspect(object objectToInspect, Type typeOfObjectToInspect)
		{
			return typeof(Exception).IsAssignableFrom(typeOfObjectToInspect);
		}

		public override IEnumerable<ObjectInfo> GetMemberList(object objectToInspect, Type typeOfObjectToInspect)
		{
			return base.GetMemberList(objectToInspect, typeOfObjectToInspect)
				.Where(p => !p.Name.StartsWith("ToString"))
				.ForEach(p =>
				         	{
								if(p.Value is NonSerializableWrapper)
								{
									p.Value = ((NonSerializableWrapper)p.Value).Context;
								}
				         	})
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