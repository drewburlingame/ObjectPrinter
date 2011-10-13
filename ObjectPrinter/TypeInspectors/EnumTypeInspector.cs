using System;
using System.Collections.Generic;

namespace ObjectPrinter.TypeInspectors
{
	public class EnumTypeInspector : ITypeInspector
	{
		public bool ShouldInspect(object objectToInspect, Type typeOfObjectToInspect)
		{
			return typeof(Enum).IsAssignableFrom(typeOfObjectToInspect);
		}

		public IEnumerable<ObjectInfo> GetMemberList(object objectToInspect, Type typeOfObjectToInspect)
		{
			return new List<ObjectInfo>
			       	{
			       		new ObjectInfo
			       			{
			       				Value = typeOfObjectToInspect.Name + "." + Enum.GetName(typeOfObjectToInspect, objectToInspect)
			       			}
			       	};
		}
	}
}