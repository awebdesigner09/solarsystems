using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Sales.Application.Data
{
    public class CachedQuoteRequestCounter(IDistributedCache cache) : IQuoteRequestCounter
    {
        public async Task<int> GetAsync(string customerId, string systemModelId, CancellationToken cancellationToken = default)
        {
            string cacheKey = $"customer:{customerId}:system:{systemModelId}";

            var count = await cache.GetStringAsync(cacheKey, cancellationToken);
            if (!string.IsNullOrEmpty(count))
                return int.Parse(count);

            await cache.SetStringAsync(cacheKey,"0", cancellationToken);
            return 0;
        }
        public async Task UpAsync(string customerId, string systemModelId, CancellationToken cancellationToken = default)
        {
            string cacheKey = $"customer:{customerId}:system:{systemModelId}";

            var count = await cache.GetStringAsync(cacheKey, cancellationToken);

            if (!string.IsNullOrEmpty(count))
                await cache.SetStringAsync(cacheKey, Convert.ToString(int.Parse(count) + 1), cancellationToken);
            else
                await cache.SetStringAsync(cacheKey, "0", cancellationToken);
        }
        public async Task DownAsync(string customerId, string systemModelId, CancellationToken cancellationToken = default)
        {
            string cacheKey = $"customer:{customerId}:system:{systemModelId}";

            var count = await cache.GetStringAsync(cacheKey, cancellationToken);

            if (!string.IsNullOrEmpty(count) && int.Parse(count) > 0)
                await cache.SetStringAsync(cacheKey, Convert.ToString(int.Parse(count) - 1), cancellationToken);
            else
                await cache.SetStringAsync(cacheKey, "0", cancellationToken);
        }
        public async Task DeleteAsync(string customerId, string systemModelId, CancellationToken cancellationToken = default)
        {
            string cacheKey = $"customer:{customerId}:system:{systemModelId}";
            await cache.RemoveAsync(cacheKey, cancellationToken);
        }
    }
}
