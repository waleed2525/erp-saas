using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ERP.Application.Common.Interfaces;
using ERP.Domain.Common;
using ERP.Domain.Entities;
using ERP.Infrastructure.Identity;
using ERP.Infrastructure.MultiTenancy;
using ERP.Infrastructure.Security;

namespace ERP.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        private readonly IHttpContextAccessor _http;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            IHttpContextAccessor http) : base(options) =>
            _http = http;

        /*---------------  DbSets  ---------------*/
        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Item> Items => Set<Item>();
        public DbSet<Warehouse> Warehouses => Set<Warehouse>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

        /*---------------  Model Config ---------------*/
        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            /* 1️⃣  المفتاح الأساسي على الكيان الأساس */
            b.Entity<BaseAuditableEntity>(cfg =>
            {
                cfg.HasKey(e => e.Id);
            });

            /* 2️⃣  مرشح Tenant عام */
            var tenantId = _http.HttpContext?.Items["TenantId"] as Guid?;
            b.Entity<BaseAuditableEntity>()
             .HasQueryFilter(e => tenantId == null || e.TenantId == tenantId);

            /* 3️⃣  تكوين Account (بدون HasKey) */
            b.Entity<Account>(cfg =>
            {
                cfg.Property(e => e.AccountNumber)
                   .IsRequired()
                   .HasMaxLength(50);

                cfg.HasOne(e => e.ParentAccount)
                   .WithMany(p => p.ChildAccounts)
                   .HasForeignKey(e => e.ParentAccountId);
            });

            /* 4️⃣  تكوين Item */
            b.Entity<Item>(cfg =>
            {
                cfg.Property(i => i.SKU).IsRequired().HasMaxLength(40);
            });

            /* 5️⃣  تكوين Warehouse */
            b.Entity<Warehouse>(cfg =>
            {
                cfg.Property(w => w.Code).IsRequired().HasMaxLength(20);
            });

            /* 6️⃣  تكوين RefreshToken */
            b.Entity<RefreshToken>(cfg =>
            {
                cfg.HasIndex(r => r.Token).IsUnique();
            });
        }
    }
}
