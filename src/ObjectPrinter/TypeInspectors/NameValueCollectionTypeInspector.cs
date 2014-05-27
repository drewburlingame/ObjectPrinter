using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace ObjectPrinter.TypeInspectors
{
    /// <summary>
    /// inspects each entry in the NameValueCollection, the key as a property name and the value as a value
    /// </summary>
    public class NameValueCollectionTypeInspector : ITypeInspector
    {
        ///<summary></summary>
        public bool ShouldInspect(object objectToInspect, Type typeOfObjectToInspect)
        {
            return typeof(NameValueCollection).IsAssignableFrom(typeOfObjectToInspect);
        }

        ///<summary></summary>
        public IEnumerable<ObjectInfo> GetMemberList(object objectToInspect, Type typeOfObjectToInspect)
        {
            var nvc = (NameValueCollection) objectToInspect;
            for (int i = 0; i < nvc.Count; i++)
            {
                yield return new ObjectInfo(nvc.Keys[i], nvc[i]);
            }
        }
    }
}