using System;

namespace Phorcys.Web.Models;

public class ChecklistIndexViewModel {
    public int ChecklistId { get; set; }

    [System.ComponentModel.DisplayName("Title")]
    public string Title { get; set; } = string.Empty;

    [System.ComponentModel.DisplayName("Created")]
    public DateTime Created { get; set; }

    [System.ComponentModel.DisplayName("Last Modified")]
    public DateTime? LastModified { get; set; }

    [System.ComponentModel.DisplayName("Items")]
    public int ItemCount { get; set; }
}
