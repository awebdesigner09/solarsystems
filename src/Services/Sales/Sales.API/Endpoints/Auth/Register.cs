using Microsoft.AspNetCore.Identity;
using Sales.Domain.Identity;
using Sales.Domain.Models;
using Sales.Application.Sales.Commands.CreateCustomer;
using Sales.Domain.ValueObjects;
using Sales.Infrastructure.Data;

namespace Sales.API.Endpoints.Auth
{
    public record RegisterRequest(
        string Username, 
        string Email, 
        string Password,
        string AddressLine1,
        string? AddressLine2,
        string City,
        string State,
        string PostalCode,
        string? Country);
    public record RegisterResponse(bool Succeeded, IEnumerable<string>? Errors = null);

    public class Register : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/auth/register", async (
                RegisterRequest request,
                UserManager<ApplicationUser> userManager,
                ISender sender) =>
            {
                var user = new ApplicationUser
                {
                    UserName = request.Username,
                    Email = request.Email
                };

                var result = await userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    // Assign the "Customer" role
                    await userManager.AddToRoleAsync(user, "Customer");

                    // Use the CreateCustomerCommand to create the customer profile
                    var customerDto = new CustomerDto(
                        Id: Guid.NewGuid(),
                        Name: request.Username,
                        Email: request.Email,
                        Address: new AddressDto(
                            request.AddressLine1,
                            request.AddressLine2,
                            request.City,
                            request.State,
                            request.PostalCode,
                            request.Country
                        )
                    );
                    
                    var command = new CreateCustomerCommand(customerDto, user.Id);
                    
                    await sender.Send(command);

                    return Results.Created($"/auth/register/{user.Id}", new RegisterResponse(true));
                }
                else
                {
                    return Results.BadRequest(new RegisterResponse(false, result.Errors.Select(e => e.Description)));
                }
            })
            .WithName("RegisterUser")
            .Produces<RegisterResponse>(StatusCodes.Status201Created)
            .Produces<RegisterResponse>(StatusCodes.Status400BadRequest)
            .WithSummary("Register New User")
            .WithDescription("Registers a new user with a username, email, and password.");
        }
    }
}