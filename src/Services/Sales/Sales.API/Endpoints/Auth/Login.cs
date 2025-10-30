using Microsoft.AspNetCore.Identity;
using Sales.Domain.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Sales.API.Endpoints.Auth
{
    public record LoginRequest(string Username, string Password);
    public record LoginResponse(bool Succeeded, string? Token = null, IEnumerable<string>? Errors = null);

    public class Login : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/auth/login", async (
                LoginRequest request,
                UserManager<ApplicationUser> userManager,
                IConfiguration configuration) =>
            {
                var user = await userManager.FindByNameAsync(request.Username);
                if (user == null)
                {
                    return Results.Unauthorized();
                }

                var result = await userManager.CheckPasswordAsync(user, request.Password);

                if (!result)
                {
                    return Results.Unauthorized();
                }

                // User is authenticated, generate JWT token
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id) // Add UserId as NameIdentifier
                };

                // Add roles to claims
                var userRoles = await userManager.GetRolesAsync(user);
                foreach (var role in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!));

                var token = new JwtSecurityToken(
                    issuer: configuration["JWT:ValidIssuer"],
                    audience: configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3), // Token expiration time
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return Results.Ok(new LoginResponse(true, new JwtSecurityTokenHandler().WriteToken(token)));
            })
            .WithName("LoginUser")
            .Produces<LoginResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithSummary("Login User")
            .WithDescription("Authenticates a user and returns a JWT token.");
        }
    }
}