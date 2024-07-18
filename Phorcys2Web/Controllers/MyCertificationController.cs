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
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Phorcys.Web.Controllers
{
	public class MyCertificationController : Controller
	{
		private readonly MyCertificationServices _myCertificationServices;
		private readonly UserServices _userServices;
		private readonly AgencyServices _agencyServices;
		private readonly ILogger _logger;

		public MyCertificationController(MyCertificationServices myCertificationServices, UserServices userServices,
			ILogger<MyCertificationController> logger, AgencyServices agencyServices)
		{
			_myCertificationServices = myCertificationServices;
			_userServices = userServices;
			_logger = logger;
			_agencyServices = agencyServices;
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

		[Authorize, HttpGet]
		public ActionResult Create()
		{
			var model = new MyCertificationViewModel();
			model.DiveAgencyListItems = BuildAgencyList();
			string value = model.DiveAgencyListItems.First().Value;
			model.CertificationListItems = BuildCertificationList(int.Parse(value));
			return View(model);
		}

		private IList<SelectListItem> BuildAgencyList()
		{
			IList<SelectListItem> agencyList = new List<SelectListItem>();
			IEnumerable<DiveAgency> agencies = _agencyServices.GetAgencies();
			SelectListItem item;

			foreach (var agency in agencies)
			{
				item = new SelectListItem();
				item.Text = agency.Contact.Company;
				item.Value = agency.DiveAgencyId.ToString();
				agencyList.Add(item);
			}
			return agencyList;
		}


		public IList<SelectListItem> BuildCertificationList(int DiveAgencyId)
		{
			IList<SelectListItem> agencyCertificationList = new List<SelectListItem>();
			DiveAgency agency = _agencyServices.GetAgency(DiveAgencyId);
			SelectListItem item;

			foreach (var certification in agency.Certifications)
			{
				item = new SelectListItem();
				item.Text = certification.Title;
				item.Value = certification.CertificationId.ToString();
				agencyCertificationList.Add(item);
			}
			
			return agencyCertificationList;
		}

        [Authorize, HttpPost, ValidateAntiForgeryToken]
        public ActionResult UpdateCertificationList(MyCertificationViewModel model)
		{
			model.CertificationListItems = BuildCertificationList(model.DiveAgencyId);
			return RedirectToAction("Create", model);
		}
	}
}