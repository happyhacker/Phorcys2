namespace Phorcys.Web.Models;

public class ChecklistCreateViewModel {
    public string Title { get; set; } = string.Empty;

    // JSON-encoded items from Telerik grid
    public string ItemsJson { get; set; } = string.Empty;
}
