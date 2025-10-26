using Sales.Application.Sales.Commands.Delete;

namespace Sales.API.Endpoints.Customer
{
    public record DeleteCustomerResponse(bool IsSuccess);
    public class DeleteCustomer : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/customers/{id}", async (Guid Id, ISender sender) =>
            {
                var result = await sender.Send(new DeleteCustomerCommand(Id));
                var response = new DeleteCustomerResponse(result.IsSuccess);
                return Results.Ok(response);
            })
            .WithName("DeleteCustomer")
            .Produces<DeleteCustomerResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Customer")
            .WithDescription("Deletes a Customer");
        }
    }
}
