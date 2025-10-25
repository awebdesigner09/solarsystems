using BuildingBlocks.Pagination;

namespace Sales.Application.Sales.Queries.GetCustomers
{
    public class GetCustomersHandler(IApplicationDbContext dbContext) 
        : IQueryHandler<GetCustomersQuery, GetCustomersResult>
    {
        public async Task<GetCustomersResult> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            var pageIndex = request.PaginationRequest.PageIndex;
            var pageSize = request.PaginationRequest.PageSize;
            var totalCount = await dbContext.Customers.LongCountAsync(cancellationToken);
            var customers = await dbContext.Customers
                .OrderBy(c => c.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
           return new GetCustomersResult(
                new PaginatedResult<CustomerDto>(
                    pageIndex,
                    pageSize,
                    totalCount,
                    customers.ToCustomerDtoList()));
            
        }
    }
}
