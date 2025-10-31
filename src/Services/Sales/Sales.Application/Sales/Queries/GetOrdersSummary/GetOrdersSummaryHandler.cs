
using BuildingBlocks.Pagination;

namespace Sales.Application.Sales.Queries.GetAllOrders
{
    public class GetOrdersSummaryHandler(IApplicationDbContext dbContext)
        : IQueryHandler<GetOrdersSummaryQuery, GetOrdersSummaryResult>
    {
        public async Task<GetOrdersSummaryResult> Handle(GetOrdersSummaryQuery request, CancellationToken cancellationToken)
        {
            var pageIndex = request.PaginationRequest.PageIndex;
            var pageSize = request.PaginationRequest.PageSize;
            var totalCount = await dbContext.Orders.LongCountAsync(cancellationToken);

            var orderSummaries = await (from order in dbContext.Orders
                                        join quote in dbContext.Quotes on order.QuoteId equals quote.Id
                                        join quoteRequest in dbContext.QuoteRequests on quote.QuoteRequestId equals quoteRequest.Id
                                        join customer in dbContext.Customers on quoteRequest.CustomerId equals customer.Id
                                        join systemModel in dbContext.SystemModels on quoteRequest.SystemModelId equals systemModel.Id
                                        // Select raw data into an anonymous type first
                                        select new
                                        {
                                            order.Id,
                                            order.QuoteId,
                                            systemModel.Name,
                                            customer.Address.City,
                                            customer.Address.State,
                                            quote.TotalPrice,
                                            order.CreatedAt,
                                            order.Status,
                                            order.LastModified
                                        })
                                .OrderByDescending(o => o.CreatedAt) // Use the property name from the anonymous type
                                .Skip(pageSize * pageIndex)
                                .Take(pageSize)
                                .ToListAsync(cancellationToken) // Execute query, fetch raw data
                                .ConfigureAwait(false); // Optional: if using in an async context

            // Project to the DTO on the client side
            var orderSummaryDtos = orderSummaries
                .Select(o => new OrderSummaryDto(
                    o.Id.Value,
                    o.QuoteId.Value,
                    o.Name,
                    o.City,
                    o.State,
                    o.TotalPrice,
                    o.CreatedAt,
                    o.Status,
                    o.LastModified
                ))
                .ToList();

           
            return new GetOrdersSummaryResult(
                new PaginatedResult<OrderSummaryDto>(
                    pageIndex,
                    pageSize,
                    totalCount,
                    orderSummaryDtos));
        }
    }
}