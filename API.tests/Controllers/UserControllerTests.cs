using Api.Controllers;
using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Application.DTOs.User;
using FluentAssertions;

namespace API.Tests.Controllers
{
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
        public void GetAllUsers_ShouldReturnOk()
        {
            // Arrange
            var users = new List<User>
    {
        new User { Id = Guid.NewGuid(), Email = "a@test1.com", PasswordHash = "123" },
        new User { Id = Guid.NewGuid(), Email = "a@test2.com", PasswordHash = "123" }
    };
            _service.Setup(s => s.GetAllUsers()).Returns(users);

            // Act
            var result = _controller.GetAllUsers();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();

            var expectedDtos = users.Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                FullName = u.FullName,
                IsAdmin = u.IsAdmin
            }).ToList();

            var returnedDtos = okResult.Value as List<UserDto>;
            returnedDtos.Should().NotBeNull();
            returnedDtos.Should().BeEquivalentTo(expectedDtos);
        }


        [Fact]
        public void Create_ShouldReturnCreated()
        {
            // Arrange
            var dto = new CreateUserDto { Email = "a@test.com", Password = "123" };
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = dto.Email,
                IsAdmin = false
                // TenantId is set inside service, not exposed in DTO
            };

            _service.Setup(s => s.CreateUser(dto.Email, dto.Password, It.IsAny<string>(), false))
                    .Returns(user);

            // Act
            var response = _controller.CreateUser(dto);

            // Assert
            var created = response.Result as CreatedAtActionResult;
            created.Should().NotBeNull();

            // Map the domain object to DTO like the controller does
            var expectedDto = new UserDto
            {
                Id = user.Id,
                Email = user.Email
                // Do not include PasswordHash
            };

            var returnedDto = created.Value as UserDto;
            returnedDto.Should().NotBeNull();
            returnedDto.Should().BeEquivalentTo(expectedDto);
        }

    }
}