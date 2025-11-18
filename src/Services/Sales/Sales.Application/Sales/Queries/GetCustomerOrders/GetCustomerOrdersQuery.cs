namespace Sales.Application.Sales.Queries.GetCustomerOrders
{
    public record GetCustomerOrdersQuery(Guid customerId) : IQuery<GetCustomerOrdersResult>;
    public record GetCustomerOrdersResult(IEnumerable<OrderSummaryDto> Orders);

}
