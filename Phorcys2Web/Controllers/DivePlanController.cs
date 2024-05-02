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
using Microsoft.Extensions.Options;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Phorcys.Data;
using Microsoft.AspNetCore.Authorization;

namespace Phorcys2Web.Controllers
{
	public class DivePlanController : Controller
	{
		private readonly DivePlanServices _divePlanServices;
		private readonly DiveSiteServices _diveSiteServices;

		// Inject DivePlanServices into the controller
		public DivePlanController(DivePlanServices divePlanServices, DiveSiteServices diveSiteServices)
		{
			_divePlanServices = divePlanServices;
			_diveSiteServices = diveSiteServices;
		}
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if (!string.IsNullOrEmpty(context.HttpContext.Request.Query["culture"]))
			{
				CultureInfo.DefaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(context.HttpContext.Request.Query["culture"]);
			}
			base.OnActionExecuting(context);
		}

		//private DivePlanServices divePlanServices = new DivePlanServices();
		//private DiveSiteServices diveSiteServices = new DiveSiteServices();

		// GET: DiveController
		[Authorize]
		public ActionResult Index()
		{
			try
			{
				var divePlans = _divePlanServices.GetDivePlans(); // Await the completion of the async method
				var model = CreateIndexModel(divePlans); 

				return View(model);
			}
			catch (Exception ex)
			{
				// Handle or log the exception as appropriate
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
			model.DiveSiteList = BuildDiveSiteList(null);

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
				divePlan.Notes = " " + model.Notes;
				divePlan.MaxDepth = model.MaxDepth;
				divePlan.ScheduledTime = model.ScheduledTime;
				divePlan.Created = DateTime.Now;
				divePlan.LastModified = DateTime.Now;
				divePlan.UserId = 3; //Hardcoded for now
				divePlan.DiveSiteId = model.DiveSiteSelectedId;
				_divePlanServices.SaveNewDivePlan(divePlan);
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
			DivePlan divePlan = _divePlanServices.GetDivePlan(Id);
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
				divePlanDto.Notes = " " + model.Notes;
				divePlanDto.MaxDepth = model.MaxDepth;
				divePlanDto.ScheduledTime = model.ScheduledTime;
				divePlanDto.UserId = 3; //Hardcoded for now
				divePlanDto.DiveSiteId = model.DiveSiteSelectedId;
				_divePlanServices.EditDivePlan(divePlanDto);
				TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = "The Dive Plan was successfully updated.";
				return RedirectToAction("Index");
			}
			else
			{
				model.DiveSiteList = BuildDiveSiteList(model.DiveSiteSelectedId);
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
				return View("Error"); // Or redirect to a different view as appropriate
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
				//model.UserName = dive.User.
				models.Add(model);

			}

			return models;
		}

		private IList<SelectListItem> BuildDiveSiteList(int? diveSiteId)
		{
			IList<SelectListItem> diveSiteList = new List<SelectListItem>();
			IEnumerable<DiveSite> diveSites = _diveSiteServices.GetDiveSites();
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

	}

}

