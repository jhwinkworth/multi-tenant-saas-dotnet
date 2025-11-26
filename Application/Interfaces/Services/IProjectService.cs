using Application.Interfaces.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.Interfaces.Services
{
    public interface IProjectService
    {
        public Project CreateProject(string name);
        public List<Project> GetAllProjects();
        public Project? GetProjectById(Guid id);
        public void UpdateProject(Project project);
        public void DeleteProject(Project project);
    }
}
