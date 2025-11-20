using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class PlansController : ControllerBase
{
    private readonly AppDbContext _context;

    public PlansController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Plan>>> GetPlans() =>
        await _context.Plans.Where(p => p.IsActive).ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Plan>> GetPlan(Guid id)
    {
        var plan = await _context.Plans.FindAsync(id);
        return plan == null ? NotFound() : plan;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<Plan>> CreatePlan(Plan plan)
    {
        _context.Plans.Add(plan);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPlan), new { id = plan.Id }, plan);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePlan(Guid id, Plan plan)
    {
        if (id != plan.Id) return BadRequest();
        _context.Entry(plan).State = EntityState.Modified;

        try { await _context.SaveChangesAsync(); }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Plans.Any(p => p.Id == id)) return NotFound();
            throw;
        }

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePlan(Guid id)
    {
        var plan = await _context.Plans.FindAsync(id);
        if (plan == null) return NotFound();

        _context.Plans.Remove(plan);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
