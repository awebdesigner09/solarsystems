
using BuildingBlocks.Pagination;

namespace Sales.Application.Sales.Queries.GetQuoteRequests
{
    public record GetQuoteRequestsQuery(PaginationRequest PaginationRequest) : IQuery<GetQuoteRequestsResult>;
    public record GetQuoteRequestsResult(PaginatedResult<QuoteRequestDto> QuoteRequests);

}
