using Api.Controllers;
using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Application.DTOs.User;

public class UsersControllerTests
{
    private readonly Mock<IUserService> _service;
    private readonly UsersController _controller;

    public UsersControllerTests()
    {
        _service = new Mock<IUserService>();
        _controller = new UsersController(_service.Object);
    }

    [Fact]
    public void GetAll_ShouldReturnOk()
    {
        var list = new List<User> { new User { Id = Guid.NewGuid(), Email = "a@test.com", PasswordHash = "atest" } };
        _service.Setup(s => s.GetAllUsers()).Returns(list);

        var result = _controller.GetAll();

        var ok = result as OkObjectResult;
        ok.Should().NotBeNull();
        ok.Value.Should().BeEquivalentTo(list);
    }

    [Fact]
    public void Create_ShouldReturnCreated()
    {
        var dto = new CreateUserDto { Email = "a@test.com", PasswordHash = "123" };
        var user = new User { Id = Guid.NewGuid(), Email = dto.Email };

        _service.Setup(s => s.CreateUser(dto.Email, dto.PasswordHash, false)).Returns(user);

        var response = _controller.Create(dto);

        var created = response.Result as CreatedAtActionResult;
        created.Should().NotBeNull();
        created.Value.Should().BeEquivalentTo(user);
    }
}
