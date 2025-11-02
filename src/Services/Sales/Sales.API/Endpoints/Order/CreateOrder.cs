using Sales.Application.Sales.Commands.CreateOrder;

namespace Sales.API.Endpoints.Order
{
    public record CreateOrderRequest(OrderDto Order);
    public record CreateOrderResponse(Guid Id);
    public class CreateOrder : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/orders", async (CreateOrderRequest request, ISender sender) =>
            {
                var command = new CreateOrderCommand(request.Order);
                var result = await sender.Send(command);
                var response = new CreateOrderResponse(result.Id);
                return Results.Created($"/orders/{response.Id}", response);
            })
            .WithName("CreateOrder")
            .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Order")
            .WithDescription("Creates Order")
            .RequireAuthorization("CustomerPolicy");
        }
    }
}
