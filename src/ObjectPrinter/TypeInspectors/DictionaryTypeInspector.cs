using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ObjectPrinter.TypeInspectors
{
    public class DictionaryTypeInspector : ITypeInspector
    {
        public bool ShouldInspect(object objectToInspect, Type typeOfObjectToInspect)
        {
            return typeof(IDictionary).IsAssignableFrom(typeOfObjectToInspect);
        }

        public IEnumerable<ObjectInfo> GetMemberList(object objectToInspect, Type typeOfObjectToInspect)
        {
            return from DictionaryEntry entry in (IDictionary)objectToInspect
                   select new ObjectInfo(entry.Key.ToString(), entry.Value.RemoveNonSerializableWrapper());
        }
    }
}