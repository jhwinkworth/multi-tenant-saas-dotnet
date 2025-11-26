namespace Domain.Entities;

public class Plan
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public decimal PricePerMonth { get; set; } = 0;
    public string? Description { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    // Navigation
    public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
