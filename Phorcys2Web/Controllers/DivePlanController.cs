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
	public class DivePlanController : Controller
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if (!string.IsNullOrEmpty(context.HttpContext.Request.Query["culture"]))
			{
				CultureInfo.DefaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(context.HttpContext.Request.Query["culture"]);
			}
			base.OnActionExecuting(context);
		}

		private DivePlanServices divePlanServices = new DivePlanServices();

		// GET: DiveController
		public ActionResult Index()
		{
			try
			{
				var divePlans = divePlanServices.GetDivePlans(); // Await the completion of the async method
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
		[HttpGet]
		public ActionResult Create()
		{
			var model = new DiveViewModel();
			model.DivePlanList = BuildDivePlanList();

			return View(model);
		}


		// POST: DiveController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int DivePlanId)
		{
			try
			{
				divePlanServices.Delete(DivePlanId);
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

