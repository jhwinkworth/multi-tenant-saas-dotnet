using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class TenantsController : ControllerBase
{
    private readonly AppDbContext _context;

    public TenantsController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Tenant>>> GetTenants() =>
        await _context.Tenants.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Tenant>> GetTenant(Guid id)
    {
        var tenant = await _context.Tenants.FindAsync(id);
        return tenant == null ? NotFound() : tenant;
    }

    [HttpPost]
    public async Task<ActionResult<Tenant>> CreateTenant(Tenant tenant)
    {
        _context.Tenants.Add(tenant);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetTenant), new { id = tenant.Id }, tenant);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTenant(Guid id, Tenant tenant)
    {
        if (id != tenant.Id) return BadRequest();
        _context.Entry(tenant).State = EntityState.Modified;

        try { await _context.SaveChangesAsync(); }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Tenants.Any(t => t.Id == id)) return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTenant(Guid id)
    {
        var tenant = await _context.Tenants.FindAsync(id);
        if (tenant == null) return NotFound();

        _context.Tenants.Remove(tenant);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
