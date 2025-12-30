using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Phorcys.Data;
using Phorcys.Domain;
using System.Collections.Concurrent;

namespace Phorcys.Services {
    public class ChecklistServices : IChecklistServices {
        private readonly PhorcysContext _context;
        private readonly ILogger<ChecklistServices> _logger;
        private readonly ConcurrentDictionary<string, ChecklistInstanceCacheEntry> _instanceCache = new();

        public ChecklistServices(PhorcysContext context, ILogger<ChecklistServices> logger) {
            _context = context;
            _logger = logger;
        }

        private sealed class ChecklistInstanceCacheEntry {
            public string Title { get; set; } = string.Empty;
            public List<ChecklistInstanceItem> Items { get; set; } = new();
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

                _context.Checklists.Add(checklist);
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

        public void Delete(int id) {
            try {
                var checklist = _context.Checklists.Find(id);
                if(checklist != null) {
                    _context.Checklists.Remove(checklist);
                    _context.SaveChanges();
                }
            }
            catch(DbUpdateException ex) {
                _logger.LogError(ex, "Error deleting Checklist {id}: {ErrorMessage}", id, ex.Message);
                throw;
            }
            catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        public IEnumerable<Checklist> GetChecklists(int userId) {
            return _context.Checklists
                .Include(c => c.Items)
                .Where(c => c.UserId == userId)
                .OrderBy(c => c.Title)
                .ToList();
        }

        public ChecklistInstanceItemsResult? GetChecklistInstanceItems(int userId, int checklistId) {
            var cacheKey = GetCacheKey(userId, checklistId);

            if(_instanceCache.TryGetValue(cacheKey, out var cachedEntry)) {
                return CloneResult(checklistId, cachedEntry);
            }

            var createdEntry = CreateInstanceCacheEntry(userId, checklistId);
            if(createdEntry == null) {
                return null;
            }

            _instanceCache[cacheKey] = createdEntry;

            return CloneResult(checklistId, createdEntry);
        }

        public ChecklistInstanceItemsResult? UpdateChecklistInstanceItems(int userId, int checklistId, IEnumerable<ChecklistInstanceItem> updatedItems) {
            var cacheKey = GetCacheKey(userId, checklistId);

            var cacheEntry = _instanceCache.GetOrAdd(cacheKey, _ => CreateInstanceCacheEntry(userId, checklistId) ?? new ChecklistInstanceCacheEntry());

            if(string.IsNullOrWhiteSpace(cacheEntry.Title) && !cacheEntry.Items.Any()) {
                return null;
            }

            var updatedLookup = (updatedItems ?? Enumerable.Empty<ChecklistInstanceItem>())
                .Where(i => i.SequenceNumber.HasValue)
                .ToDictionary(i => i.SequenceNumber!.Value, i => i.IsChecked);

            foreach(var item in cacheEntry.Items) {
                if(item.SequenceNumber.HasValue && updatedLookup.TryGetValue(item.SequenceNumber.Value, out var isChecked)) {
                    item.IsChecked = isChecked;
                }
            }

            return CloneResult(checklistId, cacheEntry);
        }

        private string GetCacheKey(int userId, int checklistId) => $"{userId}:{checklistId}";

        private ChecklistInstanceCacheEntry? CreateInstanceCacheEntry(int userId, int checklistId) {
            var checklist = _context.Checklists
                .Include(c => c.Items)
                .FirstOrDefault(c => c.UserId == userId && c.ChecklistId == checklistId);

            if(checklist == null) {
                _logger.LogWarning("Checklist {ChecklistId} not found for user {UserId} when requesting instance items.", checklistId, userId);
                return null;
            }

            var instanceItems = checklist.Items
                .OrderBy(i => i.SequenceNumber)
                .ThenBy(i => i.ChecklistItemId)
                .Select(item => new ChecklistInstanceItem {
                    ChecklistInstanceId = 0,
                    SequenceNumber = item.SequenceNumber,
                    Title = item.Title,
                    IsChecked = false,
                    Created = DateTime.Now
                })
                .ToList();

            return new ChecklistInstanceCacheEntry {
                Title = checklist.Title,
                Items = instanceItems
            };
        }

        private static ChecklistInstanceItemsResult CloneResult(int checklistId, ChecklistInstanceCacheEntry entry) {
            var clonedItems = entry.Items
                .Select(item => new ChecklistInstanceItem {
                    ChecklistInstanceId = item.ChecklistInstanceId,
                    SequenceNumber = item.SequenceNumber,
                    Title = item.Title,
                    IsChecked = item.IsChecked,
                    Created = item.Created
                })
                .ToList();

            return new ChecklistInstanceItemsResult {
                ChecklistId = checklistId,
                Title = entry.Title,
                Items = clonedItems
            };
        }
    }
}
