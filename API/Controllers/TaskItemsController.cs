using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class TaskItemsController : ControllerBase
{
    private readonly AppDbContext _context;

    public TaskItemsController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetTaskItems() =>
        await _context.TaskItems
            .Include(t => t.Project)
            .ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItem>> GetTaskItem(Guid id)
    {
        var task = await _context.TaskItems
            .Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.Id == id);
        return task == null ? NotFound() : task;
    }

    [HttpPost]
    public async Task<ActionResult<TaskItem>> CreateTaskItem(TaskItem task)
    {
        _context.TaskItems.Add(task);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetTaskItem), new { id = task.Id }, task);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTaskItem(Guid id, TaskItem task)
    {
        if (id != task.Id) return BadRequest();
        _context.Entry(task).State = EntityState.Modified;

        try { await _context.SaveChangesAsync(); }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.TaskItems.Any(t => t.Id == id)) return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTaskItem(Guid id)
    {
        var task = await _context.TaskItems
            .Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.Id == id && t.Project.TenantId == _context.CurrentTenantId);
        if (task == null) return NotFound();

        _context.TaskItems.Remove(task);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
