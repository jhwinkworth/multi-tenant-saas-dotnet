namespace Application.DTOs.TaskItem
{
    public class TaskItemDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
        public Guid TenantId { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
