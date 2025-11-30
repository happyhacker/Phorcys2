using Phorcys.Data;
using Phorcys.Domain;
using Phorcys.Services;

public class ChecklistServices : IChecklistServices {
    private readonly PhorcysContext _context;

    public ChecklistServices(PhorcysContext context) {
        _context = context;
    }

    public int CreateChecklistWithItems(
        int userId,
        string title,
        IEnumerable<(string Title, int SequenceNumber)> items) {
        var checklist = new Checklist {
            UserId = userId,
            Title = title,
            Created = DateTime.UtcNow
        };

        _context.Checklists.Add(checklist);
        _context.SaveChanges(); // get ChecklistId

        var orderedItems = (items ?? Enumerable.Empty<(string Title, int SequenceNumber)>())
            .Where(i => !string.IsNullOrWhiteSpace(i.Title))
            .OrderBy(i => i.SequenceNumber)
            .ToList();

        foreach(var item in orderedItems) {
            _context.ChecklistItems.Add(new ChecklistItem {
                ChecklistId = checklist.ChecklistId,
                Title = item.Title,
                SequenceNumber = item.SequenceNumber,
                Created = DateTime.UtcNow
            });
        }

        if(orderedItems.Count > 0) {
            _context.SaveChanges();
        }

        return checklist.ChecklistId;
    }
}
