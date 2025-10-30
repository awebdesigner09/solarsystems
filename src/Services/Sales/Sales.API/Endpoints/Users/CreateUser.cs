using Microsoft.AspNetCore.Identity;
using Sales.Application.Sales.Commands.CreateCustomer;
using Sales.Domain.Identity;

namespace Sales.API.Endpoints.Users
{
    public record CreateUserRequest(
        string Username,
        string Email,
        string Password,
        string Role,
        string? AddressLine1,
        string? AddressLine2,
        string? City,
        string? State,
        string? PostalCode);

    public record CreateUserResponse(bool Succeeded, string? UserId, IEnumerable<string>? Errors = null);

    public class CreateUser : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/users", async (
                CreateUserRequest request,
                UserManager<ApplicationUser> userManager,
                RoleManager<IdentityRole> roleManager,
                ISender sender) =>
            {
                var user = new ApplicationUser
                {
                    UserName = request.Username,
                    Email = request.Email
                };

                var result = await userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                {
                    return Results.BadRequest(new CreateUserResponse(false, null, result.Errors.Select(e => e.Description)));
                }

                if (!await roleManager.RoleExistsAsync(request.Role))
                {
                    return Results.BadRequest(new CreateUserResponse(false, user.Id, new[] { $"Role '{request.Role}' does not exist." }));
                }

                await userManager.AddToRoleAsync(user, request.Role);

                // If the new user is a customer, create the associated customer profile.
                if (request.Role.Equals("Customer", StringComparison.OrdinalIgnoreCase))
                {
                    var customerDto = new CustomerDto(
                        Id: Guid.NewGuid(),
                        Name: request.Username,
                        Email: request.Email,
                        Address: new AddressDto(
                            request.AddressLine1!,
                            request.AddressLine2,
                            request.City!,
                            request.State!,
                            request.PostalCode!,
                            "USA"
                        )
                    );

                    var command = new CreateCustomerCommand(customerDto, user.Id);
                    await sender.Send(command);
                }

                return Results.Created($"/api/users/{user.Id}", new CreateUserResponse(true, user.Id));

            })
            .WithName("CreateUser")
            .Produces<CreateUserResponse>(StatusCodes.Status201Created)
            .Produces<CreateUserResponse>(StatusCodes.Status400BadRequest)
            .WithSummary("Create a New User (Admin)")
            .WithDescription("Creates a new user with a specified role. Only accessible by administrators.")
            .RequireAuthorization("AdminPolicy"); // Secure the endpoint
        }
    }
}