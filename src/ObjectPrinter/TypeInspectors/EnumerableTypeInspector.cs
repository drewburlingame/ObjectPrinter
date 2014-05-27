using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ObjectPrinter.TypeInspectors
{
    public class EnumerableTypeInspector : ITypeInspector
    {
        public bool ShouldInspect(object objectToInspect, Type typeOfObjectToInspect)
        {
            return typeof(IEnumerable).IsAssignableFrom(typeOfObjectToInspect);
        }

        public IEnumerable<ObjectInfo> GetMemberList(object objectToInspect, Type typeOfObjectToInspect)
        {
            return from Object obj in (IEnumerable)objectToInspect
                   select new ObjectInfo(obj.RemoveNonSerializableWrapper());
        }
    }
}