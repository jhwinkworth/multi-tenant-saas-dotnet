using Domain.Entities;
using Xunit;

namespace Domain.Tests;

public class SubscriptionTests
{
    [Fact]
    public void Subscription_ShouldBelongToTenant()
    {
        var tenantId = Guid.NewGuid();

        var subscription = new Subscription
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            PlanId = Guid.NewGuid()
        };

        Assert.Equal(tenantId, subscription.TenantId);
    }

    [Fact]
    public void Subscription_ShouldStorePlanId()
    {
        var planId = Guid.NewGuid();

        var s = new Subscription
        {
            Id = Guid.NewGuid(),
            TenantId = Guid.NewGuid(),
            PlanId = planId
        };

        Assert.Equal(planId, s.PlanId);
    }
}
