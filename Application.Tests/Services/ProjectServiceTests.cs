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
    public class ProjectServiceTests
    {
        private readonly Mock<IProjectRepository> _projectRepoMock;
        private readonly ProjectService _projectService;

        public ProjectServiceTests()
        {
            _projectRepoMock = new Mock<IProjectRepository>();
            _projectService = new ProjectService(_projectRepoMock.Object);
        }

        [Fact]
        public void CreateProject_ShouldReturnProject_WithTenantId()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            _projectRepoMock.Setup(r => r.Add(It.IsAny<Project>()))
                .Returns((Project p) => p);

            // Act
            var project = _projectService.CreateProject("Test Project", tenantId);

            // Assert
            project.Name.Should().Be("Test Project");
            project.TenantId.Should().Be(tenantId);
            _projectRepoMock.Verify(r => r.Add(It.IsAny<Project>()), Times.Once);
        }

        [Fact]
        public void GetAllProjects_ShouldReturnProjects()
        {
            var projects = new List<Project>
            {
                new Project { Id = Guid.NewGuid(), Name = "P1", TenantId = Guid.NewGuid() },
                new Project { Id = Guid.NewGuid(), Name = "P2", TenantId = Guid.NewGuid() }
            };
            _projectRepoMock.Setup(r => r.GetAllForTenant()).Returns(projects);

            var result = _projectService.GetAllProjects();

            result.Count.Should().Be(2);
        }

        [Fact]
        public void GetProjectById_ShouldReturnProject_WhenExists()
        {
            var projectId = Guid.NewGuid();
            var project = new Project { Id = projectId, Name = "P1", TenantId = Guid.NewGuid() };
            _projectRepoMock.Setup(r => r.GetById(projectId)).Returns(project);

            var result = _projectService.GetProjectById(projectId);

            result.Should().NotBeNull();
            result!.Id.Should().Be(projectId);
        }

        [Fact]
        public void UpdateProject_ShouldCallRepo()
        {
            var project = new Project { Id = Guid.NewGuid(), Name = "Old Name" };
            _projectService.UpdateProject(project);
            _projectRepoMock.Verify(r => r.Update(project), Times.Once);
        }

        [Fact]
        public void DeleteProject_ShouldCallRepo()
        {
            var project = new Project { Id = Guid.NewGuid(), Name = "P1" };
            _projectService.DeleteProject(project);
            _projectRepoMock.Verify(r => r.Delete(project), Times.Once);
        }
    }
}
