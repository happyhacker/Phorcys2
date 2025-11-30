namespace Phorcys.Domain;
public class ChecklistInstanceItem {
    public int ChecklistInstanceItemId { get; set; }
    public int ChecklistInstanceId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int? SequenceNumber { get; set; }
    public bool IsChecked { get; set; }
    public DateTime Created { get; set; }

    public ChecklistInstance ChecklistInstance { get; set; } = null!;
}