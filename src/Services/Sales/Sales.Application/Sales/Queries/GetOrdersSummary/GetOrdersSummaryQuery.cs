using BuildingBlocks.Pagination;
namespace Sales.Application.Sales.Queries.GetAllOrders
{
    public record GetOrdersSummaryQuery(PaginationRequest PaginationRequest) : IQuery<GetOrdersSummaryResult>;
    public record GetOrdersSummaryResult(PaginatedResult<OrderSummaryDto> OrdersSummary);
}
