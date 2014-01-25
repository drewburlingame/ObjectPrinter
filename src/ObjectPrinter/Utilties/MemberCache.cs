using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace ObjectPrinter.Utilties
{
    public class MemberCache : IMemberCache
    {
        private readonly BindingFlags _bindingFlags;

        private readonly ConcurrentDictionary<Type, IEnumerable<PropertyInfo>> _propertyCache = new ConcurrentDictionary<Type, IEnumerable<PropertyInfo>>();
        private readonly ConcurrentDictionary<Type, IEnumerable<FieldInfo>> _fieldCache = new ConcurrentDictionary<Type, IEnumerable<FieldInfo>>();
        private readonly ConcurrentDictionary<Type, IEnumerable<MethodInfo>> _methodCache = new ConcurrentDictionary<Type, IEnumerable<MethodInfo>>();

        public MemberCache(BindingFlags bindingFlags)
        {
            _bindingFlags = bindingFlags;
        }

        public IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            IEnumerable<PropertyInfo> members;

            if (!_propertyCache.TryGetValue(type, out members))
            {
                _propertyCache[type] = members = type.GetProperties(_bindingFlags);
            }

            return members;
        }
        public IEnumerable<FieldInfo> GetFields(Type type)
        {
            IEnumerable<FieldInfo> members;

            if (!_fieldCache.TryGetValue(type, out members))
            {
                _fieldCache[type] = members = type.GetFields(_bindingFlags);
            }

            return members;
        }
        public IEnumerable<MethodInfo> GetMethods(Type type)
        {
            IEnumerable<MethodInfo> members;

            if (!_methodCache.TryGetValue(type, out members))
            {
                _methodCache[type] = members = type.GetMethods(_bindingFlags);
            }

            return members;
        }

        public void Clear()
        {
            _propertyCache.Clear();
            _fieldCache.Clear();
            _methodCache.Clear();
        }
    }
}