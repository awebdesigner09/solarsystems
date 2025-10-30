using Sales.Application.Sales.Commands.DeleteSystemModel;

namespace Sales.API.Endpoints.SystemModel
{
    // Accepts the SystemModel ID to delete as parameter.
    // Constructs a DeleteSystemModelCommand and sends it to the mediator.
    // Returns a success response if deletion is successful.

    //public record DeleteSystemModelRequest(Guid SystemModelId);

    public record DeleteSystemModelResponse(bool IsSuccess);
    public class DeleteSystemModel : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/system-models/{id}", async (Guid Id, ISender sender) =>
            {
                var result = await sender.Send(new DeleteSystemModelCommand(Id));
                var response = new DeleteSystemModelResponse(result.IsSuccess);
                return Results.Ok(response);
            })
            .WithName("DeleteSystemModel")
            .Produces<DeleteSystemModelResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete System Model")
            .WithDescription("Deletes a SystemModel")
            .RequireAuthorization("AdminPolicy");
        }
    }
}
