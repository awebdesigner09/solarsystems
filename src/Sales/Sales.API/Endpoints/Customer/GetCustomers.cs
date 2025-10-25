using BuildingBlocks.Pagination;
using Sales.Application.Sales.Queries.GetCustomers;

namespace Sales.API.Endpoints.Customer
{
    public record GetCustomersResponse(PaginatedResult<CustomerDto> Customers);
    public class GetCustomers : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/customers", async([AsParameters] PaginationRequest request, ISender sender) =>
            {   
                var result = await sender.Send(new GetCustomersQuery(request));
                var response = new GetCustomersResponse(result.Customers);
                return Results.Ok(response);
            })
            .WithName("GetCustomers")
            .Produces<GetCustomersResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Customers")
            .WithDescription("Get Customers with pagination.");
        }
    }
}
