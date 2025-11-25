using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.Interfaces.Repositories
{
    public interface IProjectRepository
    {
        Project Add(Project project);
        Project? GetById(Guid id);
        List<Project> GetAllForTenant();
        void Update(Project project);
        void Delete(Project project);
    }
}
