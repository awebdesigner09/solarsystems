using BuildingBlocks.Pagination;

namespace Sales.Application.Sales.Queries.GetCustomerQuoteRequests
{
    public record GetCustomerQuoteRequestsQuery(Guid customerId) : IQuery<GetCustomerQuoteRequestsResult>;
     public record GetCustomerQuoteRequestsResult(IEnumerable<QuoteRequestDto> QuoteRequests);

}
