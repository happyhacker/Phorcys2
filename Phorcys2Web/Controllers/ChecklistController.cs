using Microsoft.AspNetCore.Mvc;
using Phorcys.Services;
using Phorcys.Web.Models;
using System.Text.Json;
using Phorcys.Data.DTOs;

namespace Phorcys.Web.Controllers {
    public class ChecklistController : Controller {
        private readonly IChecklistServices _checklistService;
        private readonly UserServices _userServices;
        private readonly ILogger _logger;

        public ChecklistController(ILogger<ContactController> logger,
            UserServices userServices,IChecklistServices checklistService) {
            _checklistService = checklistService;
            _logger = logger;
            _userServices = userServices;
        }

        [HttpGet]
        public IActionResult Create() {
            return View(new ChecklistCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ChecklistCreateViewModel model) {
            if(!ModelState.IsValid)
                return View(model);

            List<ChecklistItemCreateDto>? dtoItems = null;

            if(!string.IsNullOrWhiteSpace(model.ItemsJson)) {
                try {
                    dtoItems = JsonSerializer.Deserialize<List<ChecklistItemCreateDto>>(model.ItemsJson);
                }
                catch {
                    ModelState.AddModelError("", "Unable to parse checklist items.");
                    return View(model);
                }
            }

            // map Web DTOs -> simple tuples for service
            var items = (dtoItems ?? new List<ChecklistItemCreateDto>())
                .Where(x => !string.IsNullOrWhiteSpace(x.Title))
                .Select(x => (x.Title, x.SequenceNumber));

            // TODO: replace with your actual UserId lookup from [Users] table
            int userId = _userServices.GetUserId();

            var checklistId = _checklistService.CreateChecklistWithItems(
                userId,
                model.Title,
                items);

            // later you might redirect to an Index or Edit page for that checklist
            return RedirectToAction("Index", "Home");
        }
    }
}
