using Sales.Domain.Models;

namespace Sales.Application.Sales.Queries.GetCustomerQuotes
{
    public class GetCustomerQuotesHandler(IApplicationDbContext dbContext) :
        IQueryHandler<GetCustomerQuotesQuery, GetCustomerQuotesResult>
    {
        public async Task<GetCustomerQuotesResult> Handle(GetCustomerQuotesQuery request, CancellationToken cancellationToken)
        {
            var customerId = CustomerId.Of(request.customerId);
            var customerQuotes = await (from quoterequest in dbContext.QuoteRequests
                                        .Where(qr => qr.CustomerId == customerId)
                                        join quote in dbContext.Quotes on quoterequest.Id equals quote.QuoteRequestId into quotes
                                        from q in quotes.DefaultIfEmpty()
                                        select new QuoteDto(
                                            q == null ? (Guid?)null : q.Id.Value,
                                            quoterequest.Id.Value,
                                            q.IssuedOn,
                                            q.ValidUntil,
                                            q.Components != null ? new ComponentsDto(q.Components.NoOfPanels, q.Components.NoOfInverters, q.Components.NoOfMoutingSystems, q.Components.NoOfBatteries) : null,
                                            q.BasePrice,
                                            q.Tax1,
                                            q.Tax2,
                                            q.TotalPrice
                                        )
                                        )
                                .ToListAsync(cancellationToken)
                                .ConfigureAwait(false);

            return new GetCustomerQuotesResult(customerQuotes);
        }
    }
}
