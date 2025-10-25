using Sales.Application.Sales.Commands.CreateCustomer;

namespace Sales.API.Endpoints.Customer
{
    public record CreateCustomerRequest(CustomerDto Customer);
    public record CreateCustomerResponse(Guid Id);
    public class CreateCustomer : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/customers", async (CreateCustomerRequest request, ISender sender) =>
            {
                var command = new CreateCustomerCommand(request.Customer);
                var result = await sender.Send(command);
                var response = new CreateCustomerResponse(result.Id);
                return Results.Created($"/customers/{response.Id}", response);
            })
            .WithName("CreateCustomer")
            .Produces<CreateCustomerResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Customer")
            .WithDescription("Creates new Customer");
        }
    }
}
