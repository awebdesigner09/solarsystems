using Sales.Application.Sales.Commands.UpdateCustomer;

namespace Sales.API.Endpoints.Customer
{
    public record UpdateCustomerRequest(CustomerDto Customer);
    public record UpdateCustomerResponse(bool IsSuccess);

    public class UpdateCustomer : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/customers", async (UpdateCustomerRequest request, ISender sender) =>
            {
                var command = new UpdateCustomerCommand(request.Customer);
                var result = await sender.Send(command);
                var response = new UpdateCustomerResponse(result.IsSuccess);
                return Results.Ok(response);
            })
            .WithName("UpdateCustomer")
            .Produces<UpdateCustomerResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Update Customer")
            .WithDescription("Updates an existing Customer");
        }
    }
}
