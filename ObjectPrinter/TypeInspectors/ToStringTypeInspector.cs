using System;
using System.Collections.Generic;

namespace ObjectPrinter.TypeInspectors
{
	public abstract class ToStringTypeInspector : ITypeInspector
	{
		public abstract bool ShouldInspect(object objectToInspect, Type typeOfObjectToInspect);

		public IEnumerable<ObjectInfo> GetMemberList(object objectToInspect, Type typeOfObjectToInspect)
		{
			return new List<ObjectInfo>
			       	{
			       		new ObjectInfo
			       			{
			       				Value = objectToInspect.ToString()
			       			}
			       	};
		}
	}
}