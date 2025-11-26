using Api.Controllers;
using Application.DTOs;
using Application.DTOs.TaskItem;
using Application.Interfaces.Services;
using Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

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
        var list = new List<TaskItem> { new TaskItem { Id = Guid.NewGuid() } };
        _service.Setup(s => s.GetForProject(It.IsAny<Guid>())).Returns(list);

        var result = _controller.GetForProject(Guid.NewGuid());

        var ok = result as OkObjectResult;
        ok.Should().NotBeNull();
        ok.Value.Should().BeEquivalentTo(list);
    }

    [Fact]
    public void Create_ShouldReturnCreatedAtAction()
    {
        var dto = new CreateTaskItemDto { Name = "Do this", ProjectId = Guid.NewGuid() };
        var item = new TaskItem { Id = Guid.NewGuid(), Name = dto.Name };

        _service.Setup(s => s.Create(dto.Name, dto.ProjectId)).Returns(item);

        var response = _controller.Create(dto);

        var created = response.Result as CreatedAtActionResult;
        created.Should().NotBeNull();
        created.Value.Should().BeEquivalentTo(item);
    }

    [Fact]
    public void Delete_ShouldReturnNoContent()
    {
        var id = Guid.NewGuid();

        var result = _controller.Delete(id);

        result.Should().BeOfType<NoContentResult>();
    }
}
