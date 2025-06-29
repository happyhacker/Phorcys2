using Microsoft.Data.SqlClient;
using Phorcys.Data;
using Phorcys.Data.DTOs;
using Phorcys.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using System.Reflection.Metadata;

namespace Phorcys.Services
{
	public class GearServices
	{
		private readonly PhorcysContext _context;
		private readonly ILogger _logger;
		private const int systemUser = 6;
		public GearServices(PhorcysContext context, ILogger<GearServices> logger)
		{
			_context = context;
			_logger = logger;
		}

		public IEnumerable<Gear> GetGearList(int userId)
		{
			try
			{
				return _context.Gear
					.Where(g => g.UserId == userId)
					.OrderByDescending(g => g.Acquired)
					.ToList();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error fetching gear list for user {UserId}: {Message}", userId, ex.Message);
				throw new Exception("Unable to retrieve gear from the database.", ex);
			}
		}
		public List<GearDto> GetGearTitles(int userId)
		{
			try
			{
				var gearList = GetGearList(userId);
				var gearDtos = new List<GearDto>();
				foreach (Gear gear in gearList)
				{
				if (gear.NoLongerUse == null) 	
					gearDtos.Add(new GearDto
						{
							GearId = gear.GearId,
							Title = gear.Title
					});
				}
				return gearDtos;
			}
			catch(Exception ex)
			{
				_logger.LogError(ex, "Error fetching Gear Titles: {ex.Message}", ex.Message);
				throw new Exception("Can not retrieve gear from database");
			}
		}

		public GearDto GetGear(int gearId)
		{
			var gearDto = new GearDto();
			try
			{
				var gear = _context.Gear.Include(g => g.Tank).FirstOrDefault(g => g.GearId == gearId);
				if (gear != null)
				{
					gearDto.GearId = gearId;
					gearDto.Title = gear.Title;
					gearDto.Acquired = gear.Acquired;
					gearDto.RetailPrice = gear.RetailPrice;
					gearDto.Paid = gear.Paid;
					gearDto.Sn = gear.Sn;
					gearDto.NoLongerUse = gear.NoLongerUse;
					gearDto.Weight = gear.Weight;
					gearDto.Notes = gear.Notes;
					if (gear.Tank != null)
					{
						gearDto.Volume = gear.Tank.Volume;
						gearDto.WorkingPressure = gear.Tank.WorkingPressure;
						gearDto.ManufacturedMonth = gear.Tank.ManufacturedMonth;
						gearDto.ManufacturedYear = gear.Tank.ManufacturedYear;
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error retreiving Gear {gearId}: {ErrorMessage}", gearId, ex.Message);
				throw;
			}
			return gearDto;
		}
		public void Delete(int id)
		{
			try
			{
				var gear = _context.Gear.Find(id);
				if (gear != null)
				{
					_context.Gear.Remove(gear);
					_context.SaveChanges();
				}
			}
			catch (DbUpdateException ex)
			{
				_logger.LogError(ex, "Error deleting Dive Site {id}: {ErrorMessage}", id, ex.Message);
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error deleting Gear: {ErrorMessage}", id, ex.Message);
				throw;
			}
		}

		public void SaveNewGear(Gear gear)
		{
			try
			{
				_context.Gear.Add(gear);
				_context.SaveChanges();
			}
			catch (SqlException ex)
			{
				_logger.LogError(ex, "Error saving Gear: {Message}", ex.Message);
				throw;
			}
		}

		public void EditGear(GearDto gearDto)
		{
			try
			{
				var gear = _context.Gear.Include(g => g.Tank).FirstOrDefault(g => g.GearId == gearDto.GearId);
				if (gear != null)
				{
					gear.GearId = gearDto.GearId;
					gear.Title = gearDto.Title;
					gear.Acquired = gearDto.Acquired;
					gear.RetailPrice = gearDto.RetailPrice;
					gear.Paid = gearDto.Paid;
					gear.Sn = gearDto.Sn;
					gear.NoLongerUse = gearDto.NoLongerUse;
					gear.Weight = gearDto.Weight;
					gear.Notes = gearDto.Notes;
					gear.LastModified = DateTime.Now;
					if (
						gear.Tank != null
						| (
						  gearDto.Volume.HasValue && gearDto.Volume.Value != 0
						  | gearDto.WorkingPressure.HasValue && gearDto.WorkingPressure.Value != 0
						  | gearDto.ManufacturedYear.HasValue && gearDto.ManufacturedYear.Value != 0
						  | gearDto.ManufacturedMonth.HasValue && gearDto.ManufacturedMonth.Value != 0)
						)
					{
						gear.Tank.Volume = gearDto.Volume;
						gear.Tank.WorkingPressure = gearDto.WorkingPressure;
						gear.Tank.ManufacturedMonth = gearDto.ManufacturedMonth;
						gear.Tank.ManufacturedYear = gearDto.ManufacturedYear;
					}
					_context.Entry(gear).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
					_context.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error editing Gear {gearId}: {ErrorMessage}", gearDto.GearId, ex.Message);
				throw;
			}

		}
	}
}
