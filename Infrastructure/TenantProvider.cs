using Application.Interfaces;

namespace Infrastructure.Tenancy
{
    public class TenantProvider : ITenantProvider
    {
        private Guid _tenantId = Guid.Empty;

        public Guid TenantId => _tenantId;

        public void SetTenant(Guid tenantId)
        {
            _tenantId = tenantId;
        }
    }
}
