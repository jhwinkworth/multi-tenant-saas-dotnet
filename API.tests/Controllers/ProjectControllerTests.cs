using Api.Controllers;
using Application.DTOs.Project;
using Application.Interfaces;
using Application.Interfaces.Services;
using Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

public class ProjectControllerTests
{
    private readonly Mock<IProjectService> _mockService;
    private readonly Mock<ITenantProvider> _mockTenant;
    private readonly ProjectsController _controller;

    public ProjectControllerTests()
    {
        _mockService = new Mock<IProjectService>();
        _controller = new ProjectsController(_mockService.Object);
    }

    [Fact]
    public void GetAll_ShouldReturnOkWithProjects()
    {
        // Arrange
        var projects = new List<Project>
        {
            new Project { Id = Guid.NewGuid(), Name = "Project 1", TenantId = Guid.NewGuid() },
            new Project { Id = Guid.NewGuid(), Name = "Project 2", TenantId = Guid.NewGuid() }
        };
        _mockService.Setup(s => s.GetAllProjects()).Returns(projects);

        // Act
        var result = _controller.GetAll(); // ActionResult<IEnumerable<Project>>


        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.Value.Should().BeEquivalentTo(projects);
    }

    [Fact]
    public void GetById_ShouldReturnOk_WhenFound()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var project = new Project
        {
            Id = projectId,
            Name = "Test Project",
            TenantId = Guid.NewGuid()
        };

        _mockService
            .Setup(s => s.GetProjectById(projectId))
            .Returns(project);

        // Act
        var result = _controller.GetById(projectId);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();

        var ok = result.Result as OkObjectResult;
        ok.Value.Should().Be(project);
    }


    [Fact]
    public void GetById_ShouldReturnNotFound_WhenMissing()
    {
        var projectId = Guid.NewGuid();
        _mockService.Setup(s => s.GetProjectById(projectId)).Returns((Project)null);

        var result = _controller.GetById(projectId);

        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public void Create_ShouldReturnCreatedAtAction()
    {
        // Arrange
        var dto = new CreateProjectDto { Name = "New Project" };
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            TenantId = Guid.NewGuid()
        };

        _mockService
            .Setup(s => s.CreateProject(dto.Name))
            .Returns(project);

        // Act
        var result = _controller.Create(dto);

        // Assert
        result.Result.Should().BeOfType<CreatedAtActionResult>();
        var created = result.Result as CreatedAtActionResult;

        created.ActionName.Should().Be(nameof(ProjectsController.GetById));

        var returned = created.Value as ProjectDto;
        returned.Should().NotBeNull();
        returned.Id.Should().Be(project.Id);
        returned.Name.Should().Be(project.Name);
        returned.TenantId.Should().Be(project.TenantId);
    }

    [Fact]
    public void Delete_ShouldReturnNoContent()
    {
        var id = Guid.NewGuid();

        var result = _controller.Delete(id);

        result.Should().BeOfType<NoContentResult>();
    }
}
