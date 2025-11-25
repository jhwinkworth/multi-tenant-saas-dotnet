using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly AppDbContext _dbContext;

        public SubscriptionRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Subscription> GetAllForTenant(Guid tenantId) =>
            _dbContext.Subscriptions
                .Where(s => s.TenantId == tenantId)
                .AsNoTracking()
                .ToList();

        public Subscription? GetById(Guid id, Guid tenantId) =>
            _dbContext.Subscriptions
                .AsNoTracking()
                .FirstOrDefault(s => s.Id == id && s.TenantId == tenantId);

        public Subscription Add(Subscription subscription)
        {
            _dbContext.Subscriptions.Add(subscription);
            _dbContext.SaveChanges();
            return subscription;
        }

        public void Update(Subscription subscription)
        {
            _dbContext.Subscriptions.Update(subscription);
            _dbContext.SaveChanges();
        }

        public void Delete(Subscription subscription)
        {
            _dbContext.Subscriptions.Remove(subscription);
            _dbContext.SaveChanges();
        }
    }
}
