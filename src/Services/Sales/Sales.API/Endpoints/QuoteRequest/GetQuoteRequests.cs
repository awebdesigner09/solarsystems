using BuildingBlocks.Pagination;
using Sales.Application.Sales.Queries.GetQuoteRequests;

namespace Sales.API.Endpoints.QuoteRequest
{
    public record GetQuoteRequestsResponse(PaginatedResult<QuoteRequestDto> QuoteRequests);
    public class GetQuoteRequests : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/quote-requests", async ([AsParameters] PaginationRequest request, ISender sender) =>
            {
                var result = await sender.Send(new GetQuoteRequestsQuery(request));
                var response = new GetQuoteRequestsResponse(result.QuoteRequests);
                return Results.Ok(response);
            })
            .WithName("GetQuoteRequests")
            .Produces<GetQuoteRequestsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Quote Requests")
            .WithDescription("Retrieves a paginated list of Quote Requests")
            .RequireAuthorization("AdminPolicy");
        }
    }
}
