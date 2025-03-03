using Phorcys.Data;
using Phorcys.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Phorcys.Data.DTOs;
using Microsoft.Data.SqlClient;
using Telerik.SvgIcons;

namespace Phorcys.Services
{
    public class ContactServices
    {
        private readonly PhorcysContext _context;
        private readonly ILogger _logger;
        private readonly UserServices _userServices;
        private const int systemUser = 6;

        public ContactServices(PhorcysContext context, ILogger<ContactServices> logger, UserServices userServices)
        {
            _context = context;
            _logger = logger;
            _userServices = userServices;
        }

        public IEnumerable<Contact> GetContacts(int userId)
        {
            try
            {
                var contacts = _context.Contacts
                    .Include(c => c.Divers)
                    .Where(c => c.UserId == userId || c.UserId == systemUser)
                    .OrderBy(c => c.LastName).ToList();

                return contacts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retreiving Contacts for user {userId}" + ex.Message, userId);
                throw new Exception("Can't connect to database");
            }
        }

        public ContactDto GetContact(int contactId)
        {
            try
            {
                var dto = new ContactDto();
                var contact = _context.Contacts
                    .Include(c => c.Instructors)
                    .Include(c => c.DiveShops)
                    .Include(c => c.Divers)
                    .FirstOrDefault(c => c.ContactId == contactId);
                dto.ContactId = contactId;
				dto.UserId = contact.UserId;
				dto.IsInstructor = contact.Instructors?.Any() == true;
                dto.IsDiveShop = contact.DiveShops?.Any() == true;
                dto.IsDiver = contact.Divers?.Any() == true;
				dto.Company = contact.Company;
                dto.FirstName = contact.FirstName;
                dto.LastName = contact.LastName;
                dto.Email = contact.Email;
                dto.Address1 = contact.Address1;
                dto.Address2 = contact.Address2;    
                dto.City = contact.City;
                dto.State = contact.State;
                dto.PostalCode = contact.PostalCode;
                dto.CountryCode = contact.CountryCode;  
                dto.Notes = contact.Notes;

                return dto;
            }
            catch (SqlException ex)
            {
                _logger.LogError("Error connecting to the database: {message}", ex.Message);
                throw;
            }
        }

        public void Delete(int contactId)
        {
            try
            {
                //var contact = _context.Contacts.Find(contactId);
                var contact = _context.Contacts
                .Include(c => c.Divers)
                .FirstOrDefault(c => c.ContactId == contactId);
                if (contact != null)
                {
                    if (contact.Divers != null && contact.Divers.Any())
                    {
                        Diver diver = contact.Divers.FirstOrDefault(d => d.ContactId == contactId);
                        _context.Divers.Remove(diver);
                    }
                    _context.Contacts.Remove(contact);
                    _context.SaveChanges();
                }
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error deleting Contact {id}: {ErrorMessage}", contactId, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error deleting Contact: " + ex.Message);
            }
        }

        public void SaveNewContact(ContactDto dto)
        {
            try
            {
                var contact = new Contact();
                contact.UserId = dto.UserId;
                contact.Company = dto.Company ?? "";
                contact.FirstName = dto.FirstName;
                contact.LastName = dto.LastName;
                contact.Address1 = dto.Address1 ?? "";
                contact.Address2 = dto.Address2 ?? "";
                contact.City = dto.City ?? "";
                contact.State = dto.State ?? "";
                contact.PostalCode = dto.PostalCode ?? "";
                contact.CountryCode = dto.CountryCode ?? "";
                contact.Email = dto.Email ?? "";
                contact.CellPhone = "";
                contact.HomePhone = "";
                contact.WorkPhone = "";
                contact.Gender = "";
                contact.Notes = dto.Notes ?? "";
                contact.Created = DateTime.Now;
                contact.LastModified = DateTime.Now;

                _context.Contacts.Add(contact);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("Error saving Contact");
                throw;
            }
        }

        public void Save(ContactDto dto)
        {
            try
            {
                var contact = _context.Contacts.Find(dto.ContactId);

				contact.Company = dto.Company ?? "";
				contact.FirstName = dto.FirstName;
				contact.LastName = dto.LastName;
				contact.Address1 = dto.Address1 ?? "";
				contact.Address2 = dto.Address2 ?? "";
				contact.City = dto.City ?? "";
				contact.State = dto.State ?? "";
				contact.PostalCode = dto.PostalCode ?? "";
				contact.CountryCode = dto.CountryCode ?? "";
				contact.Email = dto.Email ?? "";
				contact.CellPhone = "";
				contact.HomePhone = "";
				contact.WorkPhone = "";
				contact.Gender = "";
				contact.Notes = dto.Notes ?? "";
				contact.LastModified = DateTime.Now;

				_context.Contacts.Update(contact);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("Database error saving Contact: {msg}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error saving Diver Certification: {msg}", ex.Message);
                throw;
            }
        }

        private string GetCountryCode(string code)
        {
            string countryCode = "";
            Country country = _context.Countries.FirstOrDefault(d => d.CountryCode == code);
            if (country == null)
            {
                countryCode = "US";
            }
            else
            {
                countryCode = country.CountryCode;
            }
            return countryCode;
        }

		public IEnumerable<Country> GetCountries(string code)
		{
			try
			{
				var countries = _context.Countries
										 .AsNoTracking()
										 .OrderBy(country => country.Name)
										 .ToList();
				return countries;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred while getting countries");
				throw new Exception("Can't connect to database");
			}
		}

		private int CreateNewContact(Phorcys.Domain.User user)
        {
            Contact contact = new Contact();
            contact.ContactId = (int)user.ContactId;
            _context.Contacts.Add(contact);
            _context.SaveChanges();

            return contact.ContactId;
        }
    }
}

