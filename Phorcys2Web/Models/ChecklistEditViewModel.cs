namespace Phorcys.Web.Models;

public class ChecklistEditViewModel {
    public int ChecklistId { get; set; }
    public string Title { get; set; } = string.Empty;

    // JSON-encoded items from Telerik grid
    public string ItemsJson { get; set; } = string.Empty;
}
