using System;
namespace ERP.Domain.Common
{
    public abstract class BaseAuditableEntity
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }         // 🏷️ Multi‑tenant
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedOn { get; set; }
    }
}
