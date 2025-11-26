using Domain.Entities;
using Xunit;

namespace Domain.Tests;

public class TaskItemTests
{
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
            ProjectId = project.Id,
            Project = project
        };

        Assert.Equal(project.Id, task.ProjectId);
        Assert.Equal(project, task.Project);
    }

    [Fact]
    public void TaskItem_CanStoreTitle()
    {
        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = "Test Task"
        };

        Assert.Equal("Test Task", task.Title);
    }

    [Fact]
    public void TaskItem_CanStoreDescription()
    {
        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            Description = "This is a test task."
        };

        Assert.Equal("This is a test task.", task.Description);
    }

    [Fact]
    public void TaskItem_ShouldStoreIsCompleted()
    {
        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            IsCompleted = true
        };

        Assert.True(task.IsCompleted);
    }
}
