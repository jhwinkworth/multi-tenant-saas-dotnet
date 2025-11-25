using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Interfaces.Services
{
    public interface IPlanService
    {
        Plan CreatePlan(string name, decimal pricePerMonth, bool isActive = true);
        List<Plan> GetAllPlans();
        Plan? GetPlanById(Guid id);
        void UpdatePlan(Plan plan);
        void DeletePlan(Plan plan);
        Subscription AssignPlanToSubscription(Subscription subscription, Plan plan, DateTime start, DateTime end);
        bool IsPlanAvailable(Guid planId);
    }
}
