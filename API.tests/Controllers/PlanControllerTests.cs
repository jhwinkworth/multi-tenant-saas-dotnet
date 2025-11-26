using Api.Controllers;
using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using FluentAssertions;
using Application.DTOs.Plan;

namespace API.Tests.Controller
{
    public class PlansControllerTests
    {
        private readonly Mock<IPlanService> _service;
        private readonly PlansController _controller;

        public PlansControllerTests()
        {
            _service = new Mock<IPlanService>();
            _controller = new PlansController(_service.Object);
        }

        [Fact]
        public void GetAllPlans_ShouldReturnOk()
        {
            // Arrange
            var plans = new List<Plan> { new Plan { Id = Guid.NewGuid(), Name = "Pro" } };
            _service.Setup(s => s.GetAllPlans()).Returns(plans);

            // Act
            var result = _controller.GetAll();

            // Assert
            var expectedDto = new List<PlanDto> { new PlanDto { Id = plans[0].Id, Name = "Pro" } };

            var ok = result.Result as OkObjectResult;
            ok.Should().NotBeNull();
            ok.Value.Should().BeEquivalentTo(expectedDto);
        }
    }
}
