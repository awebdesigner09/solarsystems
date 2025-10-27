using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Sales.Application.Data
{
    public class CachedQuoteRepository(IQuoteRepository repository, IDistributedCache cache)
        : IQuoteRepository
    {   
        public async Task<Quote> GetQuoteAsync(Guid quoteId, CancellationToken cancellationToken = default)
        {
            var cachedQuote = await cache.GetStringAsync(quoteId.ToString(), cancellationToken);
            if (!string.IsNullOrEmpty(cachedQuote))
                return JsonSerializer.Deserialize<Quote>(cachedQuote)!;

            var quote = await repository.GetQuoteAsync(quoteId, cancellationToken);
            await cache.SetStringAsync(quoteId.ToString(), JsonSerializer.Serialize(quote),cancellationToken);
            return quote;
        }
        public async Task<Quote> StoreQuoteAsync(Quote quote, CancellationToken cancellationToken = default)
        {
            await repository.StoreQuoteAsync(quote, cancellationToken);
            await cache.SetStringAsync(quote.Id.Value.ToString(), JsonSerializer.Serialize(quote), cancellationToken);
            return quote;
        }
        public async Task<bool> DeleteQuoteAync(Guid quoteId, CancellationToken cancellationToken = default)
        {
            await repository.DeleteQuoteAync(quoteId, cancellationToken);
            await cache.RemoveAsync(quoteId.ToString(), cancellationToken);
            return true;
        }

    }
}
