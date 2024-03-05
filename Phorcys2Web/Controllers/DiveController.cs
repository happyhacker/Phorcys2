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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace Phorcys2Web.Controllers
{
	public class DiveController : Controller
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if (!string.IsNullOrEmpty(context.HttpContext.Request.Query["culture"]))
			{
				CultureInfo.DefaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(context.HttpContext.Request.Query["culture"]);
			}
			base.OnActionExecuting(context);
		}

		private DiveServices diveServices = new DiveServices();
		private DivePlanServices divePlanServices = new DivePlanServices();

		// GET: DiveController
		public async Task<ActionResult> Index()
		{
			try
			{
				var dives = await diveServices.GetDivesAsync(); // Await the completion of the async method
				var model = CreateIndexModel(dives); // Now dives is IEnumerable<Dive>

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
		[HttpGet]
		public ActionResult Create()
		{
			var model = new DiveViewModel();
			model.DivePlanList = BuildDivePlanList();

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(DiveViewModel model)
		{
			if (ModelState.IsValid)
			{
				Dive dive = new Dive();
				dive.DiveNumber = model.DiveNumber;
				dive.Title = model.Title;
				dive.Minutes = model.Minutes;
				dive.Notes = " " + model.Notes;
				dive.MaxDepth = model.MaxDepth;
				dive.AvgDepth = model.AvgDepth;
				dive.Temperature = model.Temperature;
				dive.AdditionalWeight = model.AdditionalWeight;
				dive.DescentTime = model.DescentTime;
				dive.Created = DateTime.Now;
				dive.LastModified = DateTime.Now;
				dive.UserId = 3; //Hardcoded for now
				dive.DivePlanId = model.DivePlanSelectedId;
				diveServices.SaveNewDive(dive);
				TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = "The Dive was successfully created.";
				return RedirectToAction("Index");			
			}
			else
			{
				model.DivePlanList = BuildDivePlanList();
				return View(model);
			}
		}


		// GET: DiveController/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: DiveController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// POST: DiveController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int DiveId)
		{
			try
			{
				diveServices.Delete(DiveId);
				TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = "Dive successfully deleted.";
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				return View("Error"); // Or redirect to a different view as appropriate
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
				//model.UserName = dive.User.
				models.Add(model);

			}

			return models;
		}

		private IList<SelectListItem> BuildDivePlanList()
		{
			IList<SelectListItem> divePlanList = new List<SelectListItem>();
			IEnumerable<DivePlan> divePlans = divePlanServices.GetDivePlans();
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

