using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.Interfaces.Services
{
    public interface ISubscriptionService
    {
        public Subscription CreateSubscription(Guid planId, DateTime start, DateTime end);
        public List<Subscription> GetAllSubscriptions();
        public Subscription? GetSubscriptionById(Guid id);
        public void UpdateSubscription(Subscription subscription);
        public void DeleteSubscription(Subscription subscription);
    }
}
