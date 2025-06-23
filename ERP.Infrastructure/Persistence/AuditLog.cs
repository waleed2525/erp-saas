using System;
using ERP.Domain.Common;

namespace ERP.Infrastructure.Persistence
{
    public class AuditLog : BaseAuditableEntity
    {
        public string Route { get; set; } = default!;
        public string Method { get; set; } = default!;
        public Guid? UserId { get; set; }
        public int StatusCode { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    }
}
