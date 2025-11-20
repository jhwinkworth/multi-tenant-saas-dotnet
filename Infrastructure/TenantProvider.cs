using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

public interface ITenantProvider
{
    Guid CurrentTenantId { get; }
}

public class TenantProvider : ITenantProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TenantProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid CurrentTenantId
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?.User
                .Claims.FirstOrDefault(c => c.Type == "TenantId");

            if (claim == null)
                throw new Exception("TenantId claim not found. Ensure JWT is valid and TenantId is included.");

            return Guid.Parse(claim.Value);
        }
    }
}
