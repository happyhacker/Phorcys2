using Microsoft.AspNetCore.Mvc;

namespace Phorcys.Web.Controllers
{
    public class EndController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
