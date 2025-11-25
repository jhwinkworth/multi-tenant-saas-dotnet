using Microsoft.AspNetCore.Builder;

namespace Api.Middleware
{
    public static class TenantMiddlewareExtensions
    {
        public static IApplicationBuilder UseTenantMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<TenantMiddleware>();
        }
    }
}
