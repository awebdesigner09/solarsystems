using BuildingBlocks.Pagination;
using Sales.Application.Sales.Queries.GetCustomerQuoteRequests;

namespace Sales.Application.Sales.Queries.GetQuoteRequests
{
    public class GetCustomerQuoteRequestsHandler(IApplicationDbContext dbContext) :
        IQueryHandler<GetCustomerQuoteRequestsQuery, GetCustomerQuoteRequestsResult>
    {
        public async Task<GetCustomerQuoteRequestsResult> Handle(GetCustomerQuoteRequestsQuery query, CancellationToken cancellationToken)
        {
            var quoteRequests = await dbContext.QuoteRequests
                .Where(qr => qr.CustomerId == CustomerId.Of(query.customerId))
                .AsNoTracking()
                .OrderByDescending(qr => qr.CreatedAt).ToListAsync();

            return new GetCustomerQuoteRequestsResult(quoteRequests.ToQuoteRequestDtoList());
        }
    }
}
