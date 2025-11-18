using Sales.Application.Sales.Queries.GetQuote;
using Sales.Application.Sales.Queries.GetSystemModels;

namespace Sales.Application.Sales.Queries.GetQuoteDetails
{
    public class GetQuoteHandler(IApplicationDbContext dbContext)
        : IQueryHandler<GetQuoteQuery, GetQuoteResult>
    {
        public async Task<GetQuoteResult> Handle(GetQuoteQuery request, CancellationToken cancellationToken)
        {
            var quoteId = QuoteId.Of(request.Id);
            var quote = await dbContext.Quotes.FindAsync([quoteId]);
            
            if (quote == null)
                throw new QuoteNotFoundException(quoteId.Value);

            return new GetQuoteResult(quote.ToQuoteDto());
        }
    }
}
