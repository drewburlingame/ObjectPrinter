using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ObjectPrinter.Utilties;

namespace ObjectPrinter.TypeInspectors
{
	/// <summary>Returns the full name of an enum</summary>
	public class EnumTypeInspector : ITypeInspector
	{
		public bool ShouldInspect(object objectToInspect, Type typeOfObjectToInspect)
		{
			return typeof(Enum).IsAssignableFrom(typeOfObjectToInspect);
		}

		public IEnumerable<ObjectInfo> GetMemberList(object objectToInspect, Type typeOfObjectToInspect)
		{
			var objAsEnum = (Enum)objectToInspect;
			var allValues = Enum.GetValues(typeOfObjectToInspect);

			return new List<ObjectInfo>
			       	{
			       		new ObjectInfo
			       			{
			       				Value = typeOfObjectToInspect.Name
			       				        + "."
			       				        + allValues
			       				          	.Cast<Enum>()
			       				          	.Where(objAsEnum.HasFlag)
			       				          	.JoinToString(" | ")
			       			}
			       	};
		}
	}
}