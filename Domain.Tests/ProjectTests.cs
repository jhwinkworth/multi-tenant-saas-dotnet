
using Domain.Entities;
using Xunit;

namespace Domain.Tests;

public class ProjectTests
{
    [Fact]
    public void Project_ShouldStoreTenantId()
    {
        var tenantId = Guid.NewGuid();

        var project = new Project
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            Name = "Demo Project"
        };

        Assert.Equal(tenantId, project.TenantId);
    }

    [Fact]
    public void Project_ShouldStoreName()
    {
        var project = new Project
        {
            Id = Guid.NewGuid(),
            TenantId = Guid.NewGuid(),
            Name = "Test Name"
        };

        Assert.Equal("Test Name", project.Name);
    }
}