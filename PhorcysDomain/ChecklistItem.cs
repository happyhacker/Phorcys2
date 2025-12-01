namespace Phorcys.Domain;
public class ChecklistItem {
    public int ChecklistItemId { get; set; }
    public int ChecklistId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int? SequenceNumber { get; set; }
    public DateTime? Created { get; set; }

    public Checklist Checklist { get; set; } = null!;
}
