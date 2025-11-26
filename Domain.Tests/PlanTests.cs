using Domain.Entities;
using Xunit;

namespace Domain.Tests;

public class PlanTests
{
    [Fact]
    public void Plan_ShouldStoreName()
    {
        var p = new Plan
        {
            Id = Guid.NewGuid(),
            Name = "Pro"
        };

        Assert.Equal("Pro", p.Name);
    }

    [Fact]
    public void Plan_ShouldStorePricePerMonth()
    {
        var p = new Plan
        {
            Id = Guid.NewGuid(),
            PricePerMonth = 29.99m
        };

        Assert.Equal(29.99m, p.PricePerMonth);
    }

    [Fact]
    public void Plan_CanStoreDescription()
    {
        var p = new Plan
        {
            Id = Guid.NewGuid(),
            Description = "Premium"
        };

        Assert.Equal("Premium", p.Description);
    }

    [Fact]
    public void Plan_ShouldStoreIsActive()
    {
        var p = new Plan
        {
            Id = Guid.NewGuid(),
            IsActive = false
        };

        Assert.False(p.IsActive);
    }
}
