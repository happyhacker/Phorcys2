using Phorcys.Domain;

namespace Phorcys.Services;

public interface IChecklistServices {
    int CreateChecklistWithItems(
        int userId,
        string title,
        IEnumerable<(string Title, int SequenceNumber)> items);

    void Delete(int id);
    IEnumerable<Checklist> GetChecklists(int userId);
    Checklist? GetChecklistById(int userId, int checklistId);
    void UpdateChecklistWithItems(
        int userId,
        int checklistId,
        string title,
        IEnumerable<(string Title, int SequenceNumber)> items);
    ChecklistInstanceItemsResult? GetChecklistInstanceItems(int userId, int checklistId);
    ChecklistInstanceItemsResult? UpdateChecklistInstanceItems(
        int userId,
        int checklistId,
        IEnumerable<ChecklistInstanceItem> updatedItems);
}
