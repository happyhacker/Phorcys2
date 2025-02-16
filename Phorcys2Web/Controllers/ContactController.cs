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
using Humanizer;

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
			model.CountryList = BuildCountryList("US");

			return View(model);
		}

		[Authorize, HttpPost, ValidateAntiForgeryToken]
		public ActionResult Create(ContactViewModel model)
		{
			if (ModelState.IsValid)
			{
				var dto = new ContactDto();
				dto.UserId = _userServices.GetUserId();
				dto.ContactId = model.ContactId;
				dto.Company = model.Company;
				dto.FirstName = model.FirstName;
				dto.LastName = model.LastName;
				dto.Address1 = model.Address1;
				dto.Address2 = model.Address2;
				dto.City = model.City;
				dto.State = model.State;
				dto.PostalCode = model.PostalCode;
				dto.CountryCode = model.SelectedCountryCode;
				dto.Email = model.Email;
				dto.Notes = model.Notes;

				_contactServices.SaveNewContact(dto);

				TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = "The Contact was successfully added.";
				return RedirectToAction("Index");
			}
			else { return View(model); }
		}

		[Authorize, HttpGet]
		public ActionResult Edit(int Id)
		{
			try
			{
				var model = new ContactViewModel();
				ContactDto dto = _contactServices.GetContact(Id);
				model.ContactId = Id;
				model.Company = dto.Company;
				model.FirstName = dto.FirstName;
				model.LastName = dto.LastName;
				model.Address1 = dto.Address1;
				model.Address2 = dto.Address2;
				model.City = dto.City;
				model.State = dto.State;
				model.PostalCode = dto.PostalCode;
				model.CountryList = BuildCountryList(dto.CountryCode);
				model.CountryCode = dto.CountryCode;
				model.Email = dto.Email;
				model.UserId = dto.UserId;
				model.Notes = dto.Notes;

				return View(model);
			}
			catch (Exception ex)
			{
				TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = "There was an error connecting to the database.";
				return RedirectToAction("Index");
			}
		}

		[Authorize, HttpPost, ValidateAntiForgeryToken]
		public ActionResult Edit(ContactViewModel model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var dto = new ContactDto();
					dto.ContactId = model.ContactId;
					dto.UserId = model.UserId;
					dto.Company = model.Company;
					dto.FirstName = model.FirstName;
					dto.LastName = model.LastName;
					dto.Address1 = model.Address1;
					dto.Address2 = model.Address2;
					dto.City = model.City;
					dto.State = model.State;
					dto.PostalCode = model.PostalCode;
					dto.CountryCode = model.SelectedCountryCode;
					dto.Email = model.Email;
					dto.Notes = model.Notes;
					_contactServices.Save(dto);
					TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = "The Contact was successfully updated.";
					return RedirectToAction("Index");
				}
				else
				{
					return View(model);
				}
			}
			catch (Exception ex)
			{
				TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = "There was an error connecting to the database.";
				return RedirectToAction("Index");
			}
		}

		private IList<SelectListItem> BuildCountryList(string countryCode)
		{
			IList<SelectListItem> countryList = new List<SelectListItem>();
			IEnumerable<Country> countries = _contactServices.GetCountries(countryCode);
			SelectListItem item;

			foreach (var country in countries)
			{
				item = new SelectListItem();
				if (country.CountryCode == countryCode)
				{
					item.Selected = true;
				}
				item.Text = country.Name;
				item.Value = country.CountryCode;
				countryList.Add(item);
			}
			return countryList;
		}


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