using Microsoft.AspNetCore.Mvc;
using Phorcys.Services;
using Phorcys.Web.Models;
using Phorcys.Data;
using Phorcys.Domain;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;

namespace Phorcys.Web.Controllers
{
	public class LocationController : Controller
	{
		private readonly LocationServices _locationServices;
		private readonly UserServices _userServices;

		public LocationController(LocationServices locationServices, UserServices userServices)
		{
			_locationServices = locationServices;
			_userServices = userServices;
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

		private List<LocationViewModel> CreateIndexModel(IEnumerable<Phorcys.Domain.DiveLocation> locations)
		{
			List<LocationViewModel> models = new List<LocationViewModel>();
			LocationViewModel model;

			foreach (var location in locations)
			{
				model = new LocationViewModel();
				model.DiveLocationId = location.DiveLocationId;
				model.Title = location.Title;
				model.Notes = location.Notes;
				//model.UserName = dive.User.
				models.Add(model);

			}

			return models;
		}


	}
}
