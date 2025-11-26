using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ITenantProvider _tenantProvider;

        public ProjectService(IProjectRepository projectRepository, ITenantProvider tenantProvider)
        {
            _projectRepository = projectRepository;
            _tenantProvider = tenantProvider;
        }

        public Project CreateProject(string name)
        {
            var tenantId = _tenantProvider.TenantId;

            var project = new Project { Id = Guid.NewGuid(), Name = name, TenantId = tenantId };
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
