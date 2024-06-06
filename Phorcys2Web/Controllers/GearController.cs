using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Phorcys.Services;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Phorcys.Web.Models;
using Phorcys.Domain;
using Phorcys2Web.Controllers;

namespace Phorcys.Web.Controllers
{
	public class GearController : Controller
	{
		private readonly GearServices _gearServices;
		private readonly UserServices _userServices;

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if (!string.IsNullOrEmpty(context.HttpContext.Request.Query["culture"]))
			{
				CultureInfo.DefaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(context.HttpContext.Request.Query["culture"]);
			}
			base.OnActionExecuting(context);
		}

		public GearController(GearServices gearServices, UserServices userServices)
		{
			_gearServices = gearServices;
			_userServices = userServices;
		}
		[Authorize]
		public IActionResult Index()
		{
			try
			{
				var gearList = _gearServices.GetGear(_userServices.GetUserId());
				var model = CreateIndexModel(gearList);

				return View(model);
			}
			catch (Exception ex)
			{
				// Handle or log the exception as appropriate
				return View("Error"); // Or another appropriate response
			}


			return View();
		}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int GearId)
		{
			try
			{
				_gearServices.Delete(GearId);
				TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = "Gear successfully deleted.";
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				return View("Error");
			}
		}

		
		[HttpGet]
		public ActionResult Create()
		{
			var model = new GearViewModel();
			return View(model);
		}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(GearViewModel model)
		{
			if (ModelState.IsValid)
			{
				var gear = new Gear();
				gear.Title = model.Title;
				gear.Sn = model.Sn;
				gear.Acquired = model.Acquired;
				gear.RetailPrice = model.RetailPrice;
				gear.Paid = model.Paid;
				gear.Weight = model.Weight;
				gear.Notes = model.Notes;
				gear.Created = DateTime.Now;
				gear.LastModified = DateTime.Now;
				int userId = _userServices.GetUserId();
				gear.UserId = userId;
				_gearServices.SaveNewGear(gear);
				TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = "The Dive gear was successfully created.";
				return RedirectToAction("Index");
			}
			else
			{
				return View(model);
			}
		}

		private List<GearViewModel> CreateIndexModel(IEnumerable<Phorcys.Domain.Gear> gearList)
		{
			List<GearViewModel> models = new List<GearViewModel>();
			GearViewModel model;

			foreach (var gear in gearList)
			{
				model = new GearViewModel();
				model.GearId = gear.GearId;
				model.Title = gear.Title;
				model.RetailPrice = gear.RetailPrice;
				model.Paid = gear.Paid;
				model.Sn = gear.Sn;
				model.Acquired = gear.Acquired;
				model.NoLongerUse = gear.NoLongerUse;
				model.Weight = gear.Weight;
				model.Notes = gear.Notes;
				models.Add(model);
			}
			return models;
		}
	}
}
