using Application.Interfaces.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using Application.Interfaces.Services;

namespace Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public Project CreateProject(string name, Guid tenantId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Project name cannot be empty");

            var project = new Project
            {
                Id = Guid.NewGuid(),
                Name = name,
                TenantId = tenantId
            };

            return _projectRepository.Add(project);
        }

        public List<Project> GetAllProjects() =>
            _projectRepository.GetAllForTenant();

        public Project? GetProjectById(Guid id) =>
            _projectRepository.GetById(id);

        public void UpdateProject(Project project)
        {
            if (project == null) throw new ArgumentNullException(nameof(project));
            _projectRepository.Update(project);
        }

        public void DeleteProject(Project project)
        {
            if (project == null) throw new ArgumentNullException(nameof(project));
            _projectRepository.Delete(project);
        }
    }
}
