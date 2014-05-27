using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace ObjectPrinter.TypeInspectors
{
    public class NameValueCollectionTypeInspector : ITypeInspector
    {
        public bool ShouldInspect(object objectToInspect, Type typeOfObjectToInspect)
        {
            return typeof(NameValueCollection).IsAssignableFrom(typeOfObjectToInspect);
        }

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