using System;
using System.Collections.Generic;
using System.Reflection;

namespace ObjectPrinter.TypeInspectors
{
    public class MemberCache : IMemberCache
    {
        private readonly BindingFlags _bindingFlags;

        readonly Dictionary<Type, IEnumerable<PropertyInfo>> _propertyCache = new Dictionary<Type, IEnumerable<PropertyInfo>>();
        readonly Dictionary<Type, IEnumerable<FieldInfo>> _fieldCache = new Dictionary<Type, IEnumerable<FieldInfo>>();
        readonly Dictionary<Type, IEnumerable<MethodInfo>> _methodCache = new Dictionary<Type, IEnumerable<MethodInfo>>();

        public MemberCache(BindingFlags bindingFlags)
        {
            _bindingFlags = bindingFlags;
        }

        public IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            return Get(type, _propertyCache, () => type.GetProperties(_bindingFlags));
        }
        public IEnumerable<FieldInfo> GetFields(Type type)
        {
            return Get(type, _fieldCache, () => type.GetFields(_bindingFlags));
        }
        public IEnumerable<MethodInfo> GetMethods(Type type)
        {
            return Get(type, _methodCache, () => type.GetMethods(_bindingFlags));
        }

        private IEnumerable<TMember> Get<TMember>(
            Type type, 
            Dictionary<Type, IEnumerable<TMember>> cache,
            Func<IEnumerable<TMember>> getMembers)
        {
            IEnumerable<TMember> members;

            lock (cache)
            {
                if (!cache.TryGetValue(type, out members))
                {
                    cache[type] = members = getMembers();
                }
            }

            return members;
        }
    }
}