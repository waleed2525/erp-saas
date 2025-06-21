using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ERP.Domain.Entities;

namespace ERP.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Account> Accounts { get; }
        DbSet<Item> Items { get; }
        DbSet<Warehouse> Warehouses { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
