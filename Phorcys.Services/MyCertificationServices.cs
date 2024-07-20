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

namespace Phorcys.Services
{
	public class MyCertificationServices
	{
		private readonly PhorcysContext _context;
		private readonly ILogger _logger;
		private readonly UserServices _userServices;

		public MyCertificationServices(PhorcysContext context, ILogger<MyCertificationServices> logger, UserServices userServices)
		{
			_context = context;
			_logger = logger;
			_userServices = userServices;
		}

		public IEnumerable<vwMyCertification> GetMyCerts(int userId)
		{
			try
			{
				var myCerts = _context.vwMyCertifications
					.Where(c => c.UserId == userId)
					.OrderByDescending(c => c.Certified).ToList();

				return myCerts;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error retreiving Certifications for user {userId}" + ex.Message, userId);
				throw new Exception("Can't connect to database");
			}
		}

		public MyCertificationDto GetMyCert(int diverCertificationId)
		{
			try
			{
				var myCertDto = new MyCertificationDto();
				var myCert = _context.DiverCertifications.Find(diverCertificationId);
				myCertDto.CertificationId = myCert.CertificationId;
				myCertDto.InstructorId = myCert.InstructorId;
				myCertDto.Certified = myCert.Certified;
				myCertDto.CertificationNum = myCert.CertificationNum;
				myCertDto.Notes = myCert.Notes;

				return myCertDto;
			}catch(SqlException ex)
			{
				_logger.LogError("Error connecting to the database: {message}", ex.Message);
				throw;
			}
		}

		public void Delete(int diverCertificationId)
		{
			try
			{
				var cert = _context.DiverCertifications.Find(diverCertificationId);
				if (cert != null)
				{
					_context.DiverCertifications.Remove(cert);
					_context.SaveChanges();
				}
			}
			catch (DbUpdateException ex)
			{
				_logger.LogError(ex, "Error deleting Certification {id}: {ErrorMessage}", diverCertificationId, ex.Message);
				throw ex;
			}
			catch (Exception ex)
			{
				_logger.LogError("Error deleting Certification: " + ex.Message);
			}
		}

		public void SaveNewDiverCertification(MyCertificationDto certDto)
		{
			try
			{
				var cert = new DiverCertification();
				var diverId = GetDiverId(_userServices.GetUserId());

				cert.DiverId = diverId;
				cert.CertificationId = certDto.CertificationId;
				cert.InstructorId = certDto.InstructorId;
				cert.CertificationNum = certDto.CertificationNum;
				cert.Certified = certDto.Certified;
				cert.Notes = certDto.Notes;
				cert.Created = DateTime.Now;
				cert.LastModified = DateTime.Now;

				_context.DiverCertifications.Add(cert);
				_context.SaveChanges();
			}
			catch (DbUpdateException ex)
			{
				_logger.LogError("Error saving Diver Certification");
				throw ex;
			}
		}

		private int GetDiverId(int userId)
		{
			int diverId = 0;
			var user = _userServices.GetUser(userId);
			Diver diver = _context.Divers.FirstOrDefault(d => d.ContactId == user.ContactId);
			if (diver == null)
			{
				//create diver record
				diverId = CreateNewDiver(user);
			}
			else
			{
				diverId = diver.DiverId;
			}
			return diverId;
		}

		private int CreateNewDiver(User user)
		{
			Diver diver = new Diver();
			diver.ContactId = (int)user.ContactId;
			_context.Divers.Add(diver);
			_context.SaveChanges();

			return diver.DiverId;
		}
	}
}

