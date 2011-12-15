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
			string enumValues;

			var isFlagEnum = typeOfObjectToInspect.GetCustomAttributes(typeof (FlagsAttribute), false).Any(a => true);
			if(isFlagEnum)
			{
				var allValues = Enum.GetValues(typeOfObjectToInspect);
				enumValues = allValues.Cast<Enum>().Where(objAsEnum.HasFlag).JoinToString(" | ");
			}
			else
			{
				enumValues = objectToInspect.ToString();
			}

			return new List<ObjectInfo>
			       	{
			       		new ObjectInfo
			       			{
			       				Value = typeOfObjectToInspect.Name
			       				        + "."
			       				        + enumValues
			       			}
			       	};
		}
	}
}