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
using Phorcys.Web.ModelsNew;

namespace Phorcys.Web.Controllers
{
	public class MyCertificationController : Controller
	{
		private readonly MyCertificationServices _myCertificationServices;
		private readonly UserServices _userServices;
		private readonly ILogger _logger;

		public MyCertificationController(MyCertificationServices myCertificationServices, UserServices userServices,
			ILogger<MyCertificationController> logger)
		{
			_myCertificationServices = myCertificationServices;
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
				var certs = _myCertificationServices.GetMyCerts(_userServices.GetUserId());
				//var model = CreateIndexModel(certs);

				return View(certs);
			}
			catch (Exception ex)
			{
				return View("Error"); // Or another appropriate response
			}
		}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int diverCertificationId)
		{
			try
			{
				_myCertificationServices.Delete(diverCertificationId);
				TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] =
					"Certification successfully deleted.";

				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] =
					"there was an error deleting the Certification.";
				{
					return View("Error");
				}
			}
		}
	}
}
