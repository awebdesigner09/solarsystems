﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Sales.Application.Data;
using Sales.Domain.Identity;
using Sales.Domain.Models;
using System.Reflection;
namespace Sales.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
        public DbSet<Customer> Customers => Set<Customer>();

        public DbSet<SystemModel> SystemModels => Set<SystemModel>();

        public DbSet<QuoteRequest> QuoteRequests => Set<QuoteRequest>();

        public DbSet<Order> Orders => Set<Order>();

        public DbSet<Quote> Quotes => Set<Quote>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Ignore<CustomerId>();
            builder.Ignore<SystemModelId>();
            builder.Ignore<QuoteRequestId>();
            builder.Ignore<OrderId>();
            builder.Ignore<QuoteId>();
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            SeedUsersAndRoles(builder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.ConfigureWarnings(warnings =>
            {
                // Suppress the PendingModelChangesWarning
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning);
                // Suppress the enum default value warnings for Order.Status and QuoteRequest.Status
                warnings.Ignore(CoreEventId.PossibleIncorrectRequiredNavigationWithQueryFilterInteractionWarning); // Often good to include
                //warnings.Ignore(ModelValidationEventId.DefaultValueWarning);
            });
        }

        private void SeedUsersAndRoles(ModelBuilder builder)
        {
            // Seed Roles
            var adminRoleId = "fab4fac1-c546-41de-aebc-a14da6895711";
            var customerRoleId = "c7b013f0-5201-4317-abd8-c211f91b7330";

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = customerRoleId, Name = "Customer", NormalizedName = "CUSTOMER" }
            );

            // Seed Admin User
            var adminUserId = "a18be9c0-aa65-4af8-bd17-00bd9344e575";
            var adminUser = new ApplicationUser
            {
                Id = adminUserId,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@solarsystems.com",
                NormalizedEmail = "ADMIN@SOLARSYSTEMS.COM",
                EmailConfirmed = true
            };

            var passwordHasher = new PasswordHasher<ApplicationUser>();
            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Admin123!");
            builder.Entity<ApplicationUser>().HasData(adminUser);

            // Seed Normal User (for John Doe)
            var customerUserId = "a18be9c0-aa65-4af8-bd17-00bd9344e576";
            var customerUser = new ApplicationUser
            {
                Id = customerUserId,
                UserName = "johndoe",
                NormalizedUserName = "JOHNDOE",
                Email = "johndoe@email.com",
                NormalizedEmail = "JOHNDOE@EMAIL.COM",
                EmailConfirmed = true
            };
            customerUser.PasswordHash = passwordHasher.HashPassword(customerUser, "Customer123!");
            builder.Entity<ApplicationUser>().HasData(customerUser);

            // Seed Normal User (for Jane Smith)
            var janeSmithUserId = "a18be9c0-aa65-4af8-bd17-00bd9344e577";
            var janeSmithUser = new ApplicationUser
            {
                Id = janeSmithUserId,
                UserName = "janesmith",
                NormalizedUserName = "JANESMITH",
                Email = "janesmith@email.com",
                NormalizedEmail = "JANESMITH@EMAIL.COM",
                EmailConfirmed = true
            };
            janeSmithUser.PasswordHash = passwordHasher.HashPassword(janeSmithUser, "Customer123!");
            builder.Entity<ApplicationUser>().HasData(janeSmithUser);

            // Assign roles to users
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { RoleId = adminRoleId, UserId = adminUserId },
                new IdentityUserRole<string> { RoleId = customerRoleId, UserId = customerUserId },
                new IdentityUserRole<string>
                {
                    RoleId = customerRoleId,
                    UserId = janeSmithUserId
                }
            );
        }
    }
}
