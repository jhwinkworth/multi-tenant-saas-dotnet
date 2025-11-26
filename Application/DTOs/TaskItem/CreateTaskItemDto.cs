namespace Application.DTOs.TaskItem
{
    public class CreateTaskItemDto
    {
        public string Title { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }
    }
}
