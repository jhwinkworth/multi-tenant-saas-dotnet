using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.Interfaces.Repositories
{
    public interface ISubscriptionRepository
    {
        List<Subscription> GetAllForTenant(Guid tenantId);
        Subscription? GetById(Guid id, Guid tenantId);
        Subscription Add(Subscription subscription);
        void Update(Subscription subscription);
        void Delete(Subscription subscription);
    }
}
