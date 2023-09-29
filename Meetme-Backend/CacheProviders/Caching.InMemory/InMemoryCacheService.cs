using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;
using MeetMe.Core.Interface;

namespace MeetMe.Caching.InMemory
{
    public class InMemoryCacheService : ICacheService
    {
        private readonly IMemoryCache memoryCache;

        public InMemoryCacheService(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public void Remove(string key)
        {
            memoryCache.Remove(key);
        }

        public T GetData<T>(string key)
        {
            return memoryCache.Get<T>(key);
        }
        public void SetData<T>(string key, T value, int aliveForSeconds = 30)
        {
            var slidingExpirationTime = TimeSpan.FromSeconds(aliveForSeconds);

            memoryCache.Set(key, value, slidingExpirationTime);
        }

        public async Task<T> GetOrAdd<T>(string key, Func<Task<T>> action, int aliveForSeconds = 30)
        {
            var item = await memoryCache.GetOrCreateAsync(key, async entry =>
            {
                var obj = new object();

                entry.SlidingExpiration = TimeSpan.FromSeconds(aliveForSeconds);

                var added = await action();
                return added;

            });

            return item;
        }
    }

}
