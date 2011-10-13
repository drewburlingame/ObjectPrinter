using System;
using System.Collections.Generic;

namespace ObjectPrinter.TypeInspectors
{
	public interface ITypeInspector
	{
		bool ShouldInspect(object objectToInspect, Type typeOfObjectToInspect);
		IEnumerable<ObjectInfo> GetMemberList(object objectToInspect, Type typeOfObjectToInspect);
	}
}