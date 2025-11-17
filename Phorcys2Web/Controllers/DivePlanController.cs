using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
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
using Phorcys.Data.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace Phorcys2Web.Controllers
{
	public class DivePlanController : Controller
	{
		private readonly DiveTypeServices _diveTypeServices;
		private readonly DivePlanServices _divePlanServices;
		private readonly DiveSiteServices _diveSiteServices;
		private readonly UserServices _userServices;
		private readonly GearServices _gearServices;
		private readonly ILogger _logger;

		// Inject DivePlanServices into the controller
		public DivePlanController(DivePlanServices divePlanServices,
			DiveSiteServices diveSiteServices, UserServices userServices, 
			GearServices gearServices, DiveTypeServices diveTypeServices, 
			ILogger<DivePlanController> logger)
		{
			_diveTypeServices = diveTypeServices;
			_divePlanServices = divePlanServices;
			_diveSiteServices = diveSiteServices;
			_userServices = userServices;
			_gearServices = gearServices;
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
		public ActionResult Index()
		{
			try
			{
				var divePlans = _divePlanServices.GetDivePlans(_userServices.GetUserId()); // Await the completion of the async method
				var model = CreateIndexModel(divePlans);

				return View(model);
			}
			catch (Exception ex)
			{
				return View("Error"); // Or another appropriate response
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
			var model = new DivePlanViewModel();
			model.DiveSiteList = BuildDiveSiteList(null); //ToDo shouldn't this be userid?
			model.AvailableGear = BuildGearList(_userServices.GetUserId());
			model.AvailableDiveTypes = BuildDiveTypeList(_userServices.GetUserId());
			model.DiveBuddies = BuildDiveBuddiesList(_userServices.GetUserId());

			return View(model);
		}


		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(DivePlanViewModel model)
		{
			if (ModelState.IsValid)
			{
				var divePlan = new DivePlan();
				divePlan.Title = model.Title;
				divePlan.Minutes = model.Minutes;
				divePlan.Notes = model.Notes ?? "";
				divePlan.MaxDepth = model.MaxDepth;
				divePlan.ScheduledTime = model.ScheduledTime;
				divePlan.Created = DateTime.Now;
				divePlan.LastModified = DateTime.Now;
				int userId = _userServices.GetUserId();
				divePlan.UserId = userId;
				divePlan.DiveSiteId = model.DiveSiteSelectedId;
				_divePlanServices.SaveNewDivePlan(divePlan, model.SelectedGearIds, model.SelectedDiveTypeIds, model.SelectedDiverIds);
				TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = "The Dive Plan was successfully created.";
				return RedirectToAction("Index");
			}
			else
			{
				model.DiveSiteList = BuildDiveSiteList(null);
				return View(model);
			}
		}

		[Authorize]
		[HttpGet]
		public ActionResult Edit(int Id)
		{
			var divePlan = _divePlanServices.GetDivePlan(Id);
			DivePlanViewModel model = new DivePlanViewModel();
			model.DivePlanId = Id;
			model.Title = divePlan.Title;
			model.Minutes = divePlan.Minutes;
			model.Notes = divePlan.Notes;
			model.MaxDepth = divePlan.MaxDepth;
			model.UserId = divePlan.UserId;
			model.ScheduledTime = divePlan.ScheduledTime;
			model.DiveSiteId = divePlan.DiveSiteId;
			model.DiveSiteList = BuildDiveSiteList(divePlan.DiveSiteId);
			model.SelectedDiveTypeIds = divePlan.DiveTypes.Select(t  => t.DiveTypeId).ToList();
			model.AvailableDiveTypes = BuildDiveTypeList(Id);
			model.SelectedGearIds = divePlan.Gears.Select(g => g.GearId).ToList();
			model.AvailableGear = BuildGearList(divePlan.UserId);
            model.SelectedDiverIds= divePlan.DiveTeams.Select(db => db.DiverId).ToList();
            model.DiveBuddies = BuildDiveBuddiesList(divePlan.UserId);

            return View(model);
		}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(DivePlanViewModel model)
		{
			if (ModelState.IsValid)
			{
				var divePlanDto = new DivePlanDto();
				divePlanDto.DivePlanId = model.DivePlanId;
				divePlanDto.Title = model.Title;
				divePlanDto.Minutes = model.Minutes;
				divePlanDto.Notes = model.Notes ?? "";
				divePlanDto.MaxDepth = model.MaxDepth;
				divePlanDto.ScheduledTime = model.ScheduledTime;
				divePlanDto.DiveSiteId = model.DiveSiteSelectedId;
				divePlanDto.SelectedGearIds = model.SelectedGearIds ?? new List<int>();
				divePlanDto.SelectedDiveTypeIds = model.SelectedDiveTypeIds ?? new List<int>();
                divePlanDto.SelectedDiverIds = model.SelectedDiverIds ?? new List<int>();

                _divePlanServices.EditDivePlan(divePlanDto);
				TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = "The Dive Plan was successfully updated.";
				return RedirectToAction("Index");
			}
			else
			{
				model.DiveSiteList = BuildDiveSiteList(model.DiveSiteSelectedId);
				model.AvailableGear = BuildGearList(model.UserId);

				return View(model);
			}

		}

		// POST: DiveController/Delete/5
		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int DivePlanId)
		{
			try
			{
				_divePlanServices.Delete(DivePlanId);
				TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = "Dive Plan successfully deleted.";
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
                TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = "Dive Plan can not be deleted because a dive is tied to it.";
                return RedirectToAction("Index");
            }
		}

		private List<DivePlanIndexViewModel> CreateIndexModel(IEnumerable<Phorcys.Domain.DivePlan> divePlans)
		{
			List<DivePlanIndexViewModel> models = new List<DivePlanIndexViewModel>();
			DivePlanIndexViewModel model;

			foreach (var divePlan in divePlans)
			{
				model = new DivePlanIndexViewModel();
				model.DivePlanId = divePlan.DivePlanId;
				if (divePlan.DiveSite != null)
				{
					model.DiveSite = divePlan.DiveSite.Title;
				}
				model.Title = divePlan.Title;
				model.Minutes = divePlan.Minutes;
				model.MaxDepth = divePlan.MaxDepth;
				model.ScheduledTime = divePlan.ScheduledTime;
				model.Notes = divePlan.Notes;
				models.Add(model);
			}
			return models;
		}

		private IList<SelectListItem> BuildDiveSiteList(int? diveSiteId)
		{
			IList<SelectListItem> diveSiteList = new List<SelectListItem>();
			IEnumerable<DiveSite> diveSites = _diveSiteServices.GetDiveSites(_userServices.GetUserId());
			SelectListItem item;

			foreach (var diveSite in diveSites)
			{
				item = new SelectListItem();
				item.Text = diveSite.Title;
				item.Value = diveSite.DiveSiteId.ToString();
				if (diveSiteId != null)
				{
					if (diveSite.DiveSiteId == diveSiteId)
					{
						item.Selected = true;
					}
				}

				diveSiteList.Add(item);
			}
			return diveSiteList;
		}

		private List<SelectListItem> BuildDiveTypeList(int userId)
		{
			var diveTypeDtos = _diveTypeServices.GetDiveTypeDtos(userId) ?? new List<DiveTypeDto>();
			var selectListItems = new List<SelectListItem>();

			foreach (var diveType in diveTypeDtos)
			{
					selectListItems.Add(new SelectListItem
					{
						Text = diveType.Title,
						Value = diveType.DiveTypeId.ToString()
					});
			}
			return selectListItems;
		}

		private List<SelectListItem> BuildGearList(int userId)
		{
			var gearDtos = _gearServices.GetGearTitles(userId) ?? new List<GearDto>();
			var selectListItems = new List<SelectListItem>();

			foreach (var gear in gearDtos)
			{
				if (gear.NoLongerUse == null) // Include only if not marked as no longer used
				{
					selectListItems.Add(new SelectListItem
					{
						Text = gear.Title,
						Value = gear.GearId.ToString()
					});
				}
			}
			return selectListItems;
		}

		private List<SelectListItem> BuildDiveBuddiesList(int userId)
		{
			var diveBuddies = _divePlanServices.GetDiveBuddies(userId) ?? new List<Contact>();
			var selectListItems = new List<SelectListItem>();

			foreach (var buddy in diveBuddies)
			{
				selectListItems.Add(new SelectListItem
				{
					Text = $"{buddy.FirstName} {buddy.LastName}",
					Value = buddy.Diver.DiverId.ToString()
				});
			}
			return selectListItems;
		}
	}
}

