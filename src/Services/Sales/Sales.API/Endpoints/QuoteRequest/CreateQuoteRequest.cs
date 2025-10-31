using Sales.Application.Sales.Commands.CreateQuoteRequest;

namespace Sales.API.Endpoints.QuoteRequest
{
    public record CreateQuoteRequestRequest(QuoteRequestDto QuoteRequest);
    public record CreateQuoteRequestResponse(Guid Id);
    public class CreateQuoteRequest : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/quote-requests", async (CreateQuoteRequestRequest request, ISender sender) =>
            {
                var command = new CreateQuoteRequestCommand(request.QuoteRequest);
                var result = await sender.Send(command);
                var response = new CreateQuoteRequestResponse(result.Id);
                return Results.Created($"/quote-requests/{response.Id}", response);
            })
            .WithName("CreateQuoteRequest")
            .Produces<CreateQuoteRequestResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Quote Request")
            .WithDescription("Creates new Quote Request")
            .RequireAuthorization("CustomerPolicy");
        }
    }
}
