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
    public void Plan_ShouldStorePrice()
    {
        var p = new Plan
        {
            Id = Guid.NewGuid(),
            PricePerMonth = 29.99m
        };

        Assert.Equal(29.99m, p.PricePerMonth);
    }
}
