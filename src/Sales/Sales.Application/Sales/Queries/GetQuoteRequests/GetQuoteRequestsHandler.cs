using BuildingBlocks.Pagination;

namespace Sales.Application.Sales.Queries.GetQuoteRequests
{
    public class GetQuoteRequestsHandler(IApplicationDbContext dbContext) :
        IQueryHandler<GetQuoteRequestsQuery, GetQuoteRequestsResult>
    {
        public async Task<GetQuoteRequestsResult> Handle(GetQuoteRequestsQuery request, CancellationToken cancellationToken)
        {
            var pageIndex = request.PaginationRequest.PageIndex;
            var pageSize = request.PaginationRequest.PageSize;
            var totalCount = await dbContext.QuoteRequests.CountAsync(cancellationToken);
            var quoteRequests = await dbContext.QuoteRequests
                .AsNoTracking()
                .OrderBy(qr => qr.CreatedAt)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new GetQuoteRequestsResult(
                new PaginatedResult<QuoteRequestDto>(
                    pageIndex,
                    pageSize,
                    totalCount,
                    quoteRequests.ToQuoteRequestDtoList()));
        }
    }
}
