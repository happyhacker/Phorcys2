using Microsoft.Extensions.Logging;
using Phorcys.Data;
using Phorcys.Domain;

namespace Phorcys.Services {
    public class ChecklistServices : IChecklistServices {
        private readonly PhorcysContext _context;
        private readonly ILogger<ChecklistServices> _logger;

        public ChecklistServices(PhorcysContext context, ILogger<ChecklistServices> logger) {
            _context = context;
            _logger = logger;
        }

        public int CreateChecklistWithItems(
            int userId,
            string title,
            IEnumerable<(string Title, int SequenceNumber)> items) {
            try {
                var checklist = new Checklist {
                    UserId = userId,
                    Title = title,
                    Created = DateTime.Now
                };

                var orderedItems = (items ?? Enumerable.Empty<(string Title, int SequenceNumber)>())
                    .Where(i => !string.IsNullOrWhiteSpace(i.Title))
                    .OrderBy(i => i.SequenceNumber)
                    .ToList();

                foreach(var item in orderedItems) {
                    checklist.Items.Add(new ChecklistItem {
                        Title = item.Title,
                        SequenceNumber = item.SequenceNumber,
                        Created = DateTime.Now
                    });
                }

                // Add only the parent; EF will insert children based on the Items collection
                _context.Checklists.Add(checklist);

                // Single SaveChanges: EF wraps this in a transaction and does parent+children in one go
                _context.SaveChanges();

                return checklist.ChecklistId;
            }
            catch(Exception ex) {
                _logger.LogError(ex,
                    "Error creating checklist '{Title}' for user {UserId}. Items: {@Items}",
                    title, userId, items);

                throw;
            }
        }
    }
}
