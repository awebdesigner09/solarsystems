using Sales.Application.Sales.Queries.GetCustomerQuotes;

namespace Sales.API.Endpoints.Quote
{
    public record GetCustomerQuotesResponse(IEnumerable<QuoteDto> Quotes);
    public class GetCustomerQuotes: ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/customer-quotes/{id}", async (Guid Id, ISender sender) =>
            {
                var result = await sender.Send(new GetCustomerQuotesQuery(Id));
                var response = new GetCustomerQuotesResponse(result.Quotes);
                return Results.Ok(response);
            })
            .WithName("GetCustomerQuotes")
            .Produces<GetCustomerQuotesResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Quotes for customerId")
            .WithDescription("Retrieves list of Quotes for the customer")
            .RequireAuthorization("CustomerPolicy");
        }
    }
}
