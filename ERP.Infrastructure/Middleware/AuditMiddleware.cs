using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using ERP.Infrastructure.Persistence;

namespace ERP.Infrastructure.Middleware
{
    public class AuditMiddleware
    {
        private readonly RequestDelegate _next;

        public AuditMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext ctx, ApplicationDbContext db)
        {
            await _next(ctx);

            var audit = new AuditLog
            {
                TenantId   = (Guid)(ctx.Items["TenantId"] ?? Guid.Empty),
                Route      = ctx.Request.Path,
                Method     = ctx.Request.Method,
                StatusCode = ctx.Response.StatusCode,
                UserId     = ctx.User.Identity?.IsAuthenticated == true
                             ? Guid.Parse(ctx.User.FindFirst("sub")!.Value)
                             : null
            };

            db.AuditLogs.Add(audit);
            await db.SaveChangesAsync();
        }
    }
}
