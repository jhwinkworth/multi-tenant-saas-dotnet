using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class TaskItemRepository : ITaskItemRepository
    {
        private readonly AppDbContext _context;

        public TaskItemRepository(AppDbContext context)
        {
            _context = context;
        }

        // Add a task
        public TaskItem Add(TaskItem task)
        {
            _context.TaskItems.Add(task);
            _context.SaveChanges();
            return task;
        }
        public TaskItem? GetById(Guid id)
        {
            return _context.TaskItems
                .Include(t => t.Project)
                .FirstOrDefault(t => t.Id == id);
        }

        public List<TaskItem> GetAll()
        {
            return _context.TaskItems
                .Include(t => t.Project)
                .ToList();
        }

        public List<TaskItem> GetTasksByProjectId(Guid projectId)
        {
            return _context.TaskItems
                .Where(t => t.ProjectId == projectId)
                .Include(t => t.Project)
                .ToList();
        }

        public void Update(TaskItem task)
        {
            _context.TaskItems.Update(task);
            _context.SaveChanges();
        }

        public void Delete(TaskItem task)
        {
            _context.TaskItems.Remove(task);
            _context.SaveChanges();
        }
    }
}
