using Microsoft.AspNetCore.Mvc;
using Phorcys.Services;
using Phorcys.Web.Models;
using Phorcys.Data;
using Phorcys.Data.DTOs;
using Phorcys.Domain;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Phorcys2Web.Controllers;
using Microsoft.VisualBasic;
using Microsoft.Extensions.Logging;

namespace Phorcys.Web.Controllers
{
    public class LocationController : Controller
    {
        private readonly LocationServices _locationServices;
        private readonly UserServices _userServices;
        private readonly ILogger _logger;
		private const int SystemUserId = 6;

		public LocationController(LocationServices locationServices, UserServices userServices, 
            ILogger<LocationController> logger)
        {
            _locationServices = locationServices;
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

        [Authorize]
        public ActionResult Index()
        {
            try
            {
                /*var model = new List<LocationViewModel>
				{
					new LocationViewModel { Title = "Location 1", Notes = "Notes for location 1" },
					new LocationViewModel { Title = "Location 2", Notes = "Notes for location 2" },
					new LocationViewModel { Title = "Location 3", Notes = "Notes for location 3" }
				};
				*/
                var locations = _locationServices.GetLocations(_userServices.GetUserId());
                var model = CreateIndexModel(locations);

                return View(model);
            }
            catch (Exception ex)
            {
                // Handle or log the exception as appropriate
                return View("Error"); // Or another appropriate response
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult Create()
        {
            var model = new LocationViewModel();

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LocationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var location = new DiveLocation();
                location.Title = model.Title;
                location.Notes = model.Notes;
                location.Created = DateTime.Now;
                location.LastModified = DateTime.Now;
                int userId = _userServices.GetUserId();
                location.UserId = userId;
                _locationServices.SaveNewLocation(location);
                TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = "The Dive Location was successfully created.";
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            LocationViewModel model = new LocationViewModel();
            var location = _locationServices.GetLocation(Id);
            if (location.DiveLocationId != 0)
            {
                model.DiveLocationId = location.DiveLocationId;
                model.Title = location.Title;
                model.Notes = location.Notes;
                model.LastModified = DateTime.Now;
            }
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LocationViewModel model) 
        {
            var location = new LocationDto();
            location.DiveLocationId = model.DiveLocationId;
            location.Title = model.Title;
            location.Notes = model.Notes;

            _locationServices.EditLocation(location);

            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int DiveLocationId)
        {
            try
            {
                _locationServices.Delete(DiveLocationId);
                TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = "Location successfully deleted.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("Error"); // Or redirect to a different view as appropriate
            }
        }

        private List<LocationViewModel> CreateIndexModel(IEnumerable<Phorcys.Domain.DiveLocation> locations)
        {
            List<LocationViewModel> models = new List<LocationViewModel>();
            LocationViewModel model;
			var loggedInUser = _userServices.GetUserName();

			foreach (var location in locations)
            {
                model = new LocationViewModel();
                model.LoggedIn = loggedInUser;
                model.DiveLocationId = location.DiveLocationId;
                model.UserId = location.UserId;
				model.UserName = model.UserId == Phorcys.Data.Constants.SystemUserId ? "System" : _userServices.GetUserName();
				model.Title = location.Title;
                model.Created = location.Created;
                model.LastModified = location.LastModified;
                model.Notes = location.Notes;
                models.Add(model);
            }

            return models;
        }


    }
}
