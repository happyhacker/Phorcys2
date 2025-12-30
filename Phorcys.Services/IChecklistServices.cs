using Phorcys.Domain;

namespace Phorcys.Services;

public interface IChecklistServices {
    int CreateChecklistWithItems(
        int userId,
        string title,
        IEnumerable<(string Title, int SequenceNumber)> items);

    void Delete(int id);
    IEnumerable<Checklist> GetChecklists(int userId);
    ChecklistInstanceItemsResult? GetChecklistInstanceItems(int userId, int checklistId);
}
