using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProjectsController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Project>>> GetProjects() =>
        await _context.Projects
            .ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Project>> GetProject(Guid id)
    {
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == id);
        return project == null ? NotFound() : project;
    }

    [HttpPost]
    public async Task<ActionResult<Project>> CreateProject(Project project)
    {
        project.TenantId = _context.CurrentTenantId;
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProject(Guid id, Project project)
    {
        if (id != project.Id) return BadRequest();
        project.TenantId = _context.CurrentTenantId;

        _context.Entry(project).State = EntityState.Modified;

        try { await _context.SaveChangesAsync(); }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Projects.Any(p => p.Id == id)) return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(Guid id)
    {
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == id);
        if (project == null) return NotFound();

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
