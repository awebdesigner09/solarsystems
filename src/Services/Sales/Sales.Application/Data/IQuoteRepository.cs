namespace Sales.Application.Data
{
    public interface IQuoteRepository
    {
        Task<Quote> GetQuoteAsync(Guid quoteId, CancellationToken cancellationToken = default);
        Task<Quote> StoreQuoteAsync(Quote quote, CancellationToken cancellationToken = default);
        Task<bool> DeleteQuoteAync(Guid quoteId, CancellationToken cancellationToken = default);
    }
}
