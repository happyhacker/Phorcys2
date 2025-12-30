using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Phorcys.Data.DTOs;
using Phorcys.Domain;
using Phorcys.Services;
using Phorcys.Web.Models;
using Phorcys2Web.Controllers;
using System.Linq;
using System.Text.Json;

namespace Phorcys.Web.Controllers {
    public class ChecklistController : Controller {
        private readonly IChecklistServices _checklistService;
        private readonly UserServices _userServices;
        private readonly ILogger<ChecklistController> _logger;

        public ChecklistController(
            ILogger<ChecklistController> logger,
            UserServices userServices,
            IChecklistServices checklistService) {
            _checklistService = checklistService;
            _logger = logger;
            _userServices = userServices;
        }

        [Authorize, HttpGet]
        public IActionResult Index() {
            var userId = _userServices.GetUserId();
            var checklists = _checklistService.GetChecklists(userId)
                .Select(c => new ChecklistIndexViewModel {
                    ChecklistId = c.ChecklistId,
                    Title = c.Title,
                    Created = c.Created,
                    LastModified = c.LastModified,
                    ItemCount = c.Items?.Count ?? 0
                })
                .ToList();

            return View(checklists);
        }

        [Authorize, HttpGet]
        public IActionResult Create() {
            return View(new ChecklistCreateViewModel());
        }

        [Authorize, HttpGet]
        public IActionResult CheckList(int id) {
            var userId = _userServices.GetUserId();
            var instanceItemsResult = _checklistService.GetChecklistInstanceItems(userId, id);

            if(instanceItemsResult == null) {
                return NotFound();
            }

            var model = new ChecklistInstanceViewModel {
                ChecklistId = instanceItemsResult.ChecklistId,
                Title = instanceItemsResult.Title,
                Items = instanceItemsResult.Items.Select(i => new ChecklistInstanceItemViewModel {
                    ChecklistInstanceItemId = i.ChecklistInstanceItemId,
                    SequenceNumber = i.SequenceNumber,
                    IsChecked = i.IsChecked,
                    Title = i.Title
                }).ToList()
            };

            return View(model);
        }

        [Authorize]
        public IActionResult ChecklistInstanceItems_Read([DataSourceRequest] DataSourceRequest request, int id) {
            var userId = _userServices.GetUserId();
            var instanceItemsResult = _checklistService.GetChecklistInstanceItems(userId, id);

            if(instanceItemsResult == null) {
                return NotFound();
            }

            var items = instanceItemsResult.Items.Select(i => new ChecklistInstanceItemViewModel {
                ChecklistInstanceItemId = i.ChecklistInstanceItemId,
                SequenceNumber = i.SequenceNumber,
                IsChecked = i.IsChecked,
                Title = i.Title
            });

            return Json(items.ToDataSourceResult(request));
        }

        [Authorize, HttpPost]
        public IActionResult ChecklistInstanceItems_Update(
            [DataSourceRequest] DataSourceRequest request,
            int checklistId,
            [Bind(Prefix = "models")] IEnumerable<ChecklistInstanceItemViewModel> items) {

            var userId = _userServices.GetUserId();

            var incomingItems = items ?? Enumerable.Empty<ChecklistInstanceItemViewModel>();

            var updateResult = _checklistService.UpdateChecklistInstanceItems(
                userId,
                checklistId,
                incomingItems.Select(i => new ChecklistInstanceItem {
                    ChecklistInstanceId = 0,
                    ChecklistInstanceItemId = i.ChecklistInstanceItemId,
                    SequenceNumber = i.SequenceNumber,
                    Title = i.Title,
                    IsChecked = i.IsChecked,
                    Created = DateTime.Now
                }));

            var updatedLookup = (updateResult?.Items ?? Enumerable.Empty<ChecklistInstanceItem>())
                .ToDictionary(i => i.ChecklistInstanceItemId, i => i);

            var responseItems = incomingItems
                .Select(i => updatedLookup.TryGetValue(i.ChecklistInstanceItemId, out var updated)
                    ? new ChecklistInstanceItemViewModel {
                        ChecklistInstanceItemId = updated.ChecklistInstanceItemId,
                        SequenceNumber = updated.SequenceNumber,
                        IsChecked = updated.IsChecked,
                        Title = updated.Title
                    }
                    : i);

            return Json(responseItems.ToDataSourceResult(request, ModelState));
        }

        [Authorize, HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(ChecklistCreateViewModel model) {
            if(!ModelState.IsValid) {
                return View(model);
            }

            List<ChecklistItemCreateDto>? dtoItems = null;

            if(!string.IsNullOrWhiteSpace(model.ItemsJson)) {
                try {
                    dtoItems = JsonSerializer.Deserialize<List<ChecklistItemCreateDto>>(model.ItemsJson);
                }
                catch(Exception ex) {
                    var safeTitle = (model.Title ?? string.Empty)
                        .Replace("\r", " ")
                        .Replace("\n", " ");

                    var safeItemsJson = (model.ItemsJson ?? string.Empty)
                        .Replace("\r", " ")
                        .Replace("\n", " ");

                    _logger.LogError(ex,
                        "Unable to parse checklist items JSON for checklist '{Title}'. Raw JSON: {ItemsJson}",
                        safeTitle,
                        safeItemsJson);

                    ModelState.AddModelError(string.Empty, "Unable to parse checklist items.");
                    return View(model);
                }
            }

            var items = (dtoItems ?? new List<ChecklistItemCreateDto>())
                .Where(x => !string.IsNullOrWhiteSpace(x.Title))
                .Select(x => (x.Title, x.SequenceNumber));

            try {
                int userId = _userServices.GetUserId();

                var checklistId = _checklistService.CreateChecklistWithItems(
                    userId,
                    model.Title,
                    items);

                TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] =
                    "The Checklist was successfully added.";

                return RedirectToAction("Index");
            }
            catch(Exception ex) {
                _logger.LogError(ex,
                    "Error creating checklist '{Title}' for current user. Model: {@Model}",
                    model.Title,
                    model);

                ModelState.AddModelError(string.Empty,
                    "There was a problem saving the checklist. Please try again. If the problem persists, contact support.");
                TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] =
                    "There was a problem saving the checklist. Please try again. If the problem persists, contact support.";

                return View(model);
            }
        }

        [Authorize, HttpPost, ValidateAntiForgeryToken]
        public IActionResult Delete(int checklistId) {
            try {
                var userId = _userServices.GetUserId();
                _checklistService.Delete(checklistId);

                return Ok();
            }
            catch(Exception ex) {
                _logger.LogError(ex, "Error deleting checklist {ChecklistId}.", checklistId);
                return StatusCode(500);
            }
        }
    }
}
