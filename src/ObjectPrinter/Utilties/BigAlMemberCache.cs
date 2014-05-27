using System;
using System.Collections.Generic;
using System.Reflection;

namespace ObjectPrinter.Utilties
{
    ///<summary>Big Al (Alzheimers) doesn't remember a thing.  No caching happens here.</summary>
    public class BigAlMemberCache : IMemberCache
    {
        private readonly BindingFlags _bindingFlags;

        /// <summary>
        /// Creates a new instance of BigAlMemberCache
        /// </summary>
        /// <param name="bindingFlags"></param>
        public BigAlMemberCache(BindingFlags bindingFlags)
        {
            _bindingFlags = bindingFlags;
        }

        /// <summary>
        /// Returns the properties to inspect, never caching the results
        /// </summary>
        public IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            return type.GetProperties(_bindingFlags);
        }

        /// <summary>
        /// Returns the fields to inspect, never caching the results
        /// </summary>
        public IEnumerable<FieldInfo> GetFields(Type type)
        {
            return type.GetFields(_bindingFlags);
        }

        /// <summary>
        /// Returns the methods to inspect, never caching the results
        /// </summary>
        public IEnumerable<MethodInfo> GetMethods(Type type)
        {
            return type.GetMethods(_bindingFlags);
        }
    }
}