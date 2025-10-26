using Microsoft.EntityFrameworkCore;

namespace Sales.Application.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Customer> Customers { get; }
        DbSet<SystemModel> SystemModels { get; }
        DbSet<QuoteRequest> QuoteRequests { get; }
        DbSet<Order> Orders { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
