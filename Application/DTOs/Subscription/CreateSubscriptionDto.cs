namespace Application.DTOs.Subscription
{
    public class CreateSubscriptionDto
    {
        public Guid PlanId { get; set; }  // Foreign key to Plan
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime? EndDate { get; set; }
    }
}
