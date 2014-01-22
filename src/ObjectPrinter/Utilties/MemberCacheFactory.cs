using System.Collections.Generic;
using System.Reflection;

namespace ObjectPrinter.Utilties
{
    public class MemberCacheFactory
    {
        public static MemberCacheFactory Instance = new MemberCacheFactory();
        private readonly IDictionary<BindingFlags, MemberCache> _cache = new Dictionary<BindingFlags, MemberCache>();
        
        public IMemberCache Get(bool enableCaching, BindingFlags bindingFlags)
        {
            if (!enableCaching)
            {
                return new BigAlMemberCache(bindingFlags);
            }

            MemberCache cache;
            lock (_cache)
            {
                if (!_cache.TryGetValue(bindingFlags, out cache))
                {
                    _cache[bindingFlags] = cache = new MemberCache(bindingFlags);
                }
            }
            return cache;
        }

        public void Clear()
        {
            lock (_cache)
            {
                _cache.Clear();
            }
        }
    }
}