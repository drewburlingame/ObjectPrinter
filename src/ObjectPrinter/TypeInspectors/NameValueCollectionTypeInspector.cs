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
        /// <summary>
        /// when true, Count will be the first property printed
        /// </summary>
        public bool IncludeCountsForCollections { get; set; }

        public NameValueCollectionTypeInspector()
        {
            IncludeCountsForCollections = Config.Inspectors.IncludeCountsForCollections;
        }

        ///<summary></summary>
        public bool ShouldInspect(object objectToInspect, Type typeOfObjectToInspect)
        {
            return typeof(NameValueCollection).IsAssignableFrom(typeOfObjectToInspect);
        }

        ///<summary></summary>
        public IEnumerable<ObjectInfo> GetMemberList(object objectToInspect, Type typeOfObjectToInspect)
        {
            var nvc = (NameValueCollection) objectToInspect;
            if (IncludeCountsForCollections && nvc.Count > 0)
            {
                yield return new ObjectInfo("Count", nvc.Count);
            }
            for (int i = 0; i < nvc.Count; i++)
            {
                yield return new ObjectInfo(nvc.Keys[i], nvc[i]);
            }
        }
    }
}