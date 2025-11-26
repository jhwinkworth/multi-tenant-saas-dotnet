using Xunit;
using Moq;
using Application.Services;
using Application.Interfaces.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using FluentAssertions;

namespace Application.Tests.Services
{
    public class PlanServiceTests
    {
        private readonly Mock<IPlanRepository> _planRepoMock;
        private readonly Mock<ISubscriptionRepository> _subscriptionRepoMock;
        private readonly PlanService _planService;

        public PlanServiceTests()
        {
            _planRepoMock = new Mock<IPlanRepository>();
            _subscriptionRepoMock = new Mock<ISubscriptionRepository>();
            _planService = new PlanService(_planRepoMock.Object, _subscriptionRepoMock.Object);
        }

        [Fact]
        public void CreatePlan_ShouldReturnPlan_WhenValidData()
        {
            // Arrange
            var name = "Pro Plan";
            var price = 29.99m;

            _planRepoMock.Setup(r => r.Add(It.IsAny<Plan>()))
                .Returns((Plan p) => p);

            // Act
            var plan = _planService.CreatePlan(name, price, "A plan");

            // Assert
            plan.Name.Should().Be(name);
            plan.PricePerMonth.Should().Be(price);
            plan.IsActive.Should().BeTrue();
            _planRepoMock.Verify(r => r.Add(It.IsAny<Plan>()), Times.Once);
        }

        [Fact]
        public void CreatePlan_ShouldThrow_WhenPriceNegative()
        {
            Action act = () => _planService.CreatePlan("Plan", -10, "A plan");
            act.Should().Throw<ArgumentException>().WithMessage("Plan price must be positive");
        }

        [Fact]
        public void IsPlanAvailable_ShouldReturnTrue_WhenPlanIsActive()
        {
            var planId = Guid.NewGuid();
            _planRepoMock.Setup(r => r.GetById(planId))
                .Returns(new Plan { Id = planId, IsActive = true });

            var result = _planService.IsPlanAvailable(planId);

            result.Should().BeTrue();
        }

        [Fact]
        public void IsPlanAvailable_ShouldReturnFalse_WhenPlanInactive()
        {
            var planId = Guid.NewGuid();
            _planRepoMock.Setup(r => r.GetById(planId))
                .Returns(new Plan { Id = planId, IsActive = false });

            var result = _planService.IsPlanAvailable(planId);

            result.Should().BeFalse();
        }

        [Fact]
        public void AssignPlanToSubscription_ShouldUpdateSubscription_WhenValid()
        {
            var plan = new Plan { Id = Guid.NewGuid(), IsActive = true };
            var sub = new Subscription { Id = Guid.NewGuid() };
            var start = DateTime.UtcNow;
            var end = start.AddMonths(1);

            _subscriptionRepoMock.Setup(r => r.Update(sub));

            var result = _planService.AssignPlanToSubscription(sub, plan, start, end);

            result.PlanId.Should().Be(plan.Id);
            result.StartDate.Should().Be(start);
            result.EndDate.Should().Be(end);
            _subscriptionRepoMock.Verify(r => r.Update(sub), Times.Once);
        }

        [Fact]
        public void AssignPlanToSubscription_ShouldThrow_WhenPlanInactive()
        {
            var plan = new Plan { Id = Guid.NewGuid(), IsActive = false };
            var sub = new Subscription { Id = Guid.NewGuid() };
            var start = DateTime.UtcNow;
            var end = start.AddMonths(1);

            Action act = () => _planService.AssignPlanToSubscription(sub, plan, start, end);
            act.Should().Throw<InvalidOperationException>().WithMessage("Cannot assign an inactive plan");
        }

        [Fact]
        public void AssignPlanToSubscription_ShouldThrow_WhenEndBeforeStart()
        {
            var plan = new Plan { Id = Guid.NewGuid(), IsActive = true };
            var sub = new Subscription { Id = Guid.NewGuid() };
            var start = DateTime.UtcNow;
            var end = start.AddDays(-1);

            Action act = () => _planService.AssignPlanToSubscription(sub, plan, start, end);
            act.Should().Throw<ArgumentException>().WithMessage("Subscription end date must be after start date");
        }
    }
}
