using Sales.Application.Sales.Queries.GetQuote;

namespace Sales.API.Endpoints.Quote
{
    public class GetQuote : ICarterModule
    {
        public record GetQuoteResponse(QuoteDto Quotes);
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/quote/{id}", async (Guid Id, ISender sender) =>
            {
                var result = await sender.Send(new GetQuoteQuery(Id));
                var response = new GetQuoteResponse(result.Quote);
                return Results.Ok(response);
            })
            .WithName("GetQuote")
            .Produces<GetQuoteResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get QuoteDetails by QuoteId")
            .WithDescription("Returns Quote details for the given Id")
            .RequireAuthorization("CustomerPolicy");
        }
    }
}
