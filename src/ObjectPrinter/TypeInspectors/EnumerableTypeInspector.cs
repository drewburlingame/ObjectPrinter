using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ObjectPrinter.TypeInspectors
{
    /// <summary>
    /// Inspects every item in the enumerable
    /// </summary>
    public class EnumerableTypeInspector : ITypeInspector
    {
        ///<summary></summary>
        public bool ShouldInspect(object objectToInspect, Type typeOfObjectToInspect)
        {
            return typeof(IEnumerable).IsAssignableFrom(typeOfObjectToInspect);
        }

        ///<summary></summary>
        public IEnumerable<ObjectInfo> GetMemberList(object objectToInspect, Type typeOfObjectToInspect)
        {
            return from Object obj in (IEnumerable)objectToInspect
                   select new ObjectInfo(obj.RemoveNonSerializableWrapper());
        }
    }
}