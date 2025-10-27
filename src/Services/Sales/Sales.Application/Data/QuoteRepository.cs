namespace Sales.Application.Data
{
    public class QuoteRepository(IApplicationDbContext dbContext) : IQuoteRepository
    {
       
        public async Task<Quote> GetQuoteAsync(Guid quoteId, CancellationToken cancellationToken = default)
        {
            var quote_Id = QuoteId.Of(quoteId);
            var quote = await dbContext.Quotes.FindAsync([quote_Id], cancellationToken);
            return quote is null ? throw new QuoteNotFoundException(quoteId) : quote;
        }
        public async Task<Quote> StoreQuoteAsync(Quote quote, CancellationToken cancellationToken = default)
        {
            dbContext.Quotes.Add(quote);
            await dbContext.SaveChangesAsync(cancellationToken);
            return quote;
        }
        public async Task<bool> DeleteQuoteAync(Guid quoteId, CancellationToken cancellationToken = default)
        {
            var quote_Id = QuoteId.Of(quoteId);
            var quote = await dbContext.Quotes.FindAsync([quote_Id], cancellationToken);
            if (quote is null)
                throw new QuoteNotFoundException(quoteId);
            dbContext.Quotes.Remove(quote);
            await dbContext.SaveChangesAsync(cancellationToken);
            return true;

        }

    }
}
