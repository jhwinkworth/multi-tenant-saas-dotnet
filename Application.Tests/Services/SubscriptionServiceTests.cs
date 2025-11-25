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
    public class SubscriptionServiceTests
    {
        private readonly Mock<ISubscriptionRepository> _subRepoMock;
        private readonly Mock<ITenantProvider> _tenantProviderMock;
        private readonly SubscriptionService _subService;
        private readonly Guid _tenantId = Guid.NewGuid();

        public SubscriptionServiceTests()
        {
            _subRepoMock = new Mock<ISubscriptionRepository>();
            _tenantProviderMock = new Mock<ITenantProvider>();
            _tenantProviderMock.Setup(t => t.TenantId).Returns(_tenantId);

            _subService = new SubscriptionService(_subRepoMock.Object, _tenantProviderMock.Object);
        }

        [Fact]
        public void CreateSubscription_ShouldAssignTenantId()
        {
            _subRepoMock.Setup(r => r.Add(It.IsAny<Subscription>())).Returns((Subscription s) => s);

            var result = _subService.CreateSubscription(Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow.AddMonths(1));

            result.TenantId.Should().Be(_tenantId);
            _subRepoMock.Verify(r => r.Add(It.IsAny<Subscription>()), Times.Once);
        }

        [Fact]
        public void GetAllSubscriptions_ShouldReturnForTenant()
        {
            var subs = new List<Subscription>
            {
                new Subscription { Id = Guid.NewGuid(), TenantId = _tenantId },
                new Subscription { Id = Guid.NewGuid(), TenantId = _tenantId }
            };
            _subRepoMock.Setup(r => r.GetAllForTenant(_tenantId)).Returns(subs);

            var result = _subService.GetAllSubscriptions();

            result.Count.Should().Be(2);
            result.All(s => s.TenantId == _tenantId).Should().BeTrue();
        }

        [Fact]
        public void UpdateSubscription_ShouldCallRepo()
        {
            var sub = new Subscription { Id = Guid.NewGuid(), TenantId = _tenantId };
            _subService.UpdateSubscription(sub);
            _subRepoMock.Verify(r => r.Update(sub), Times.Once);
        }

        [Fact]
        public void DeleteSubscription_ShouldCallRepo()
        {
            var sub = new Subscription { Id = Guid.NewGuid(), TenantId = _tenantId };
            _subService.DeleteSubscription(sub);
            _subRepoMock.Verify(r => r.Delete(sub), Times.Once);
        }
    }
}
