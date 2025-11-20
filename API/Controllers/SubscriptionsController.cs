using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SubscriptionsController : ControllerBase
{
    private readonly AppDbContext _context;

    public SubscriptionsController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Subscription>>> GetSubscriptions() =>
        await _context.Subscriptions
            .Include(s => s.Plan)
            .ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Subscription>> GetSubscription(Guid id)
    {
        var subscription = await _context.Subscriptions
            .Include(s => s.Plan)
            .FirstOrDefaultAsync(s => s.Id == id);
        return subscription == null ? NotFound() : subscription;
    }

    [HttpPost]
    public async Task<ActionResult<Subscription>> CreateSubscription(Subscription subscription)
    {
        subscription.TenantId = _context.CurrentTenantId;
        _context.Subscriptions.Add(subscription);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetSubscription), new { id = subscription.Id }, subscription);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSubscription(Guid id, Subscription subscription)
    {
        if (id != subscription.Id) return BadRequest();
        subscription.TenantId = _context.CurrentTenantId;
        _context.Entry(subscription).State = EntityState.Modified;

        try { await _context.SaveChangesAsync(); }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Subscriptions.Any(s => s.Id == id)) return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSubscription(Guid id)
    {
        var subscription = await _context.Subscriptions
            .FirstOrDefaultAsync(s => s.Id == id);
        if (subscription == null) return NotFound();

        _context.Subscriptions.Remove(subscription);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
