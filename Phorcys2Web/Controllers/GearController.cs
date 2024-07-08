using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Phorcys.Services;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Phorcys.Web.Models;
using Phorcys.Domain;
using Phorcys2Web.Controllers;
using Phorcys.Data.DTOs;
using Microsoft.Extensions.Logging;

namespace Phorcys.Web.Controllers
{
	public class GearController : Controller
	{
		private readonly GearServices _gearServices;
		private readonly UserServices _userServices;
		private readonly ILogger _logger;

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if (!string.IsNullOrEmpty(context.HttpContext.Request.Query["culture"]))
			{
				CultureInfo.DefaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(context.HttpContext.Request.Query["culture"]);
			}
			base.OnActionExecuting(context);
		}

		public GearController(GearServices gearServices, UserServices userServices, ILogger<GearController> logger)
		{
			_gearServices = gearServices;
			_userServices = userServices;
			_logger = logger;
		}
		[Authorize]
		public IActionResult Index()
		{
			try
			{
				var gearList = _gearServices.GetGearList(_userServices.GetUserId());
				var model = CreateIndexModel(gearList);

				return View(model);
			}
			catch (Exception ex)
			{
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

		[Authorize]
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

				if (model.WorkingPressure > 0 || model.TankVolume > 0 || model.ManufacturedMonth > 0 || model.ManufacturedYear > 0)
				{
					Tank tank = new Tank();
					tank.Volume = model.TankVolume;
					tank.WorkingPressure = model.WorkingPressure;
					tank.ManufacturedMonth = model.ManufacturedMonth;
					tank.ManufacturedYear = model.ManufacturedYear;
					tank.GearId = gear.GearId;
					gear.Tank = tank;
					tank.Gear = gear;
				}
				_gearServices.SaveNewGear(gear);
				TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = "The Dive gear was successfully created.";
				return RedirectToAction("Index");
			}
			else
			{
				return View(model);
			}
		}

		[Authorize]
		[HttpGet]
		public ActionResult Edit(int id) 
			{
			var model = new GearViewModel();
			var gearDto = _gearServices.GetGear(id);
			if (gearDto != null)
			{
				model.GearId = gearDto.GearId;
				model.Title = gearDto.Title;
				model.Acquired = gearDto.Acquired;
				model.RetailPrice = gearDto.RetailPrice;
				model.Paid = gearDto.Paid;
				model.Sn = gearDto.Sn;
				model.NoLongerUse = gearDto.NoLongerUse;
				model.Weight = gearDto.Weight;
				model.Notes = gearDto.Notes;
				model.TankVolume = gearDto.Volume;
				model.WorkingPressure = gearDto.WorkingPressure;
				model.ManufacturedMonth = gearDto.ManufacturedMonth;
				model.ManufacturedYear = gearDto.ManufacturedYear;
			}
			return View(model); 
		}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(GearViewModel model)
		{
			if (ModelState.IsValid)
			{
				var gearDto = new GearDto();

				gearDto.GearId = model.GearId;
				gearDto.Title = model.Title;
				gearDto.Acquired = model.Acquired;
				gearDto.RetailPrice = model.RetailPrice;
				gearDto.Paid = model.Paid;
				gearDto.Sn = model.Sn;
				gearDto.NoLongerUse = model.NoLongerUse;
				gearDto.Weight = model.Weight;
				gearDto.Notes = model.Notes;
				gearDto.Volume = model.TankVolume;
				gearDto.WorkingPressure = model.WorkingPressure;
				gearDto.ManufacturedMonth = model.ManufacturedMonth;
				gearDto.ManufacturedYear = model.ManufacturedYear;
						
				_gearServices.EditGear(gearDto);
				return RedirectToAction("Index");
			}
			else { return View(model); }
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
