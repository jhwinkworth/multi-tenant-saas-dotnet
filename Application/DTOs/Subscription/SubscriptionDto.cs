namespace Application.DTOs.Subscription
{
    public class SubscriptionDto
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public Guid PlanId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
