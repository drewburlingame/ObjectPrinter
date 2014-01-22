using System;
using System.Collections.Generic;
using System.Reflection;

namespace ObjectPrinter.Utilties
{
    public class MemberCache : IMemberCache
    {
        private readonly BindingFlags _bindingFlags;

        private Dictionary<Type, IEnumerable<PropertyInfo>> _propertyCache = new Dictionary<Type, IEnumerable<PropertyInfo>>();
        private Dictionary<Type, IEnumerable<FieldInfo>> _fieldCache = new Dictionary<Type, IEnumerable<FieldInfo>>();
        private Dictionary<Type, IEnumerable<MethodInfo>> _methodCache = new Dictionary<Type, IEnumerable<MethodInfo>>();

        private readonly object _propertyLockObj = new object();
        private readonly object _fieldLockObj = new object();
        private readonly object _methodLockObj = new object();

        public MemberCache(BindingFlags bindingFlags)
        {
            _bindingFlags = bindingFlags;
        }

        public IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            IEnumerable<PropertyInfo> members;

            if (!_propertyCache.TryGetValue(type, out members))
            {
                lock (_propertyLockObj)
                {
                    if (!_propertyCache.TryGetValue(type, out members))
                    {
                        var newCache = new Dictionary<Type, IEnumerable<PropertyInfo>>(_propertyCache);
                        newCache[type] = members = type.GetProperties(_bindingFlags);
                        _propertyCache = newCache;
                    }
                }
            }

            return members;
        }
        public IEnumerable<FieldInfo> GetFields(Type type)
        {
            IEnumerable<FieldInfo> members;

            if (!_fieldCache.TryGetValue(type, out members))
            {
                lock (_fieldLockObj)
                {
                    if (!_fieldCache.TryGetValue(type, out members))
                    {
                        var newCache = new Dictionary<Type, IEnumerable<FieldInfo>>(_fieldCache);
                        newCache[type] = members = type.GetFields(_bindingFlags);
                        _fieldCache = newCache;
                    }
                }
            }

            return members;
        }
        public IEnumerable<MethodInfo> GetMethods(Type type)
        {
            IEnumerable<MethodInfo> members;

            if (!_methodCache.TryGetValue(type, out members))
            {
                lock (_methodLockObj)
                {
                    if (!_methodCache.TryGetValue(type, out members))
                    {
                        var newCache = new Dictionary<Type, IEnumerable<MethodInfo>>(_methodCache);
                        newCache[type] = members = type.GetMethods(_bindingFlags);
                        _methodCache = newCache;
                    }
                }
            }

            return members;
        }

        public void Clear()
        {
            lock (_propertyLockObj)
            {
                _propertyCache.Clear();
            }
            lock (_fieldLockObj)
            {
                _fieldCache.Clear();
            }
            lock (_methodLockObj)
            {
                _methodCache.Clear();
            }
        }
    }
}