using Sales.Application.Sales.Queries.GetCustomerQuoteRequests;

namespace Sales.API.Endpoints.QuoteRequest
{

    public record GetCustomerQuoteRequestsResponse(IEnumerable<QuoteRequestDto> QuoteRequests);
    public class GetCustomerQuoteRequests : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app) 
        {
            app.MapGet("/quote-requests/{id}", async (Guid Id, ISender sender) =>
            {
                var result = await sender.Send(new GetCustomerQuoteRequestsQuery(Id));
                var response = new GetCustomerQuoteRequestsResponse(result.QuoteRequests);
                return Results.Ok(response);
            })
            .WithName("GetCustomerQuoteRequests")
            .Produces<GetCustomerQuoteRequestsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get QuoteRequests for customer")
            .WithDescription("Retrieves list of Quote Requests for the customer")
            .RequireAuthorization("CustomerPolicy");
        }
    }
}
