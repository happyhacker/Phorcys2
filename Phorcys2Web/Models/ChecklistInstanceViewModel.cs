using System.Collections.Generic;
using System.Linq;

namespace Phorcys.Web.Models;

public class ChecklistInstanceItemViewModel {
    public int ChecklistInstanceItemId { get; set; }
    public int? SequenceNumber { get; set; }
    public bool IsChecked { get; set; }
    public string Title { get; set; } = string.Empty;
}

public class ChecklistInstanceViewModel {
    public int ChecklistId { get; set; }
    public string Title { get; set; } = string.Empty;
    public IEnumerable<ChecklistInstanceItemViewModel> Items { get; set; } = Enumerable.Empty<ChecklistInstanceItemViewModel>();
}
