using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace ObjectPrinter.Utilties
{
    internal class MemberCacheFactory
    {
        private static readonly ConcurrentDictionary<BindingFlags, IMemberCache> CacheByBindingFlags = new ConcurrentDictionary<BindingFlags, IMemberCache>();

        internal static Func<BindingFlags, IMemberCache> DefaultGetCacheDelegate = flags => new MemberCache(flags);
        
        internal static IMemberCache Get(bool enableCaching, BindingFlags bindingFlags)
        {
            return !enableCaching
                       ? new BigAlMemberCache(bindingFlags)
                       : CacheByBindingFlags.GetOrAdd(
                           bindingFlags,
                           bf => (Config.InspectAllTypeInspector.GetMemberCacheDelegate ?? DefaultGetCacheDelegate)(bf));
        }
    }
}