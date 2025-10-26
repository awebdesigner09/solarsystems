
using BuildingBlocks.Pagination;

namespace Sales.Application.Sales.Queries.GetCustomers
{
    public record GetCustomersQuery(PaginationRequest PaginationRequest): IQuery<GetCustomersResult>;
    public record GetCustomersResult(PaginatedResult<CustomerDto> Customers);
}
