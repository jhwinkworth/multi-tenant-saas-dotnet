using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.Interfaces.Repositories
{
    public interface IPlanRepository
    {
        List<Plan> GetAll();
        Plan? GetById(Guid id);
        Plan Add(Plan plan);
        void Update(Plan plan);
        void Delete(Plan plan);
    }
}
