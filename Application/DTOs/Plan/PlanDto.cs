namespace Application.DTOs.Plan
{
    public class PlanDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal PricePerMonth { get; set; }
        public string? Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

    }
}
