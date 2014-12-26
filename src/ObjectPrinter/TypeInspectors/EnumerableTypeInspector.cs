using System;
using System.Collections;
using System.Collections.Generic;

namespace ObjectPrinter.TypeInspectors
{
    /// <summary>
    /// Inspects every item in the enumerable
    /// </summary>
    public class EnumerableTypeInspector : ITypeInspector
    {
        /// <summary>
        /// when true, Count will be the first property printed
        /// </summary>
        public bool IncludeCountsForCollections { get; set; }

        public EnumerableTypeInspector()
        {
            IncludeCountsForCollections = Config.Inspectors.IncludeCountsForCollections;
        }

        ///<summary></summary>
        public bool ShouldInspect(object objectToInspect, Type typeOfObjectToInspect)
        {
            return typeof(IEnumerable).IsAssignableFrom(typeOfObjectToInspect);
        }

        ///<summary></summary>
        public IEnumerable<ObjectInfo> GetMemberList(object objectToInspect, Type typeOfObjectToInspect)
        {
            if (IncludeCountsForCollections)
            {
                var collection = objectToInspect as ICollection;
                if (collection != null && collection.Count > 0)
                {
                    yield return new ObjectInfo("Count", collection.Count);
                }
            }

            var enumerable = (IEnumerable) objectToInspect;
            foreach (var obj in enumerable)
            {
                yield return new ObjectInfo(obj.RemoveNonSerializableWrapper());
            }
        }
    }
}