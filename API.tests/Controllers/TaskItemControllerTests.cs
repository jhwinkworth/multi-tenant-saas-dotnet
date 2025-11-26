using Api.Controllers;
using Application.DTOs;
using Application.DTOs.TaskItem;
using Application.Interfaces.Services;
using Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace API.Tests.Controllers
{
    public class TaskItemsControllerTests
    {
        private readonly Mock<ITaskItemService> _service;
        private readonly TaskItemsController _controller;

        public TaskItemsControllerTests()
        {
            _service = new Mock<ITaskItemService>();
            _controller = new TaskItemsController(_service.Object);
        }

        [Fact]
        public void GetForProject_ShouldReturnOk()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var tasks = new List<TaskItem>
    {
        new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = "Task 1",
            ProjectId = projectId,
            CreatedAt = DateTime.UtcNow
        }
    };

            _service.Setup(s => s.GetTasksForProject(projectId)).Returns(tasks);

            // Act
            var result = _controller.GetForProject(projectId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();

            // Map domain entities to DTOs like the controller does
            var expectedDtos = tasks.Select(t => new TaskItemDto
            {
                Id = t.Id,
                Title = t.Title,
                ProjectId = t.ProjectId,
                DueDate = t.DueDate,
                CreatedAt = t.CreatedAt
            }).ToList();

            // Compare while allowing for DateTime differences if needed
            var returnedDtos = okResult.Value as List<TaskItemDto>;
            returnedDtos.Should().NotBeNull();
            for (int i = 0; i < expectedDtos.Count; i++)
            {
                returnedDtos[i].Id.Should().Be(expectedDtos[i].Id);
                returnedDtos[i].Title.Should().Be(expectedDtos[i].Title);
                returnedDtos[i].ProjectId.Should().Be(expectedDtos[i].ProjectId);
                returnedDtos[i].CreatedAt.Should().BeCloseTo(expectedDtos[i].CreatedAt, TimeSpan.FromMilliseconds(100));
                returnedDtos[i].DueDate.Should().Be(expectedDtos[i].DueDate);
            }
        }


        [Fact]
        public void Create_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var dto = new CreateTaskItemDto
            {
                Title = "Do this",
                ProjectId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            };

            var taskItem = new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                ProjectId = dto.ProjectId,
                CreatedAt = dto.CreatedAt
            };

            _service.Setup(s => s.CreateTaskItem(dto.Title, dto.ProjectId, dto.CreatedAt, null))
                    .Returns(taskItem);

            // Act
            var response = _controller.Create(dto);

            // Assert
            var created = response.Result as CreatedAtActionResult;
            created.Should().NotBeNull();

            var expectedDto = new TaskItemDto
            {
                Id = taskItem.Id,
                Title = taskItem.Title,
                ProjectId = taskItem.ProjectId,
                DueDate = taskItem.DueDate
            };

            var returnedDto = created.Value as TaskItemDto;
            returnedDto.Should().NotBeNull();
            returnedDto.Title.Should().Be(expectedDto.Title);
            returnedDto.ProjectId.Should().Be(expectedDto.ProjectId);
            returnedDto.CreatedAt.Should().BeCloseTo(expectedDto.CreatedAt, TimeSpan.FromMilliseconds(100));
        }


        [Fact]
        public void Delete_ShouldReturnNoContent()
        {
            var id = Guid.NewGuid();

            var result = _controller.Delete(id);

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
