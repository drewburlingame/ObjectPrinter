using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ObjectPrinter.TypeInspectors
{
    /// <summary>
    /// inspects each entry as kev/value pair, 
    /// where the key is ToString'd as the property name
    /// and the value is inspected as any other value would be.
    /// we welcome pull requests that will be more expressive when the 
    /// key is not cleanly converted to a string.
    /// </summary>
    public class DictionaryTypeInspector : ITypeInspector
    {
        ///<summary></summary>
        public bool ShouldInspect(object objectToInspect, Type typeOfObjectToInspect)
        {
            return typeof(IDictionary).IsAssignableFrom(typeOfObjectToInspect);
        }

        ///<summary></summary>
        public IEnumerable<ObjectInfo> GetMemberList(object objectToInspect, Type typeOfObjectToInspect)
        {
            foreach (DictionaryEntry entry in ((IDictionary) objectToInspect))
            {
                yield return new ObjectInfo(entry.Key.ToString(), entry.Value.RemoveNonSerializableWrapper());
            }
        }
    }
}