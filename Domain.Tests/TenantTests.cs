using Domain.Entities;
using Xunit;

namespace Domain.Tests;

public class TenantTests
{
    [Fact]
    public void Tenant_ShouldStoreName()
    {
        var t = new Tenant
        {
            Id = Guid.NewGuid(),
            Name = "Acme Ltd"
        };

        Assert.Equal("Acme Ltd", t.Name);
    }

    [Fact]
    public void Tenant_ShouldHaveId()
    {
        var id = Guid.NewGuid();

        var t = new Tenant
        {
            Id = id
        };

        Assert.Equal(id, t.Id);
    }
}
