using Microsoft.AspNetCore.Mvc;

namespace Phorcys.Web.Controllers
{
	public class ModEndController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
