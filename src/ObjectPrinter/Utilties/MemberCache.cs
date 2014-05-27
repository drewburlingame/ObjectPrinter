using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace ObjectPrinter.Utilties
{
    /// <summary>
    /// Caches the member information used to inspect a type.
    /// </summary>
    public class MemberCache : IMemberCache
    {
        private readonly BindingFlags _bindingFlags;

        private readonly ConcurrentDictionary<Type, IEnumerable<PropertyInfo>> _propertyCache = new ConcurrentDictionary<Type, IEnumerable<PropertyInfo>>();
        private readonly ConcurrentDictionary<Type, IEnumerable<FieldInfo>> _fieldCache = new ConcurrentDictionary<Type, IEnumerable<FieldInfo>>();
        private readonly ConcurrentDictionary<Type, IEnumerable<MethodInfo>> _methodCache = new ConcurrentDictionary<Type, IEnumerable<MethodInfo>>();

        /// <summary>
        /// Creates a new instance of MemberCache
        /// </summary>
        /// <param name="bindingFlags"></param>
        public MemberCache(BindingFlags bindingFlags)
        {
            _bindingFlags = bindingFlags;
        }

        /// <summary>
        /// Returns the properties to inspect
        /// </summary>
        public IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            IEnumerable<PropertyInfo> members;

            if (!_propertyCache.TryGetValue(type, out members))
            {
                _propertyCache[type] = members = type.GetProperties(_bindingFlags);
            }

            return members;
        }

        /// <summary>
        /// Returns the fields to inspect
        /// </summary>
        public IEnumerable<FieldInfo> GetFields(Type type)
        {
            IEnumerable<FieldInfo> members;

            if (!_fieldCache.TryGetValue(type, out members))
            {
                _fieldCache[type] = members = type.GetFields(_bindingFlags);
            }

            return members;
        }

        /// <summary>
        /// Returns the methods to inspect
        /// </summary>
        public IEnumerable<MethodInfo> GetMethods(Type type)
        {
            IEnumerable<MethodInfo> members;

            if (!_methodCache.TryGetValue(type, out members))
            {
                _methodCache[type] = members = type.GetMethods(_bindingFlags);
            }

            return members;
        }
    }
}