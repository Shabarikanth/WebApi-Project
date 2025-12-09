namespace BugManagement.Models
{
    public class Bug
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Status { get; set; } = "Open"; // Open, Work In Progress, Closed, Hold, Rejected
        public string? Severity { get; set; }
        public string? AssignedTo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
