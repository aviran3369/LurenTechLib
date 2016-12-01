using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace LurenTech.Utilities.Caching
{
    public static class CacheHelper
    {
        private const string CACHE_KEYS = "%ALL_CACHE_KEYS%";
        private static IMemoryCache MemoryCache;

        private static List<string> Keys
        {
            get
            {
                List<string> _keys;

                var keys = Get(CACHE_KEYS);

                if (keys != null && keys is List<string>)
                    _keys = (List<string>)keys;
                else
                    _keys = new List<string>();

                return _keys;
            }
        }

        public static void Configure(IMemoryCache memoryCache)
        {
            MemoryCache = memoryCache;
        }

        public static void Set<T>(string key, T value)
        {
            MemoryCache.Set<T>(key, value);

            if (key != CACHE_KEYS && !Keys.Any(x => x == key))
            {
                Keys.Add(key);
                Set<List<string>>(CACHE_KEYS, Keys);
            }
        }

        public static void Remove(string key)
        {
            MemoryCache.Remove(key);
            Keys.RemoveAll(k => k == key);
        }

        public static object Get(string key)
        {
            object value;
            MemoryCache.TryGetValue(key, out value);
            return value;
        }

        public static void ClearAllCache()
        {
            while (Keys.Any())
            {
                string k = Keys.Take(1).Single();
                Remove(k);
            }
        }
    }
}
