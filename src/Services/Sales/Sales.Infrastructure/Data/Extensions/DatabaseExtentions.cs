
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Sales.Infrastructure.Data.Extensions
{
    public static class DatabaseExtentions
    {
        public static async Task InitialiseDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await context.Database.MigrateAsync();
            await SeedAsync(context);
        }

        private static async Task SeedAsync(ApplicationDbContext context)
        {
            await SeedSystemModelAsync(context);
            await SeedCustomerAsync(context);
            await SeedQuoteRequestAsync(context);
            await SeedQuotesAsync(context);
            await SeedOrderAsync(context);
            
        }

        private static async Task SeedSystemModelAsync(ApplicationDbContext context)
        {
            if(!await context.SystemModels.AnyAsync())
            {
                await context.SystemModels.AddRangeAsync(InitialData.SystemModels);
                await context.SaveChangesAsync();
            }
        }
        
        private static async Task SeedCustomerAsync(ApplicationDbContext context)
        {
            if (!await context.Customers.AnyAsync())
            {
                await context.Customers.AddRangeAsync(InitialData.Customers);
                await context.SaveChangesAsync();
            }
        }
        private static async Task SeedQuoteRequestAsync(ApplicationDbContext context)
        {
            if (!await context.QuoteRequests.AnyAsync())
            {
                await context.QuoteRequests.AddRangeAsync(InitialData.QuoteRequests);
                await context.SaveChangesAsync();
            }
        }
        private static async Task SeedQuotesAsync(ApplicationDbContext context)
        {
            if(!await context.Quotes.AnyAsync())
            {
                await context.Quotes.AddRangeAsync(InitialData.Quotes);
                await context.SaveChangesAsync();
            }
        }
        private static async Task SeedOrderAsync(ApplicationDbContext context)
        {
            if (!await context.Orders.AnyAsync())
            {
                await context.Orders.AddRangeAsync(InitialData.Orders);
                await context.SaveChangesAsync();
            }
        }
        
    }
}
