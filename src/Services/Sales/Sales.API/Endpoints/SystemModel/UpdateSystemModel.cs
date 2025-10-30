using Sales.Application.Sales.Commands.UpdateSystemModel;

namespace Sales.API.Endpoints.SystemModel
{
    // Accepts as UpdateSystemModelRequest object.
    // Maps to UpdateSystemModelCommand.
    // Uses MediatR to send command to corresponding handler.
    // Returns response indicating success or failure of the update operation.

    public record UpdateSystemModelRequest(SystemModelDto SystemModel);
    public record UpdateSystemModelResponse(bool IsSuccess);
    public class UpdateSystemModel : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/system-models", async (UpdateSystemModelRequest request, ISender sender) =>
            {
                var command = new UpdateSystemModelCommand(request.SystemModel);
                var result = await sender.Send(command);
                var response = new UpdateSystemModelResponse(result.IsSuccess);
                return Results.Ok(response);
            })
            .WithName("UpdateSystemModel")
            .Produces<UpdateSystemModelResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update System Model")
            .WithDescription("Update an existing Solar System Model.")
            .RequireAuthorization("AdminPolicy");
        }
    }
}