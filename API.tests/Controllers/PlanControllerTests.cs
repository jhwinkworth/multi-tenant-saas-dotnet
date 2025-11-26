using Api.Controllers;
using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;

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
        var plans = new List<Plan> { new Plan { Id = Guid.NewGuid(), Name = "Pro" } };
        _service.Setup(s => s.GetAllPlans()).Returns(plans);

        var result = _controller.GetAll();

        var ok = result as OkObjectResult;
        ok.Should().NotBeNull();
        ok.Value.Should().BeEquivalentTo(plans);
    }
}
