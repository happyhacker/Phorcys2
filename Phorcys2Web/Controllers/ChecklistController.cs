using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Phorcys.Data.DTOs;
using Phorcys.Services;
using Phorcys.Web.Models;
using Phorcys2Web.Controllers;
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
        public IActionResult Create() {
            return View(new ChecklistCreateViewModel());
        }

        [Authorize, HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(ChecklistCreateViewModel model) {
            if(!ModelState.IsValid) {
                return View(model);
            }

            // 1. Deserialize the items JSON coming from the grid
            List<ChecklistItemCreateDto>? dtoItems = null;

            if(!string.IsNullOrWhiteSpace(model.ItemsJson)) {
                try {
                    dtoItems = JsonSerializer.Deserialize<List<ChecklistItemCreateDto>>(model.ItemsJson);
                }
                catch(Exception ex) {
                    _logger.LogError(ex,
                        "Unable to parse checklist items JSON for checklist '{Title}'. Raw JSON: {ItemsJson}",
                        model.Title,
                        model.ItemsJson);

                    ModelState.AddModelError(string.Empty, "Unable to parse checklist items.");
                    return View(model);
                }
            }

            // 2. Map Web DTOs -> simple tuples for the service
            var items = (dtoItems ?? new List<ChecklistItemCreateDto>())
                .Where(x => !string.IsNullOrWhiteSpace(x.Title))
                .Select(x => (x.Title, x.SequenceNumber));

            try {
                // 3. Get the current user id (from your Users table)
                int userId = _userServices.GetUserId();

                // 4. Call the service – it handles transaction + logging
                var checklistId = _checklistService.CreateChecklistWithItems(
                    userId,
                    model.Title,
                    items);

                TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] =
                    "The Checklist was successfully added.";

                // Later you might redirect to a Checklist index or detail page
                return RedirectToAction("Index", "Home");
            }
            catch(Exception ex) {
                // High-level catch: log and show friendly message
                _logger.LogError(ex,
                    "Error creating checklist '{Title}' for current user. Model: {@Model}",
                    model.Title,
                    model);

                ModelState.AddModelError(string.Empty,
                    "There was a problem saving the checklist. Please try again. If the problem persists, contact support.");
                TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] =
                    "There was a problem saving the checklist. Please try again. If the problem persists, contact support.";

                // Re-render the view with the user's input and validation errors
                return View(model);
            }
        }
    }
}
