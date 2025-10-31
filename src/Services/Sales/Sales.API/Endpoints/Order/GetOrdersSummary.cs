using BuildingBlocks.Pagination;
using Sales.API.Endpoints.QuoteRequest;
using Sales.Application.Sales.Queries.GetAllOrders;
using Sales.Application.Sales.Queries.GetQuoteRequests;
using System.Threading.Tasks;

namespace Sales.API.Endpoints.Order
{
    public record GetOrdersSummaryResponse(PaginatedResult<OrderSummaryDto> OrdersSummary);
    public class GetOrdersSummary : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/orders-summary", async ([AsParameters] PaginationRequest request, ISender sender) =>
            {
                var result = await sender.Send(new GetOrdersSummaryQuery(request));
                var response = new GetOrdersSummaryResponse(result.OrdersSummary);
                return Results.Ok(response);
            })
            .WithName("GetOrdersSummary")
            .Produces<GetOrdersSummaryResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Summary of all Orders")
            .WithDescription("Retrieves a paginated list of Orders")
            .RequireAuthorization("AdminPolicy");
        }
    }
}
