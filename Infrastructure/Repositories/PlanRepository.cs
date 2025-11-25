using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class PlanRepository : IPlanRepository
    {
        private readonly AppDbContext _dbContext;

        public PlanRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Plan> GetAll() =>
            _dbContext.Plans.AsNoTracking().ToList();

        public Plan? GetById(Guid id) =>
            _dbContext.Plans.AsNoTracking().FirstOrDefault(p => p.Id == id);

        public Plan Add(Plan plan)
        {
            _dbContext.Plans.Add(plan);
            _dbContext.SaveChanges();
            return plan;
        }

        public void Update(Plan plan)
        {
            _dbContext.Plans.Update(plan);
            _dbContext.SaveChanges();
        }

        public void Delete(Plan plan)
        {
            _dbContext.Plans.Remove(plan);
            _dbContext.SaveChanges();
        }
    }
}
