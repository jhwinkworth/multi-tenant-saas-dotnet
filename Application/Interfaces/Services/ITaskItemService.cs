using Application.Interfaces.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.Interfaces.Services
{
    public interface ITaskItemService
    {
        public TaskItem CreateTaskItem(string title, Guid projectId, DateTime createdAt, DateTime? dueDate = null);
        public List<TaskItem> GetAllTasks();
        public List<TaskItem> GetTasksForProject(Guid projectId);
        public TaskItem? GetTaskById(Guid id);
        public void UpdateTask(TaskItem task);
        public void DeleteTask(TaskItem task);
    }
}
