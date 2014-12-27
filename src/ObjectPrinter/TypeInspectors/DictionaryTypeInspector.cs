using System;
using System.Collections;
using System.Collections.Generic;

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
        /// <summary>
        /// when true, Count will be the first property printed
        /// </summary>
        public bool IncludeCountsForCollections { get; set; }

        public DictionaryTypeInspector()
        {
            IncludeCountsForCollections = Config.Inspectors.IncludeCountsForCollections;
        }

        ///<summary></summary>
        public bool ShouldInspect(object objectToInspect, Type typeOfObjectToInspect)
        {
            return typeof(IDictionary).IsAssignableFrom(typeOfObjectToInspect);
        }

        ///<summary></summary>
        public IEnumerable<ObjectInfo> GetMemberList(object objectToInspect, Type typeOfObjectToInspect)
        {
            var dictionary = (IDictionary) objectToInspect;
            if (IncludeCountsForCollections && dictionary.Count > 0)
            {
                yield return new ObjectInfo("Count", dictionary.Count);
            }
            foreach (DictionaryEntry entry in dictionary)
            {
                yield return new ObjectInfo(entry.Key.ToString(), entry.Value.RemoveNonSerializableWrapper());
            }
        }
    }
}