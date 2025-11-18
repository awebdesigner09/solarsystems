using Sales.API.Endpoints.QuoteRequest;
using Sales.Application.Sales.Queries.GetCustomerOrders;
using Sales.Application.Sales.Queries.GetCustomerQuoteRequests;

namespace Sales.API.Endpoints.Order
{
    public record GetCustomerOrdersResponse(IEnumerable<OrderSummaryDto> Orders);
    public class GetCustomerOrders : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/orders/{id}", async (Guid Id, ISender sender) =>
            {
                var result = await sender.Send(new GetCustomerOrdersQuery(Id));
                var response = new GetCustomerOrdersResponse(result.Orders);
                return Results.Ok(response);
            })
            .WithName("GetCustomerOrders")
            .Produces<GetCustomerOrdersResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Orders for customerId")
            .WithDescription("Retrieves list of Orders for the customer")
            .RequireAuthorization("CustomerPolicy");
        }
    }
}
