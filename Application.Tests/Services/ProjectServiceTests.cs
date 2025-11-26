using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Services;
using Domain.Entities;
using FluentAssertions;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Application.Tests.Services
{
    public class ProjectServiceTests
    {
        private readonly Mock<IProjectRepository> _mockRepo;
        private readonly Mock<ITenantProvider> _mockTenantProvider;
        private readonly ProjectService _service;

        private readonly Guid _tenantId = Guid.NewGuid();

        public ProjectServiceTests()
        {
            _mockRepo = new Mock<IProjectRepository>();
            _mockTenantProvider = new Mock<ITenantProvider>();

            _mockTenantProvider.Setup(t => t.TenantId).Returns(_tenantId);

            _service = new ProjectService(_mockRepo.Object, _mockTenantProvider.Object);
        }

        [Fact]
        public void CreateProject_ShouldSetTenantId_AndReturnProject()
        {
            // Arrange
            var name = "New Project";
            Project? capturedProject = null;

            _mockRepo
                .Setup(r => r.Add(It.IsAny<Project>()))
                .Callback<Project>(p => capturedProject = p)
                .Returns((Project p) => p);

            // Act
            var result = _service.CreateProject(name);

            // Assert
            capturedProject.Should().NotBeNull();
            capturedProject!.Name.Should().Be(name);
            capturedProject.TenantId.Should().Be(_tenantId);

            result.Should().Be(capturedProject);
        }


        [Fact]
        public void GetAllProjects_ShouldReturnProjects()
        {
            // Arrange
            var projects = new List<Project>
            {
                new Project { Id = Guid.NewGuid(), Name = "P1", TenantId = _tenantId },
                new Project { Id = Guid.NewGuid(), Name = "P2", TenantId = _tenantId }
            };
            _mockRepo.Setup(r => r.GetAllForTenant()).Returns(projects);

            // Act
            var result = _service.GetAllProjects();

            // Assert
            result.Should().BeEquivalentTo(projects);
        }

        [Fact]
        public void GetProjectById_ShouldReturnProject_WhenExists()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var project = new Project { Id = projectId, Name = "P1", TenantId = _tenantId };
            _mockRepo.Setup(r => r.GetById(projectId)).Returns(project);

            // Act
            var result = _service.GetProjectById(projectId);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(projectId);
        }

        [Fact]
        public void UpdateProject_ShouldCallRepo()
        {
            // Arrange
            var project = new Project { Id = Guid.NewGuid(), Name = "Old Name" };

            // Act
            _service.UpdateProject(project);

            // Assert
            _mockRepo.Verify(r => r.Update(project), Times.Once);
        }

        [Fact]
        public void DeleteProject_ShouldCallRepo()
        {
            // Arrange
            var project = new Project { Id = Guid.NewGuid(), Name = "P1" };

            // Act
            _service.DeleteProject(project);

            // Assert
            _mockRepo.Verify(r => r.Delete(project), Times.Once);
        }
    }
}
