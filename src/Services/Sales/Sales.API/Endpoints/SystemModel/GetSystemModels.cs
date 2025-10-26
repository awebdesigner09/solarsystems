using BuildingBlocks.Pagination;
using Sales.Application.Sales.Queries.GetSystemModels;

namespace Sales.API.Endpoints.SystemModel
{

    //- Accepts pagination parameters.
    //- Constructs a <Query> object with these parameters.
    //- Retrieves the data and returns it in a paginated format.

    //public record GetSystemModelsRequest(PaginationRequest PaginationRequest);
    public record GetSystemModelsResponse(PaginatedResult<SystemModelDto> SystemModels);
    public class GetSystemModels : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/system-models", async([AsParameters] PaginationRequest request, ISender sender) =>
            {   
                var result = await sender.Send(new GetSystemModelsQuery(request));

                var response = new GetSystemModelsResponse(result.SystemModels);

                return Results.Ok(response);
            })
            .WithName("GetSystemModels")
            .Produces<GetSystemModelsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get System Models")
            .WithDescription("Get Solar System Models with pagination.");
        }
    }
}
