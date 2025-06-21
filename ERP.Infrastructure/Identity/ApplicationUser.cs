using Microsoft.AspNetCore.Identity;

namespace ERP.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public Guid TenantId { get; set; }
    }
}
