namespace Phorcys.Domain;
public class ChecklistInstance {
    public int ChecklistInstanceId { get; set; }
    public int ChecklistId { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime Created { get; set; }

    public Checklist Checklist { get; set; } = null!;
    public ICollection<ChecklistInstanceItem> Items { get; set; } = new List<ChecklistInstanceItem>();
}
