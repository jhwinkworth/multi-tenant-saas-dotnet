using Application.DTOs.TaskItem;
using Application.Interfaces.Services;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskItemsController : ControllerBase
    {
        private readonly ITaskItemService _taskService;

        public TaskItemsController(ITaskItemService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public ActionResult<List<TaskItemDto>> GetAll()
        {
            var tasks = _taskService.GetAllTasks();
            var dto = tasks.Select(t => new TaskItemDto
            {
                Id = t.Id,
                Title = t.Title,
                ProjectId = t.ProjectId,
                DueDate = t.DueDate
            }).ToList();
            return Ok(dto);
        }

        [HttpGet("{id}")]
        public ActionResult<TaskItemDto> GetById(Guid id)
        {
            var task = _taskService.GetTaskById(id);
            if (task == null) return NotFound();

            return new TaskItemDto
            {
                Id = task.Id,
                Title = task.Title,
                ProjectId = task.ProjectId,
                DueDate = task.DueDate
            };
        }

        [HttpPost]
        public ActionResult<TaskItemDto> Create([FromBody] CreateTaskItemDto dto)
        {
            var task = _taskService.CreateTask(dto.Title, dto.ProjectId, dto.DueDate);

            return CreatedAtAction(nameof(GetById), new { id = task.Id }, new TaskItemDto
            {
                Id = task.Id,
                Title = task.Title,
                ProjectId = task.ProjectId,
                DueDate = task.DueDate
            });
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] CreateTaskItemDto dto)
        {
            var task = _taskService.GetTaskById(id);
            if (task == null) return NotFound();

            task.Title = dto.Title;
            task.DueDate = dto.DueDate;
            _taskService.UpdateTask(task);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var task = _taskService.GetTaskById(id);
            if (task == null) return NotFound();

            _taskService.DeleteTask(task);
            return NoContent();
        }
    }
}
