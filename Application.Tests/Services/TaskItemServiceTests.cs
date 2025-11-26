using Xunit;
using Moq;
using Application.Services;
using Application.Interfaces.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;

namespace Application.Tests.Services
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskItemRepository> _taskItemRepoMock;
        private readonly Mock<IProjectRepository> _projectRepoMock;
        private readonly TaskItemService _taskItemService;

        public TaskServiceTests()
        {
            _taskItemRepoMock = new Mock<ITaskItemRepository>();
            _projectRepoMock = new Mock<IProjectRepository>();
            _taskItemService = new TaskItemService(_taskItemRepoMock.Object, _projectRepoMock.Object);
        }

        [Fact]
        public void CreateTask_ShouldReturnTask_WithProject()
        {
            var projectId = Guid.NewGuid();
            var project = new Project { Id = projectId, TenantId = Guid.NewGuid() };

            _projectRepoMock.Setup(p => p.GetById(projectId)).Returns(project);
            _taskItemRepoMock.Setup(r => r.Add(It.IsAny<TaskItem>())).Returns((TaskItem t) => t);

            var task = _taskItemService.CreateTaskItem("New Task", projectId, DateTime.UtcNow.AddDays(1));

            task.Title.Should().Be("New Task");
            task.ProjectId.Should().Be(projectId);
            task.Project.Should().Be(project);
            _taskItemRepoMock.Verify(r => r.Add(It.IsAny<TaskItem>()), Times.Once);
        }

        [Fact]
        public void GetAllTasks_ShouldReturnAllTasks()
        {
            var tasks = new List<TaskItem>
            {
                new TaskItem { Id = Guid.NewGuid(), Title = "T1", ProjectId = Guid.NewGuid() },
                new TaskItem { Id = Guid.NewGuid(), Title = "T2", ProjectId = Guid.NewGuid() }
            };
            _taskItemRepoMock.Setup(r => r.GetAll()).Returns(tasks);

            var result = _taskItemService.GetAllTasks();

            result.Count.Should().Be(2);
        }

        [Fact]
        public void GetTasksForProject_ShouldReturnTasksForProject()
        {
            var projectId = Guid.NewGuid();
            var tasks = new List<TaskItem>
            {
                new TaskItem { Id = Guid.NewGuid(), Title = "T1", ProjectId = projectId },
                new TaskItem { Id = Guid.NewGuid(), Title = "T2", ProjectId = projectId }
            };
            _taskItemRepoMock.Setup(r => r.GetTasksByProjectId(projectId)).Returns(tasks);

            var result = _taskItemService.GetTasksForProject(projectId);

            result.All(t => t.ProjectId == projectId).Should().BeTrue();
        }

        [Fact]
        public void UpdateTask_ShouldCallRepo()
        {
            var task = new TaskItem { Id = Guid.NewGuid(), Title = "Old Task" };
            _taskItemService.UpdateTask(task);
            _taskItemRepoMock.Verify(r => r.Update(task), Times.Once);
        }

        [Fact]
        public void DeleteTask_ShouldCallRepo()
        {
            var task = new TaskItem { Id = Guid.NewGuid(), Title = "Delete Task" };
            _taskItemService.DeleteTask(task);
            _taskItemRepoMock.Verify(r => r.Delete(task), Times.Once);
        }
    }
}
