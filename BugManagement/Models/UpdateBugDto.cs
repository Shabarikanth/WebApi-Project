namespace BugManagement.Models
{
    public class UpdateBugDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Status { get; set; } = "Open";
        public string? Severity { get; set; }
        public string? AssignedTo { get; set; }
    }
}
