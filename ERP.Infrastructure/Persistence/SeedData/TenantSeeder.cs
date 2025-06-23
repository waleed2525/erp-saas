using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ERP.Domain.Entities;
using ERP.Infrastructure.Identity;

namespace ERP.Infrastructure.Persistence.SeedData
{
    public static class TenantSeeder
    {
        private static readonly Guid Tenant0 = Guid.Parse("11111111-1111-1111-1111-111111111111");

        public static async Task SeedAsync(
            ApplicationDbContext db,
            UserManager<ApplicationUser> users,
            RoleManager<IdentityRole> roles)
        {
            await db.Database.MigrateAsync();

            if (!await roles.RoleExistsAsync("Admin"))
                await roles.CreateAsync(new IdentityRole("Admin"));

            // Create admin if not exists
            var admin = await users.FindByEmailAsync("admin@tenant0.local");
            if (admin is null)
            {
                admin = new ApplicationUser
                {
                    TenantId = Tenant0,
                    UserName = "admin@tenant0.local",
                    Email = "admin@tenant0.local",
                    EmailConfirmed = true
                };
                await users.CreateAsync(admin, "Admin@123");
                await users.AddToRoleAsync(admin, "Admin");
            }

            // Root chart of accounts
            if (!await db.Accounts.AnyAsync(a => a.TenantId == Tenant0))
            {
                db.Accounts.Add(new Account
                {
                    Id = Guid.NewGuid(),
                    TenantId = Tenant0,
                    AccountNumber = "1000",
                    Name = "Assets"
                });
                await db.SaveChangesAsync();
            }
        }
    }
}
