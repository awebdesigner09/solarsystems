using Microsoft.EntityFrameworkCore;
using Sales.Application.Data;
using Sales.Domain.Models;
using System.Reflection;

namespace Sales.Infrastructure.Data
{
    internal class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
        public DbSet<Customer> Customers => Set<Customer>();

        public DbSet<SystemModel> SystemModels => Set<SystemModel>();

        public DbSet<QuoteRequest> QuoteRequests => Set<QuoteRequest>();

        public DbSet<Order> Orders => Set<Order>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Ignore<CustomerId>();
            builder.Ignore<SystemModelId>();
            builder.Ignore<QuoteRequestId>();
            builder.Ignore<OrderId>();
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
    }
}
