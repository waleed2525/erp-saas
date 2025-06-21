using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace ERP.Infrastructure.MultiTenancy
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            // 1) Try read X-Tenant header
            var tenantHeader = context.Request.Headers["X-Tenant"];
            if (!string.IsNullOrWhiteSpace(tenantHeader))
            {
                context.Items["TenantId"] = Guid.Parse(tenantHeader!);
            }
            else
            {
                // 2) Fallback to sub-domain
                var host = context.Request.Host.Host; // e.g., tenant1.erp.com
                var parts = host.Split('.');
                if (parts.Length > 2)
                {
                    context.Items["TenantId"] = Guid.Parse(parts[0]); // assume GUID subdomain
                }
            }

            await _next(context);
        }
    }
}
