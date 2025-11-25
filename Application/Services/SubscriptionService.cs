using Application.Interfaces;
using Application.Interfaces.Services;
using Application.Interfaces.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly ITenantProvider _tenantProvider;

        public SubscriptionService(ISubscriptionRepository subscriptionRepository, ITenantProvider tenantProvider)
        {
            _subscriptionRepository = subscriptionRepository;
            _tenantProvider = tenantProvider;
        }

        public Subscription CreateSubscription(Guid planId, DateTime start, DateTime end)
        {
            if (end <= start)
                throw new ArgumentException("Subscription end date must be after start date");

            var subscription = new Subscription
            {
                Id = Guid.NewGuid(),
                PlanId = planId,
                TenantId = _tenantProvider.TenantId,
                StartDate = start,
                EndDate = end
            };

            return _subscriptionRepository.Add(subscription);
        }

        public List<Subscription> GetAllSubscriptions() =>
            _subscriptionRepository.GetAllForTenant(_tenantProvider.TenantId);

        public Subscription? GetSubscriptionById(Guid id) =>
            _subscriptionRepository.GetById(id, _tenantProvider.TenantId);

        public void UpdateSubscription(Subscription subscription) =>
            _subscriptionRepository.Update(subscription);

        public void DeleteSubscription(Subscription subscription) =>
            _subscriptionRepository.Delete(subscription);
    }
}
