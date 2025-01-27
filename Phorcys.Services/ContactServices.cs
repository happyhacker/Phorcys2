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

        //public MyCertificationDto GetMyCert(int diverCertificationId)
        //{
        //	try
        //	{
        //		var myCertDto = new MyCertificationDto();
        //		var myCert = _context.DiverCertifications.Include("Certification").FirstOrDefault(c => c.DiverCertificationId == diverCertificationId);
        //		myCertDto.AgencyId = (int)myCert.Certification.DiveAgencyId;
        //		myCertDto.CertificationId = myCert.CertificationId;
        //		myCertDto.InstructorId = myCert.InstructorId;
        //		myCertDto.Certified = myCert.Certified;
        //		myCertDto.CertificationNum = myCert.CertificationNum;
        //		myCertDto.Notes = myCert.Notes;

        //		return myCertDto;
        //	}catch(SqlException ex)
        //	{
        //		_logger.LogError("Error connecting to the database: {message}", ex.Message);
        //		throw;
        //	}
        //}

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
                    if (contact.Divers != null)
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
                throw ex;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error deleting Contact: " + ex.Message);
            }
        }

        //public void SaveNewDiverCertification(MyCertificationDto certDto)
        //{
        //	try
        //	{
        //		var cert = new DiverCertification();
        //		var diverId = GetDiverId(_userServices.GetUserId());

        //		cert.DiverId = diverId;
        //		cert.CertificationId = certDto.CertificationId;
        //		cert.InstructorId = certDto.InstructorId;
        //		cert.CertificationNum = certDto.CertificationNum;
        //		cert.Certified = certDto.Certified;
        //		cert.Notes = certDto.Notes;
        //		cert.Created = DateTime.Now;
        //		cert.LastModified = DateTime.Now;

        //		_context.DiverCertifications.Add(cert);
        //		_context.SaveChanges();
        //	}
        //	catch (DbUpdateException ex)
        //	{
        //		_logger.LogError("Error saving Diver Certification");
        //		throw ex;
        //	}
        //}

        //public void Save(MyCertificationDto certDto)
        //{
        //	try
        //	{
        //		var cert = _context.DiverCertifications.Find(certDto.DiverCertificationId);
        //		//var diverId = GetDiverId(_userServices.GetUserId());

        //		//cert.DiverId = diverId;
        //		cert.CertificationId = certDto.CertificationId;
        //		cert.InstructorId = certDto.InstructorId;
        //		cert.CertificationNum = certDto.CertificationNum;
        //		cert.Certified = certDto.Certified;
        //		cert.Notes = certDto.Notes;
        //		cert.LastModified = DateTime.Now;

        //		_context.DiverCertifications.Update(cert);
        //		_context.SaveChanges();
        //	}
        //	catch (DbUpdateException ex)
        //	{
        //		_logger.LogError("Database error saving Diver Certification: {msg}", ex.Message);
        //		throw ex;
        //	} catch(Exception ex)
        //	{
        //		_logger.LogError("Error saving Diver Certification: {msg}", ex.Message);
        //		throw ex;
        //	}
        //}


        //private int GetDiverId(int userId)
        //{
        //	int diverId = 0;
        //	var user = _userServices.GetUser(userId);
        //	Diver diver = _context.Divers.FirstOrDefault(d => d.ContactId == user.ContactId);
        //	if (diver == null)
        //	{
        //		//create diver record
        //		diverId = CreateNewDiver(user);
        //	}
        //	else
        //	{
        //		diverId = diver.DiverId;
        //	}
        //	return diverId;
        //}

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

