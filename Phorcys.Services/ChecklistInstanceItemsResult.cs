using Phorcys.Domain;

namespace Phorcys.Services;

public class ChecklistInstanceItemsResult {
    public int ChecklistId { get; set; }
    public string Title { get; set; } = string.Empty;
    public IEnumerable<ChecklistInstanceItem> Items { get; set; } = Enumerable.Empty<ChecklistInstanceItem>();
}
