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
		private readonly InstructorServices _instructorServices;
		private readonly ILogger _logger;

		public MyCertificationController(ILogger<MyCertificationController> logger,
			UserServices userServices, 
			MyCertificationServices myCertificationServices, 
			InstructorServices instructorServices,
			AgencyServices agencyServices)
		{
            _logger = logger;
			_userServices = userServices;
            _myCertificationServices = myCertificationServices;
			_agencyServices = agencyServices;
			_instructorServices = instructorServices;
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
			model.InstructorListItems = BuildInstrucorList();
			return View(model);
		}

		[Authorize, HttpPost, ValidateAntiForgeryToken]
		public ActionResult Create(MyCertificationViewModel model)
		{
			if(ModelState.IsValid)
			{
				var myCert = new MyCertificationDto();
				myCert.DiverId = model.DiverId;
				myCert.CertificationId = model.CertificationId;
				myCert.InstructorId = model.InstructorId;
				myCert.CertificationNum = model.CertificationNum;
				myCert.Certified = model.Certified;
				myCert.Notes = model.Notes;

				_myCertificationServices.SaveNewDiverCertification(myCert);

                TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = "The Certification was successfully added.";
                return RedirectToAction("Index");
			} else { return View(model); }
		}

		[Authorize, HttpGet]
		public ActionResult Edit(int Id)
		{
			try
			{
				var model = new MyCertificationViewModel();
				MyCertificationDto myCertDto = _myCertificationServices.GetMyCert(Id);

				model.DiverCertificationId = myCertDto.DiverCertificationId;
				model.CertificationId = myCertDto.CertificationId;
				model.InstructorId = myCertDto.InstructorId;
				model.CertificationNum = myCertDto.CertificationNum;
				model.Certified = myCertDto.Certified;
				model.Notes = myCertDto.Notes;

				return View(model);
			}catch(Exception ex)
			{
				TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = "There was an error connecting to the database.";
				return RedirectToAction("Index");
			}
		}

		private IList<SelectListItem> BuildAgencyList(int diveAgencyId = 0)
		{
			IList<SelectListItem> agencyList = new List<SelectListItem>();
			IEnumerable<DiveAgency> agencies = _agencyServices.GetAgencies();
			SelectListItem item;

			foreach (var agency in agencies)
			{
				item = new SelectListItem();
				if(agency.DiveAgencyId == diveAgencyId)
				{
					item.Selected = true;
				}
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

        private IList<SelectListItem> BuildInstrucorList()
        {
            IList<SelectListItem> instructorList = new List<SelectListItem>();
            IEnumerable<Instructor> instructors = _instructorServices.GetInstructors();
            SelectListItem item;

            foreach (var instructor in instructors)
            {
                item = new SelectListItem();
                item.Text = instructor.Contact.LastName + ", " + instructor.Contact.FirstName;
                item.Value = instructor.InstructorId.ToString();
                instructorList.Add(item);
            }
            return instructorList;
        }

        [Authorize, HttpPost, ValidateAntiForgeryToken]
        public ActionResult UpdateCertificationList(MyCertificationViewModel model)
		{
			model.DiveAgencyListItems = BuildAgencyList(model.DiveAgencyId);
			model.CertificationListItems = BuildCertificationList(model.DiveAgencyId);
            model.InstructorListItems = BuildInstrucorList();
            return View("Create", model);
		}
	}
}