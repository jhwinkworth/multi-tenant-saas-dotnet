using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.Interfaces.Repositories
{
    public interface ITaskItemRepository
    {
        TaskItem Add(TaskItem task);
        TaskItem? GetById(Guid id);
        List<TaskItem> GetAll();
        List<TaskItem> GetTasksByProjectId(Guid projectId);
        void Update(TaskItem task);
        void Delete(TaskItem task);
    }
}
