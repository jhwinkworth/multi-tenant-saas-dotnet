using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Services;
using Domain.Entities;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Application.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<ITenantProvider> _tenantProviderMock;
        private readonly UserService _userService;
        private readonly Guid _tenantId = Guid.NewGuid();

        public UserServiceTests()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _tenantProviderMock = new Mock<ITenantProvider>();
            _tenantProviderMock.Setup(t => t.TenantId).Returns(_tenantId);

            _userService = new UserService(_userRepoMock.Object, _tenantProviderMock.Object);
        }

        [Fact]
        public void RegisterUser_ShouldAssignTenantId()
        {
            _userRepoMock.Setup(r => r.Add(It.IsAny<User>())).Returns((User u) => u);

            var result = _userService.CreateUser("test@example.com", "1234", "Bob Mortimer", false);

            result.TenantId.Should().Be(_tenantId);
            result.Email.Should().Be("test@example.com");
            _userRepoMock.Verify(r => r.Add(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public void GetAllUsers_ShouldReturnForTenant()
        {
            var users = new List<User>
            {
                new User { Id = Guid.NewGuid(), TenantId = _tenantId },
                new User { Id = Guid.NewGuid(), TenantId = _tenantId }
            };
            _userRepoMock.Setup(r => r.GetAllForTenant(_tenantId)).Returns(users);

            var result = _userService.GetAllUsers();

            result.Count.Should().Be(2);
            result.All(u => u.TenantId == _tenantId).Should().BeTrue();
        }

        [Fact]
        public void UpdateUser_ShouldCallRepo()
        {
            var user = new User { Id = Guid.NewGuid(), TenantId = _tenantId };
            _userService.UpdateUser(user);
            _userRepoMock.Verify(r => r.Update(user), Times.Once);
        }

        [Fact]
        public void DeleteUser_ShouldCallRepo()
        {
            var user = new User { Id = Guid.NewGuid(), TenantId = _tenantId };
            _userService.DeleteUser(user);
            _userRepoMock.Verify(r => r.Delete(user), Times.Once);
        }
    }
}
