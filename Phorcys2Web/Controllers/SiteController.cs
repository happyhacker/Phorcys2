using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using Microsoft.Identity.Client;
using Phorcys.Services;
using System.Globalization;
using Phorcys.Domain;
using Phorcys.Web.Models;
using System.Collections;
using Phorcys2Web.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Phorcys.Data.DTOs;
using System.Text;
using Azure.Identity;
using Microsoft.Extensions.Logging;

namespace Phorcys.Web.Controllers
{
	public class SiteController : Controller
	{
		private readonly DiveSiteServices _diveSiteServices;
		private readonly LocationServices _locationServices;
		private readonly UserServices _userServices;
		private readonly ILogger _logger;

		public SiteController(DiveSiteServices diveSiteServices, 
			LocationServices locationServices, UserServices userServices, ILogger<SiteController> logger)
		{
			_diveSiteServices = diveSiteServices;
			_locationServices = locationServices;
			_userServices = userServices;
			_logger = logger;	
		}

		[Authorize]
		public async Task<ActionResult> Index()
		{
			try
			{
				var sites = _diveSiteServices.GetDiveSites(_userServices.GetUserId()); // Await the completion of the async method
				var model = CreateIndexModel(sites);

				return View(model);
			}
			catch (Exception ex)
			{
				return View("Error");
			}
		}

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if (!string.IsNullOrEmpty(context.HttpContext.Request.Query["culture"]))
			{
				CultureInfo.DefaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(context.HttpContext.Request.Query["culture"]);
			}
			base.OnActionExecuting(context);
		}

		[Authorize]
		[HttpGet]
		public ActionResult Create()
		{
			var model = new SiteViewModel();
			model.LocationList = BuildLocationList();

			return View(model);
		}

		[Authorize, HttpPost, ValidateAntiForgeryToken]
		public ActionResult Create(SiteViewModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var siteDto = new SiteDto();
					siteDto.UserId = _userServices.GetUserId();
					siteDto.DiveLocationId = model.LocationSelectedId;
					siteDto.Title = model.Title;
					siteDto.MaxDepth = model.MaxDepth;
					siteDto.IsFreshWater = model.IsFreshWater;
					siteDto.GeoCode = model.GeoCode;
					siteDto.Latitude = model.Latitude;
					siteDto.Longitude = model.Longitude;
					siteDto.Notes = model.Notes;

					_diveSiteServices.Save(siteDto);
					TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = "Site successfully created.";
					return RedirectToAction("Index");
				}catch (Exception ex)
				{
					TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = "An error WTF-117 occured. Unable to add site.";
					return View();
				}
			}
			else
			{
				return View();
			}
		}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int DiveSiteId)
		{
			try
			{
				_diveSiteServices.Delete(DiveSiteId);
				TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = "Site successfully deleted.";
				return RedirectToAction("Index");
			}
			catch (DbUpdateException ex)
			{
				if (ex.InnerException != null)
				{
					TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] =
						"Unable to delete the Dive Site - a Dive Plan is using it.";
				}
				else
				{
					TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] =
						"An error occurred while updating the database.";
				}
			}
			catch (Exception ex)
			{
				TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] =
					"An unexpected error occurred.";
			}

			return RedirectToAction("Index");
		}

		[Authorize]
		[HttpGet]
		public ActionResult Edit(int Id)
		{
			var model = new SiteViewModel();

			var siteDto = _diveSiteServices.GetSiteDto(Id);

			model.DiveSiteId = siteDto.DiveSiteId;
			model.DiveLocationId = siteDto.DiveLocationId;
			model.LocationSelectedId = siteDto.LocationSelectedId;
			model.LocationList = BuildLocationList();
			model.Title = siteDto.Title;
			model.GeoCode = siteDto.GeoCode;
			model.Latitude = siteDto.Latitude;
			model.Longitude = siteDto.Longitude;
			model.IsFreshWater = siteDto.IsFreshWater;
			model.MaxDepth = siteDto.MaxDepth;
			model.Notes = siteDto.Notes;

			return View(model);
		}

		[Authorize, HttpPost, ValidateAntiForgeryToken]
		public ActionResult Edit(SiteViewModel model)
		{
			if (ModelState.IsValid)
			{
				var siteDto = new SiteDto();
				siteDto.DiveSiteId = model.DiveSiteId;
				siteDto.DiveLocationId = model.LocationSelectedId;
				siteDto.Title = model.Title;
				siteDto.MaxDepth = model.MaxDepth;
				siteDto.IsFreshWater = model.IsFreshWater;
				siteDto.GeoCode = model.GeoCode;
				siteDto.Latitude = model.Latitude;
				siteDto.Longitude = model.Longitude;
				siteDto.Notes = model.Notes;
				try
				{
					_diveSiteServices.Edit(siteDto);
					TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = "The Dive Site was successfully updated.";
				} catch(Exception ex)
				{
					Console.WriteLine(ex.Message);
					TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = "There was an error saving the Dive Site";
				}
				return RedirectToAction("Index");
			}
			else
			{
				return View();
			}
		}

		[Authorize]
		public IActionResult Map()
		{
            var sites = _diveSiteServices.GetDiveSites(_userServices.GetUserId()); // Await the completion of the async method
            var model = BuildMapList(sites);

			/*
            List<MapViewModel> model = new List<MapViewModel>();			MapViewModel mapModel = new MapViewModel();
			mapModel.Title = "Jackson Blue";
			mapModel.Notes = "Favorite place on earth";
			mapModel.Latitude = 30.790484m;
			mapModel.Longitude = -85.140075m;
			model.Add(mapModel);

            mapModel = new MapViewModel();
            mapModel.Title = "Jacob's Well";
			mapModel.Notes = "Restricted access. Permit required.";
            mapModel.Latitude = 30.034497m;
            mapModel.Longitude = -98.126066m;
            model.Add(mapModel);
			*/

            return View(model);
		}

		private IList<SelectListItem> BuildLocationList()
		{
			string loggedInId = _userServices.GetLoggedInUserId();
			IList<SelectListItem> locationList = new List<SelectListItem>();
			IEnumerable<DiveLocation> locations = _locationServices.GetLocations(_userServices.GetUserId());
			SelectListItem item;

			foreach (var location in locations)
			{
				item = new SelectListItem();
				item.Text = location.Title;
				item.Value = location.DiveLocationId.ToString();
				locationList.Add(item);
			}
			return locationList;
		}


		private List<MapViewModel> BuildMapList(IEnumerable<DiveSite> sites)
		{
			List<MapViewModel> mapViewModels = new List<MapViewModel>();
			MapViewModel model;

			foreach (var site in sites)
			{
				model = new MapViewModel();
				model.Title = site.Title;
				model.Notes = site.Notes;
				model.Latitude = site.Latitude;
				model.Longitude = site.Longitude;
				model.MaxDepth = site.MaxDepth;
				model.Water = site.IsFreshWater ? "Fresh" : "Salt";
				mapViewModels.Add(model);
			}
                return mapViewModels;
		}

		private List<SiteViewModel> CreateIndexModel(IEnumerable<DiveSite> sites)
		{
			List<SiteViewModel> siteViewModels = new List<SiteViewModel>();
			SiteViewModel model;
			var loggedInUser = _userServices.GetUserName();


			foreach (var site in sites)
			{
				model = new SiteViewModel();
				model.LoggedIn = loggedInUser;
				model.DiveSiteId = site.DiveSiteId;
				model.UserId = site.UserId;
				model.UserName = model.UserId == Phorcys.Data.Constants.SystemUserId ? "System" : _userServices.GetUserName();
				model.Title = site.Title;
				if (site.DiveLocation != null)
				{
					model.LocationTitle = site.DiveLocation.Title;
				}
				model.GeoCode = site.GeoCode;
				model.IsFreshWater = site.IsFreshWater;
				model.MaxDepth = site.MaxDepth;
				model.Created = site.Created;
				model.LastModified = site.LastModified;

				siteViewModels.Add(model);
			}
			return siteViewModels;
		}

		private string Url4Map(string geoCode)
		{
			var retVal = new StringBuilder("");

			if (geoCode != null && geoCode.Trim().Length > 0)
			{
				retVal.Append("<a href=\"http://maps.google.com/maps?q=");
				retVal.Append(geoCode.Trim());
				//arrow is centered
				retVal.Append("&ll=");
				retVal.Append(geoCode.Trim());
				//zoom level
				retVal.Append("&z=14");
				retVal.Append("\"");
				//retVal.Append(" target=\"_blank\" ");
				//retVal.Append(">Map</a>");
			}
			return retVal.ToString();
		}

	}
}
