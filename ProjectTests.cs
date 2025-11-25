using System;

public class ProjectTests
{
    [Fact]
    public void Project_ShouldHaveTenantId()
    {
        var tenantId = Guid.NewGuid();
        var project = new Project
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            Name = "Test Project"
        };

        Assert.Equal(tenantId, project.TenantId);
    }

    [Fact]
    public void TaskItem_ShouldReferenceProject()
    {
        var project = new Project
        {
            Id = Guid.NewGuid(),
            TenantId = Guid.NewGuid(),
            Name = "Demo"
        };

        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            Project = project
        };

        Assert.Equal(project.Id, task.Project.Id);
    }

    [Fact]
    public void Subscription_ShouldBelongTo_Tenant()
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


}
