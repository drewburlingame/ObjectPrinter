using System;
using System.Collections.Generic;
using System.Reflection;
using ObjectPrinter.TypeInspectors;

namespace ObjectPrinter.Utilties
{
    ///<summary>Big Al (Alzheimers) doesn't remember a thing.  No caching happens here.</summary>
    public class BigAlMemberCache : IMemberCache
    {
        private BindingFlags _bindingFlags;

        public BigAlMemberCache(BindingFlags bindingFlags)
        {
            _bindingFlags = bindingFlags;
        }

        public IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            return type.GetProperties(_bindingFlags);
        }

        public IEnumerable<FieldInfo> GetFields(Type type)
        {
            return type.GetFields(_bindingFlags);
        }

        public IEnumerable<MethodInfo> GetMethods(Type type)
        {
            return type.GetMethods(_bindingFlags);
        }
    }
}