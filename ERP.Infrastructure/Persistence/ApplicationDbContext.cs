using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ERP.Domain.Entities;
using ERP.Application.Common.Interfaces;
using ERP.Infrastructure.Identity;

namespace ERP.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Item> Items => Set<Item>();
        public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    }
}
