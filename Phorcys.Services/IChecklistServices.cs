using Phorcys.Domain;

namespace Phorcys.Services;

public interface IChecklistServices {
    int CreateChecklistWithItems(
        int userId,
        string title,
        IEnumerable<(string Title, int SequenceNumber)> items);

    IEnumerable<Checklist> GetChecklists(int userId);
}
