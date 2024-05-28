using Microsoft.AspNetCore.Mvc;
using Phorcys.Services;
using Phorcys.Web.Models;

namespace Phorcys.Web.Controllers
{
	public class LocationController : Controller
	{
		public ActionResult Index()
		{
			try
			{
				var model = new List<LocationViewModel>
				{
					new LocationViewModel { Title = "Location 1", Notes = "Notes for location 1" },
					new LocationViewModel { Title = "Location 2", Notes = "Notes for location 2" },
					new LocationViewModel { Title = "Location 3", Notes = "Notes for location 3" }
				};
				return View(model);
			}
			catch (Exception ex)
			{
				// Handle or log the exception as appropriate
				return View("Error"); // Or another appropriate response
			}
		}


	}
}
