using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _context;

        public ProjectRepository(AppDbContext context)
        {
            _context = context;
        }

        // Add new project
        public Project Add(Project project)
        {
            _context.Projects.Add(project);
            _context.SaveChanges();
            return project;
        }

        // Get project by ID (tenant filtering handled by EF Core)
        public Project? GetById(Guid id)
        {
            return _context.Projects.FirstOrDefault(p => p.Id == id);
        }

        // Get all projects for current tenant
        public List<Project> GetAllForTenant()
        {
            return _context.Projects.ToList();
        }

        // Update project
        public void Update(Project project)
        {
            _context.Projects.Update(project);
            _context.SaveChanges();
        }

        // Delete project
        public void Delete(Project project)
        {
            _context.Projects.Remove(project);
            _context.SaveChanges();
        }
    }
}
