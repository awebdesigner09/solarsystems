using Sales.Application.Sales.Commands.CreateSystemModel;

namespace Sales.API.Endpoints.SystemModel
{
    // Accepts CreateSystemModelRequest object.
    // Maps to CreateSystemModelCommand.
    // Uses MediatR to send command to corresponding handler.
    // Returns response with the created SystemModel Id.
    public record CreateSystemModelRequest(SystemModelDto SystemModel);
    public record CreateSystemModelResponse(Guid Id);
    public class CreateSystemModel : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/system-models", async (CreateSystemModelRequest request, ISender sender) =>
            {
                var command = new CreateSystemModelCommand(request.SystemModel);
                var result = await sender.Send(command);
                var response = new CreateSystemModelResponse(result.Id);
                return Results.Created($"/system-models/{response.Id}", response);
            })
            .WithName("CreateSystemModel")
            .Produces<CreateSystemModelResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create SystemModel")
            .WithDescription("Creates new Solar System model").
            RequireAuthorization("AdminPolicy");
        }
    }
}
