using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services
{
    public class PlanService : IPlanService
    {
        private readonly IPlanRepository _planRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;

        public PlanService(IPlanRepository planRepository, ISubscriptionRepository subscriptionRepository)
        {
            _planRepository = planRepository;
            _subscriptionRepository = subscriptionRepository;
        }

        public Plan CreatePlan(string name, decimal pricePerMonth, bool isActive = true)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Plan name cannot be empty");
            if (pricePerMonth < 0) throw new ArgumentException("Plan price must be positive");

            var plan = new Plan
            {
                Id = Guid.NewGuid(),
                Name = name,
                PricePerMonth = pricePerMonth,
                IsActive = isActive
            };

            return _planRepository.Add(plan);
        }

        public List<Plan> GetAllPlans() =>
            _planRepository.GetAll();

        public Plan? GetPlanById(Guid id) =>
            _planRepository.GetById(id);

        public void UpdatePlan(Plan plan) =>
            _planRepository.Update(plan);

        public void DeletePlan(Plan plan)
        {
            // Check for active subscriptions
            var activeSubs = _subscriptionRepository
                .GetAllForTenant(Guid.Empty)
                .Where(s => s.PlanId == plan.Id)
                .ToList();

            if (activeSubs.Any())
                throw new InvalidOperationException("Cannot delete a plan with active subscriptions.");

            _planRepository.Delete(plan);
        }

        public Subscription AssignPlanToSubscription(Subscription subscription, Plan plan, DateTime start, DateTime end)
        {
            if (end <= start) throw new ArgumentException("Subscription end date must be after start date");
            if (!plan.IsActive) throw new InvalidOperationException("Cannot assign an inactive plan");

            subscription.PlanId = plan.Id;
            subscription.StartDate = start;
            subscription.EndDate = end;

            _subscriptionRepository.Update(subscription);
            return subscription;
        }

        public bool IsPlanAvailable(Guid planId)
        {
            var plan = _planRepository.GetById(planId);
            return plan != null && plan.IsActive;
        }
    }
}
