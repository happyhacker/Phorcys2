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
	public class ContactController : Controller
	{
		private readonly ContactServices _contactServices;
		private readonly UserServices _userServices;
		private readonly ILogger _logger;

		public ContactController(ILogger<ContactController> logger,
			UserServices userServices, 
			ContactServices contactServices, 
			InstructorServices instructorServices,
			AgencyServices agencyServices)
		{
            _logger = logger;
			_userServices = userServices;
            _contactServices = contactServices;
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
				var contacts = _contactServices.GetContacts(_userServices.GetUserId());
				var model = CreateIndexModel(contacts);

				return View(model);
			}
			catch (Exception ex)
			{
				return View("Error"); // Or another appropriate response
			}
		}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int contactId)
		{
			try
			{
				_contactServices.Delete(contactId);
				TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] =
					"Contact successfully deleted.";
				//return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] =
					"there was an error deleting the Contact.";
			}
            return RedirectToAction("Index");
        }

        private List<ContactViewModel> CreateIndexModel(IEnumerable<Phorcys.Domain.Contact> contacts)
        {
            List<ContactViewModel> viewModel = new List<ContactViewModel>();
            ContactViewModel model;

            foreach (var contact in contacts)
            {
                model = new ContactViewModel();
                model.ContactId = contact.ContactId;
                model.Company = contact.Company;
                model.FirstName = contact.FirstName;
                model.LastName = contact.LastName;
                model.Address1 = contact.Address1;
				model.Address2 = contact.Address2;	
				model.City = contact.City;
				model.State = contact.State;
				model.PostalCode = contact.PostalCode;
				model.CountryCode = contact.CountryCode;
                model.Created = contact.Created;
                model.LastModified = contact.LastModified;
                model.Notes = contact.Notes;
                model.UserName = contact.UserId == Phorcys.Data.Constants.SystemUserId ? "System" : _userServices.GetUserName();
				viewModel.Add(model);
			}
            return viewModel;
        }

        [Authorize, HttpGet]
		public ActionResult Create()
		{
			var model = new ContactViewModel();
			//model.DiveAgencyListItems = BuildAgencyList();
			//string value = model.DiveAgencyListItems.First().Value;
			//model.CertificationListItems = BuildCertificationList(int.Parse(value));
			//model.InstructorListItems = BuildInstrucorList();
			return View(model);
		}

		//[Authorize, HttpPost, ValidateAntiForgeryToken]
		//public ActionResult Create(MyCertificationViewModel model)
		//{
		//	if(ModelState.IsValid)
		//	{
		//		var myCert = new MyCertificationDto();
		//		myCert.DiverId = model.DiverId;
		//		myCert.CertificationId = model.CertificationId;
		//		myCert.InstructorId = model.InstructorId;
		//		myCert.CertificationNum = model.CertificationNum;
		//		myCert.Certified = model.Certified;
		//		myCert.Notes = model.Notes;

		//		_myCertificationServices.SaveNewDiverCertification(myCert);

  //              TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = "The Certification was successfully added.";
  //              return RedirectToAction("Index");
		//	} else { return View(model); }
		//}

		//[Authorize, HttpGet]
		//public ActionResult Edit(int Id)
		//{
		//	try
		//	{
		//		var model = new MyCertificationViewModel();
		//		MyCertificationDto myCertDto = _myCertificationServices.GetMyCert(Id);
		//		model.CertificationId = Id;
		//		model.DiveAgencyListItems = BuildAgencyList(myCertDto.AgencyId);
		//		model.CertificationId = myCertDto.CertificationId;
		//		model.DiveAgencyId = myCertDto.AgencyId;
		//		model.CertificationListItems = BuildCertificationList(model.DiveAgencyId, model.CertificationId);
		//		model.InstructorListItems = BuildInstrucorList(myCertDto.InstructorId);
		//		model.DiverCertificationId = Id;

		//		model.InstructorId = myCertDto.InstructorId;
		//		model.CertificationNum = myCertDto.CertificationNum;
		//		model.Certified = myCertDto.Certified;
		//		model.Notes = myCertDto.Notes;

		//		return View(model);
		//	}catch(Exception ex)
		//	{
		//		TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = "There was an error connecting to the database.";
		//		return RedirectToAction("Index");
		//	}
		//}

		//[Authorize, HttpPost, ValidateAntiForgeryToken]
		//public ActionResult Edit(MyCertificationViewModel model)
		//{
		//	try
		//	{
		//		if (ModelState.IsValid)
		//		{
		//			var myCertDto = new MyCertificationDto();
		//			myCertDto.DiverCertificationId = model.DiverCertificationId;
		//			myCertDto.CertificationId = model.CertificationId;
		//			myCertDto.InstructorId = model.InstructorId;
		//			myCertDto.CertificationNum = model.CertificationNum;
		//			myCertDto.Certified = model.Certified;
		//			myCertDto.Notes = model.Notes;

		//			_myCertificationServices.Save(myCertDto);
		//			return RedirectToAction("Index");
		//		}
		//		else
		//		{
		//			return View(model);
		//		}
		//	}catch (Exception ex)
		//	{
		//		TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = "There was an error connecting to the database.";
		//		return RedirectToAction("Index");
		//	}
		//}

		//private IList<SelectListItem> BuildAgencyList(int diveAgencyId = 0)
		//{
		//	IList<SelectListItem> agencyList = new List<SelectListItem>();
		//	IEnumerable<DiveAgency> agencies = _agencyServices.GetAgencies();
		//	SelectListItem item;

		//	foreach (var agency in agencies)
		//	{
		//		item = new SelectListItem();
		//		if(agency.DiveAgencyId == diveAgencyId)
		//		{
		//			item.Selected = true;
		//		}
		//		item.Text = agency.Contact.Company;
		//		item.Value = agency.DiveAgencyId.ToString();
		//		agencyList.Add(item);
		//	}
		//	return agencyList;
		//}


		//private IList<SelectListItem> BuildInstrucorList(int? instructorId = 0)
  //      {
  //          IList<SelectListItem> instructorList = new List<SelectListItem>();
  //          IEnumerable<Instructor> instructors = _instructorServices.GetInstructors();
  //          SelectListItem item;

  //          foreach (var instructor in instructors)
  //          {
  //              item = new SelectListItem();
  //              item.Text = instructor.Contact.LastName + ", " + instructor.Contact.FirstName;
  //              item.Value = instructor.InstructorId.ToString();
		//		if(instructor.InstructorId == instructorId)
		//		{
		//			item.Selected = true;
		//		}
  //              instructorList.Add(item);
  //          }
  //          return instructorList;
  //      }

  //      [Authorize, HttpPost, ValidateAntiForgeryToken]
  //      public ActionResult UpdateCertificationList(MyCertificationViewModel model)
		//{
		//	model.DiveAgencyListItems = BuildAgencyList(model.DiveAgencyId);
		//	model.CertificationListItems = BuildCertificationList(model.DiveAgencyId);
  //          model.InstructorListItems = BuildInstrucorList();
  //          return View("Create", model);
		//}
	}
}