using System;
using ERP.Domain.Common;

namespace ERP.Infrastructure.Security
{
    public class RefreshToken : BaseAuditableEntity
    {
        public Guid UserId { get; set; }
        public string Token { get; set; } = default!;
        public DateTime Expires { get; set; }
        public bool Revoked { get; set; }
    }
}
