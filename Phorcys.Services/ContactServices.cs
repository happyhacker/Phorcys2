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
                    .Include(c => c.Diver)
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
                    .Include(c => c.Instructor)
                    .Include(c => c.DiveShop)
                    .Include(c => c.Diver)
                    .Include(c => c.DiveAgency)
                    .Include(c => c.Manufacturer)
                    .FirstOrDefault(c => c.ContactId == contactId);
                dto.ContactId = contactId;
				dto.UserId = contact.UserId;
				dto.IsInstructor = contact.Instructor != null;
                dto.IsDiveShop = contact.DiveShop != null;
                dto.IsAgency = contact.DiveAgency != null;
                dto.IsManufacturer = contact.Manufacturer != null;
                dto.IsDiver = contact.Diver != null;
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
				   .Include(c => c.Instructor)
				   .Include(c => c.DiveShop)
				   .Include(c => c.Diver)
				   .Include(c => c.DiveAgency)
				   .Include(c => c.Manufacturer)
				   .FirstOrDefault(c => c.ContactId == contactId);

				if (contact != null)
                {
                    if (contact.Diver != null)
                    {

                        _context.Divers.Remove(contact.Diver);
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
                contact.FirstName = dto.FirstName ?? "";
                contact.LastName = dto.LastName ?? "";
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

				var contactId = contact.ContactId; // Retrieve the newly created Contact ID

				// Step 2: Create Related Contact Types
				var newRecords = new List<object>();

				if (dto.IsDiver)
				{
					newRecords.Add(new Diver { ContactId = contactId });
				}

				if (dto.IsInstructor)
				{
					newRecords.Add(new Instructor { ContactId = contactId });
				}

				if (dto.IsDiveShop)
				{
					newRecords.Add(new DiveShop { ContactId = contactId });
				}

				if (dto.IsManufacturer)
				{
					newRecords.Add(new Manufacturer { ContactId = contactId });
				}

				if (dto.IsAgency)
				{
					newRecords.Add(new DiveAgency { ContactId = contactId });
				}

				// Step 3: Bulk Insert Related Records
				if (newRecords.Any())
				{
					_context.AddRange(newRecords);
				}
				_context.SaveChanges(); // Save related records
				//transaction.Commit(); // Commit transaction
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
					// Step 1: Find the existing contact
					var contact = _context.Contacts
						.Include(c => c.Diver)
						.Include(c => c.Instructor)
						.Include(c => c.DiveShop)
						.Include(c => c.Manufacturer)
						.Include(c => c.DiveAgency)
						.FirstOrDefault(c => c.ContactId == dto.ContactId);

					if (contact == null)
					{
						_logger.LogError("Contact with ID {id} not found.", dto.ContactId);
						return;
					}

					// Step 2: Update Contact properties
					contact.Company = dto.Company ?? "";
					contact.FirstName = dto.FirstName ?? "";
					contact.LastName = dto.LastName ?? "";
					contact.Address1 = dto.Address1 ?? "";
					contact.Address2 = dto.Address2 ?? "";
					contact.City = dto.City ?? "";
					contact.State = dto.State ?? "";
					contact.PostalCode = dto.PostalCode ?? "";
					contact.CountryCode = dto.CountryCode ?? "";
					contact.Email = dto.Email ?? "";
					contact.Gender = "";
					contact.Notes = dto.Notes ?? "";
					contact.LastModified = DateTime.Now;

					_context.Contacts.Update(contact);

					// Handle Adding and Deleting Contact Types

					// Track objects to delete
					var recordsToDelete = new List<object>();

					// Track objects to add
					var recordsToAdd = new List<object>();

					// Diver
					if (dto.IsDiver && contact.Diver == null)
					{
						recordsToAdd.Add(new Diver { ContactId = contact.ContactId });
					}
					else if (!dto.IsDiver && contact.Diver != null)
					{
						recordsToDelete.Add(contact.Diver);
					}

					// Instructor
					if (dto.IsInstructor && contact.Instructor == null)
					{
						recordsToAdd.Add(new Instructor { ContactId = contact.ContactId });
					}
					else if (!dto.IsInstructor && contact.Instructor != null)
					{
						recordsToDelete.Add(contact.Instructor);
					}

					// Dive Shop
					if (dto.IsDiveShop && contact.DiveShop == null)
					{
						recordsToAdd.Add(new DiveShop { ContactId = contact.ContactId });
					}
					else if (!dto.IsDiveShop && contact.DiveShop != null)
					{
						recordsToDelete.Add(contact.DiveShop);
					}

					// Manufacturer
					if (dto.IsManufacturer && contact.Manufacturer == null)
					{
						recordsToAdd.Add(new Manufacturer { ContactId = contact.ContactId });
					}
					else if (!dto.IsManufacturer && contact.Manufacturer != null)
					{
						recordsToDelete.Add(contact.Manufacturer);
					}

					// Dive Agency
					if (dto.IsAgency && contact.DiveAgency == null)
					{
						recordsToAdd.Add(new DiveAgency { ContactId = contact.ContactId });
					}
					else if (!dto.IsAgency && contact.DiveAgency != null)
					{
						recordsToDelete.Add(contact.DiveAgency);
					}

					// Delete records
					if (recordsToDelete.Any())
					{
						_context.RemoveRange(recordsToDelete);
					}

					// Add records
					if (recordsToAdd.Any())
					{
						_context.AddRange(recordsToAdd);
					}

					_context.SaveChanges();
				}
				catch (DbUpdateException ex)
				{
					_logger.LogError("Database error saving Contact: {msg}", ex.Message);
					throw;
				}
				catch (Exception ex)
				{
					_logger.LogError("Error saving Contact: {msg}", ex.Message);
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

		//Used as initiation when a user registers
		private int CreateNewContact(Phorcys.Domain.User user)
        {
            Contact contact = new Contact();
            contact.ContactId = (int)user.ContactId;
			contact.Company = "";
			contact.FirstName = "";
			contact.LastName = "";
			contact.Address1 = "";
			contact.Address2 = "";
			contact.City = "";
			contact.State = "";
			contact.PostalCode = "";
			contact.CountryCode = "";
			contact.Email = "";
			contact.Gender = "";
			contact.Notes = "";
			contact.CellPhone = "";
			contact.HomePhone = "";
			contact.WorkPhone = "";

			_context.Contacts.Add(contact);
            _context.SaveChanges();

            return contact.ContactId;
        }
    }
}

