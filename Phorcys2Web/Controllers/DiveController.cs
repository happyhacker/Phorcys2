using Microsoft.AspNetCore.Mvc;
using Phorcys.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Globalization;
using Phorcys.Services;
using Phorcys.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.AspNetCore.Authorization;
using Phorcys.Data.DTOs;
using Microsoft.Extensions.Logging;
using Kendo.Mvc.UI;
//using Phorcys.Web.ModelsNew;

namespace Phorcys2Web.Controllers
{
	public class DiveController : Controller
	{
		private readonly DivePlanServices _divePlanServices;
		private readonly DiveServices _diveServices;
		private readonly UserServices _userServices;
		private readonly GearServices _gearServices;
		private readonly IShearwaterCsvImportService _shearwaterCsvImportService;
		private readonly ILogger _logger;

		public DiveController(DivePlanServices divePlanServices, DiveServices diveServices,
			UserServices userServices, GearServices gearServices,
			IShearwaterCsvImportService shearwaterCsvImportService,
			ILogger<DiveController> logger)
		{
			_divePlanServices = divePlanServices;
			_diveServices = diveServices;
			_userServices = userServices;
			_gearServices = gearServices;
			_shearwaterCsvImportService = shearwaterCsvImportService;
			_logger = logger;
		}

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if (!string.IsNullOrEmpty(context.HttpContext.Request.Query["culture"]))
			{
				CultureInfo.DefaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(context.HttpContext.Request.Query["culture"]);
			}
			base.OnActionExecuting(context);
		}


		// GET: DiveController
		[Authorize]
		public async Task<ActionResult> Index()
		{
			try
			{
				var dives = await _diveServices.GetDivesAsync(_userServices.GetUserId()); // Await the completion of the async method
				var model = CreateIndexModel(dives); // Now dives is IEnumerable<Dive>

				return View(model);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				return View("Error");
			}
		}


		// GET: DiveController/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		[Authorize, HttpGet]
		public ActionResult Compare(int id)
		{
			var dive = _diveServices.GetDiveWithPlan(id);
			if (dive == null)
				return View("Error");

			var plan = dive.DivePlan;
			bool hasPlan = plan != null;

			ViewBag.DiveNumber = dive.DiveNumber;
			ViewBag.HasPlan = hasPlan;

			var rows = new List<DiveCompareViewModel>
			{
				new DiveCompareViewModel {
					Field = "Title",
					Planned = plan?.Title ?? "",
					Actual = dive.Title ?? "",
					HasPlanValue = hasPlan,
					IsMatch = hasPlan && string.Equals(plan.Title, dive.Title, StringComparison.OrdinalIgnoreCase)
				},
				new DiveCompareViewModel {
					Field = "Dive Site",
					Planned = plan?.DiveSite?.Title ?? "",
					Actual = plan?.DiveSite?.Title ?? "", // Assuming DiveSite is same as the plan
                    HasPlanValue = hasPlan,
					IsMatch = true // We assume the dive site matches the plan's dive site
				},
				new DiveCompareViewModel {
					Field = "Date / Time",
					Planned = plan?.ScheduledTime.ToString("MM/dd/yyyy hh:mm tt") ?? "",
					Actual = dive.DescentTime?.ToString("MM/dd/yyyy hh:mm tt") ?? "",
					HasPlanValue = hasPlan,
					IsMatch = hasPlan && plan.ScheduledTime.Date == dive.DescentTime?.Date
				},
				new DiveCompareViewModel {
					Field = "Duration (min)",
					Planned = plan?.Minutes?.ToString() ?? "",
					Actual = dive.Minutes?.ToString() ?? "",
					HasPlanValue = hasPlan,
					IsMatch = hasPlan && plan.Minutes == dive.Minutes
				},
				new DiveCompareViewModel {
					Field = "Max Depth",
					Planned = plan?.MaxDepth?.ToString() ?? "",
					Actual = dive.MaxDepth?.ToString() ?? "",
					HasPlanValue = hasPlan,
					IsMatch = hasPlan && plan.MaxDepth == dive.MaxDepth
				},
				new DiveCompareViewModel {
					Field = "Notes",
					Planned = plan?.Notes ?? "",
					Actual = dive.Notes ?? "",
					HasPlanValue = hasPlan,
					IsMatch = hasPlan && string.Equals(plan.Notes, dive.Notes, StringComparison.OrdinalIgnoreCase)
				}
			};

			return View(rows);
		}

		// GET: DiveController/Create
		[Authorize]
		[HttpGet]
		public ActionResult Create()
		{
			var model = new DiveViewModel();
			model.DivePlanList = BuildDivePlanList();

			return View(model);
		}

		[Authorize]
		[HttpGet]
		public IActionResult GetTanksForPlan(int divePlanId)
		{
			var tanks = _divePlanServices.GetTanksForDivePlan(divePlanId) ?? new List<TanksOnDiveDto>();
			return Json(tanks);
		}

		[Authorize, HttpPost, ValidateAntiForgeryToken]
		public ActionResult Create(DiveViewModel model)
		{
			if (ModelState.IsValid)
			{
				Dive dive = new Dive();
				dive.DiveNumber = model.DiveNumber;
				dive.Title = model.Title;
				dive.Minutes = model.Minutes;
				dive.Notes = model.Notes ?? "";
				dive.MaxDepth = model.MaxDepth;
				dive.AvgDepth = model.AvgDepth;
				dive.Temperature = model.Temperature;
				dive.AdditionalWeight = model.AdditionalWeight;
				dive.DescentTime = model.DescentTime;
				dive.Created = DateTime.Now;
				dive.LastModified = DateTime.Now;
				dive.UserId = _userServices.GetUserId();
				dive.DivePlanId = model.DivePlanSelectedId;
				_diveServices.SaveNewDive(dive, model.Tanks);

				// If a Shearwater CSV was imported, create the DiveComputerLog record
				if (model.HasImportedData)
				{
					try
					{
						int userId = _userServices.GetUserId();
						int? matchedGearId = _gearServices.FindGearIdBySerialNumber(userId, model.ImportedSerialNumber);

						var log = new Phorcys.Domain.DiveComputerLog
						{
							DiveId = dive.DiveId,
							Vendor = "Shearwater",
							Product = model.ImportedProduct,
							// Model is the same as Product for Shearwater imports
							Model = model.ImportedProduct,
							SerialNumber = model.ImportedSerialNumber,
							FirmwareVersion = model.ImportedFirmwareVersion,
							DiveNumber = model.DiveNumber,
							CnsBeforePercent = model.ImportedCnsBeforePercent,
							CnsAfterPercent = model.ImportedCnsAfterPercent,
							BatteryVoltage = model.ImportedBatteryVoltage,
							DiveMode = model.ImportedDiveMode,
							IsImperial = model.ImportedIsImperial,
							Descended = model.ImportedDescended,
							Surfaced = model.ImportedSurfaced,
							MaxDepth = model.MaxDepth,
							Minutes = model.Minutes,
							ImportedDateTime = DateTime.Now,
							// GearId is [NotMapped] — stored in-memory for reference but not persisted.
							// To persist the match, add a GearId column to DiveComputerLogs.
							GearId = matchedGearId
						};
						_diveServices.SaveDiveComputerLog(log);
					}
					catch (Exception ex)
					{
						// Log but don't fail the dive save — the dive was already created
						_logger.LogError(ex, "Failed to save DiveComputerLog for dive {DiveId}", dive.DiveId);
					}
				}

				TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = "The Dive was successfully created.";
				return RedirectToAction("Index");
			}
			else
			{
				model.DivePlanList = BuildDivePlanList();
				return View(model);
			}
		}

		/// <summary>
		/// Accepts a Shearwater Cloud CSV file, parses the summary row, and re-displays
		/// the Create Dive Log form with the imported values pre-filled.
		/// The user can review and edit all values before saving.
		/// </summary>
		[Authorize, HttpPost, ValidateAntiForgeryToken]
		public async Task<ActionResult> UploadShearwaterCsv(IFormFile csvFile, int? divePlanSelectedId)
		{
			var model = new DiveViewModel();
			model.DivePlanList = BuildDivePlanList();
			model.DivePlanSelectedId = divePlanSelectedId > 0 ? divePlanSelectedId : null;

			if (csvFile == null || csvFile.Length == 0)
			{
				ModelState.AddModelError("", "Please select a Shearwater CSV file to upload.");
				return View("Create", model);
			}

			if (!csvFile.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
			{
				ModelState.AddModelError("", "The uploaded file must be a .csv file exported from Shearwater Cloud.");
				return View("Create", model);
			}

			using var stream = csvFile.OpenReadStream();
			var summary = await _shearwaterCsvImportService.ParseAsync(stream);

			if (summary == null)
			{
				ModelState.AddModelError("", "The file could not be parsed. Please verify it is a valid Shearwater Cloud CSV export.");
				return View("Create", model);
			}

			// Prefill form fields from imported summary
			if (summary.DiveNumber.HasValue)
				model.DiveNumber = summary.DiveNumber.Value;

			if (summary.Descended.HasValue)
				model.DescentTime = summary.Descended.Value;

			if (summary.DurationMinutes.HasValue)
				model.Minutes = summary.DurationMinutes.Value;

			if (summary.MaxDepth.HasValue)
				model.MaxDepth = summary.MaxDepth.Value;

			// Store import metadata for DiveComputerLog creation on save
			model.HasImportedData = true;
			model.ImportedSerialNumber = summary.SerialNumber;
			model.ImportedProduct = summary.Product;
			model.ImportedFirmwareVersion = summary.FirmwareVersion;
			model.ImportedCnsBeforePercent = summary.CnsBeforePercent;
			model.ImportedCnsAfterPercent = summary.CnsAfterPercent;
			model.ImportedBatteryVoltage = summary.BatteryVoltage;
			model.ImportedDiveMode = summary.DiveMode;
			model.ImportedIsImperial = summary.IsImperial;
			model.ImportedDescended = summary.Descended;
			model.ImportedSurfaced = summary.Surfaced;

			_logger.LogInformation("Shearwater CSV imported: DiveNumber={DiveNumber}, Start={Start}, Duration={Mins}min, MaxDepth={Depth}",
				summary.DiveNumber, summary.Descended, summary.DurationMinutes, summary.MaxDepth);

			return View("Create", model);
		}


		[Authorize, HttpGet]
		public ActionResult Edit(int id)
		{
			var model = new DiveViewModel();
			var dive = _diveServices.GetDive(id);
			if (dive != null)
			{
				model.DiveId = dive.DiveId;
				model.DivePlanSelectedId = dive.DivePlanId;
				model.DiveNumber = dive.DiveNumber;
				model.DivePlanList = BuildDivePlanList();
				model.Title = dive.Title;
				model.MaxDepth = dive.MaxDepth;
				model.AvgDepth = dive.AvgDepth;
				model.DescentTime = dive.DescentTime;
				model.AdditionalWeight = dive.AdditionalWeight;
				model.Minutes = dive.Minutes;
				model.Temperature = dive.Temperature;
				model.Notes = dive.Notes ?? "";
			}
			return View(model);
		}

		// POST: DiveController/Edit/5
		[Authorize, HttpPost, ValidateAntiForgeryToken]
		public ActionResult Edit(DiveViewModel model)
		{
			if (!ModelState.IsValid)
			{
				model.DivePlanList = BuildDivePlanList();  // rebuild dropdown
														   // Keep model.Tanks as posted so the view doesn’t lose user edits
				return View(model);
			}

			var diveDto = new DiveDto();
			diveDto.DiveId = model.DiveId;
			diveDto.DivePlanId = model.DivePlanSelectedId;
			diveDto.DiveNumber = model.DiveNumber;
			diveDto.Title = model.Title;
			diveDto.Minutes = model.Minutes;
			diveDto.DescentTime = model.DescentTime;
			diveDto.AvgDepth = model.AvgDepth;
			diveDto.MaxDepth = model.MaxDepth;
			diveDto.Temperature = model.Temperature;
			diveDto.AdditionalWeight = model.AdditionalWeight;
			diveDto.Notes = model.Notes ?? "";
			try
			{
				_diveServices.Edit(diveDto);
				if (model.DivePlanSelectedId.HasValue && model.Tanks != null && model.Tanks.Count > 0)
				{
					// Choose ONE of these patterns depending on how your services are organized:

					// A) If this lives in DivePlanServices:
					_divePlanServices.UpsertTanksOnDive(model.DivePlanSelectedId.Value, model.Tanks);

					// OR B) If you prefer to keep it in DiveServices:
					// _diveServices.SaveTanksOnDive(model.DivePlanSelectedId.Value, model.Tanks);
				}

				TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = "The Dive was successfully updated.";
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "There was an error editing a Dive");
				TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = "There was an error editing the Dive.";
			}
			return RedirectToAction("Index");

		}


		// POST: DiveController/Delete/5
		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int diveId)
		{
			try
			{
				_diveServices.Delete(diveId);
				TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = "Dive successfully deleted.";
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error deleting Dive {diveId}", diveId);
				TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = "Unable to delete Dive. ";
				return View("Error");
			}
		}
        private List<DiveViewModel> CreateIndexModel(IEnumerable<Phorcys.Domain.Dive> dives) {
            var models = new List<DiveViewModel>();

            foreach(var dive in dives) {
                var model = new DiveViewModel {
                    DiveId = dive.DiveId,
                    DiveNumber = dive.DiveNumber,
                    Title = dive.Title,
                    Minutes = dive.Minutes,
                    MaxDepth = dive.MaxDepth,
                    AvgDepth = dive.AvgDepth,
                    AdditionalWeight = dive.AdditionalWeight,
                    Created = dive.Created,
                    LastModified = dive.LastModified,
                    DescentTime = dive.DescentTime,
                    Notes = dive.Notes
                };

                if(dive.DivePlan != null) {
                    model.DivePlanTitle = dive.DivePlan.Title;

                    if(dive.DivePlan.DiveSite != null)
                        model.DiveSite = dive.DivePlan.DiveSite.Title;

                    var buddyNames = dive.DivePlan.DiveTeams?
                        .Select(dt => {
                            var c = dt.Diver?.Contact;
                            var first = (c?.FirstName ?? "").Trim();
                            var last = (c?.LastName ?? "").Trim();
                            return $"{first} {last}".Trim();
                        })
                        .Where(n => !string.IsNullOrWhiteSpace(n))
                        .Distinct()
                        .OrderBy(n => n)
                        .ToList();

                    if(buddyNames != null && buddyNames.Count > 0)
                        model.DiveBuddies = string.Join(":", buddyNames);
                }
                models.Add(model);
            }

            return models;
        }

        private IList<SelectListItem> BuildDivePlanList()
		{
			IList<SelectListItem> divePlanList = new List<SelectListItem>();
			string loggedInId = _userServices.GetLoggedInUserId();
			IEnumerable<DivePlan> divePlans = _divePlanServices.GetDivePlans(_userServices.GetUserId());
			SelectListItem item;

			foreach (var divePlan in divePlans)
			{
				item = new SelectListItem();
				item.Text = divePlan.Title;
				item.Value = divePlan.DivePlanId.ToString();
				divePlanList.Add(item);
			}
			return divePlanList;
		}

		[Authorize]
		[HttpGet]
		public IActionResult GetPlanDefaults(int divePlanId)
		{
			var plan = _divePlanServices.GetDivePlan(divePlanId);
			if (plan == null) return NotFound();

			var dto = new
			{
				Title = plan.Title,
				DescentTime = plan.ScheduledTime.ToString("yyyy-MM-ddTHH:mm"),
				Minutes = plan.Minutes,
				MaxDepth = plan.MaxDepth
			};

			return Json(dto);
		}


	}

}
