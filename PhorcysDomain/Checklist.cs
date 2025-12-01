namespace Phorcys.Domain;

public class Checklist {
    public int ChecklistId { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }

    public ICollection<ChecklistItem> Items { get; set; } = new List<ChecklistItem>();
    public ICollection<ChecklistInstance> Instances { get; set; } = new List<ChecklistInstance>();
}