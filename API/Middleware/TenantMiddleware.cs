using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Application.Interfaces;

namespace Api.Middleware
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITenantProvider tenantProvider)
        {
            // User must be authenticated AND have a TenantId claim
            var tenantClaim = context.User?.Claims
                .FirstOrDefault(c => c.Type == "TenantId");

            if (tenantClaim != null && Guid.TryParse(tenantClaim.Value, out var tenantId))
            {
                tenantProvider.SetTenant(tenantId);
            }

            await _next(context);
        }
    }
}
