namespace Domain.Entities;

public class Subscription
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TenantId { get; set; }
    public Guid PlanId { get; set; }  // Foreign key to Plan

    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiresAt { get; set; }

    // Navigation
    public Tenant Tenant { get; set; }
    public Plan Plan { get; set; }
}
