using System.Collections.Generic;

namespace ObjectPrinter.TypeInspectors
{
    public static class TypeInspectorExtensions
	{
		public static IEnumerable<ObjectInfo> GetMemberList(this ITypeInspector inspector, ObjectInfo objectInfo)
		{
			return inspector.GetMemberList(objectInfo.Value, objectInfo.Type);
		}
	}
}