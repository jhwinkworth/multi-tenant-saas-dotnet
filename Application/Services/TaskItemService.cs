using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.Services
{
    public class TaskItemService : ITaskItemService
    {
        private readonly ITaskItemRepository _taskItemRepository;
        private readonly IProjectRepository _projectRepository;

        public TaskItemService(ITaskItemRepository taskRepository, IProjectRepository projectRepository)
        {
            _taskItemRepository = taskRepository;
            _projectRepository = projectRepository;
        }

        public TaskItem CreateTask(string title, Guid projectId, DateTime? dueDate = null)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Task title cannot be empty");

            // Ensure project exists (tenant filtering handled by EF Core)
            var project = _projectRepository.GetById(projectId)
                ?? throw new ArgumentException("Project not found");

            var task = new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = title,
                ProjectId = projectId,
                Project = project,
                DueDate = dueDate
            };

            return _taskItemRepository.Add(task);
        }

        public List<TaskItem> GetAllTasks() =>
            _taskItemRepository.GetAll();

        public List<TaskItem> GetTasksForProject(Guid projectId) =>
            _taskItemRepository.GetTasksByProjectId(projectId);

        public TaskItem? GetTaskById(Guid id) =>
            _taskItemRepository.GetById(id);

        public void UpdateTask(TaskItem task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            _taskItemRepository.Update(task);
        }

        public void DeleteTask(TaskItem task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            _taskItemRepository.Delete(task);
        }
    }
}
