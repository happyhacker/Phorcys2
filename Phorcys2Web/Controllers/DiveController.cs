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
//using Phorcys.Web.ModelsNew;

namespace Phorcys2Web.Controllers
{
	public class DiveController : Controller
	{
		private readonly DivePlanServices _divePlanServices;
		private readonly DiveServices _diveServices;
		private readonly UserServices _userServices;
		private readonly ILogger _logger;

		public DiveController(DivePlanServices divePlanServices, DiveServices diveServices, 
			UserServices userServices, ILogger<DiveController> logger)
		{
			_divePlanServices = divePlanServices;
			_diveServices = diveServices;
			_userServices = userServices;
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

		// GET: DiveController/Create
		[Authorize]
		[HttpGet]
		public ActionResult Create()
		{
			var model = new DiveViewModel();
			model.DivePlanList = BuildDivePlanList();

			return View(model);
		}

        [HttpGet]
        public IActionResult GetTanksForPlan(int divePlanId)
        {
            var tanks = _divePlanServices.GetTanksForDivePlan(divePlanId);
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
				_diveServices.SaveNewDive(dive);
				TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = "The Dive was successfully created.";
				return RedirectToAction("Index");
			}
			else
			{
				model.DivePlanList = BuildDivePlanList();
				return View(model);
			}
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
				model.Notes = dive.Notes;
			}
			return View(model);
		}

		// POST: DiveController/Edit/5
		[Authorize, HttpPost, ValidateAntiForgeryToken]
		public ActionResult Edit(DiveViewModel model)
		{
			if (ModelState.IsValid)
			{
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
				diveDto.Notes = model.Notes;
				try
				{
					_diveServices.Edit(diveDto);
					TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = "The Dive was successfully updated.";
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "There was an error editing a Dive");
					TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = "There was an error editing the Dive.";
				}
				return RedirectToAction("Index");

			}
			else
			{
				return View();
			}
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
				return View("Error");
			}
		}
		private List<DiveViewModel> CreateIndexModel(IEnumerable<Phorcys.Domain.Dive> dives)
		{
			List<DiveViewModel> models = new List<DiveViewModel>();
			DiveViewModel model;

			foreach (var dive in dives)
			{
				model = new DiveViewModel();
				model.DiveId = dive.DiveId;
				model.DiveNumber = dive.DiveNumber;
				if (dive.DivePlan != null)
				{
					model.DivePlanTitle = dive.DivePlan.Title;
					model.DiveSite = dive.DivePlan.DiveSite.Title;
				}
				model.Title = dive.Title;
				model.Minutes = dive.Minutes;
				model.MaxDepth = dive.MaxDepth;
				model.AvgDepth = dive.AvgDepth;
				model.AdditionalWeight = dive.AdditionalWeight;
				model.Created = dive.Created;
				model.LastModified = dive.LastModified;
				model.DescentTime = dive.DescentTime;
				model.Notes = dive.Notes;
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

	}

}
