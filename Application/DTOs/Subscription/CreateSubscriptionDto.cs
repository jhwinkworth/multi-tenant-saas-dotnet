namespace Application.DTOs.Subscription
{
    public class CreateSubscriptionDto
    {
        public Guid PlanId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
