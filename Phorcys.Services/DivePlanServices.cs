using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Phorcys.Data;
using Phorcys.Data.DTOs;
using Phorcys.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Phorcys.Services;

public class DivePlanServices
{
	private readonly PhorcysContext _context;
	private readonly ILogger _logger;

	public DivePlanServices(PhorcysContext context, ILogger<DivePlanServices> logger)
	{
		_context = context;
		_logger = logger;
	}

	//PhorcysContext context = new PhorcysContext();

	public IEnumerable<DivePlan> GetDivePlans(int userId)
	{
		var divePlans = _context.DivePlans.Where(r => r.UserId==userId).Include(d => d.DiveSite).ThenInclude(u => u.User).AsNoTracking().OrderByDescending(dp => dp.ScheduledTime).ToList();
		return divePlans;
	}

	public async Task<IEnumerable<DivePlan>> GetDivePlansAsync()
	{
		try
		{
			var divePlans = _context.DivePlans.Include(d => d.DiveSite).ThenInclude(u => u.User).AsNoTracking().OrderByDescending(dp => dp.ScheduledTime).ToList();
			return divePlans;
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			_logger.LogError(ex, "Can't connect to database: " + ex.Message);
			throw new Exception("Can't connect to database");
		}
	}

	public void SaveNewDivePlan(DivePlan divePlan, List<int> gearIds)
	{
		try
		{
			// Load actual Gear entities from the database
			var gears = _context.Gear
				.Where(g => gearIds.Contains(g.GearId))
				.ToList();

			// Assign the gears to the DivePlan's navigation property
			foreach (var gear in gears)
			{
				divePlan.Gears.Add(gear);
			}

			_context.DivePlans.Add(divePlan);
			_context.SaveChanges();
		}
		catch (SqlException ex)
		{
			Console.WriteLine(ex.Message);
			_logger.LogError(ex, "Error saving Dive Plan and Gear associations: " + ex.Message);
		}
	}

	/*public void SaveNewDivePlan(DivePlan divePlan)
	{
		try
		{
			_context.DivePlans.Add(divePlan);
			_context.SaveChanges();
		}
		catch (SqlException ex)
		{
			Console.WriteLine(ex.Message);
			_logger.LogError(ex, "Error saving Dive Plan : " + ex.Message);
		}
	}*/

	public void EditDivePlan(DivePlanDto divePlanDto)
	{
		var divePlan = GetDivePlan(divePlanDto.DivePlanId);
		divePlan.Title = divePlanDto.Title;
		divePlan.ScheduledTime = divePlanDto.ScheduledTime;
		divePlan.MaxDepth = divePlanDto.MaxDepth;	
		divePlan.Minutes = divePlanDto.Minutes;
		divePlan.Notes = divePlanDto.Notes;
		divePlan.DiveSiteId = divePlanDto.DiveSiteId;
		divePlan.LastModified = DateTime.Now;

		_context.Entry(divePlan).State = EntityState.Modified;
		_context.SaveChanges();
	}
	public DivePlan GetDivePlan(int divePlanId)
	{
		DivePlan divePlan = new DivePlan();
		try
		{
			divePlan = _context.DivePlans.FirstOrDefault(dp => dp.DivePlanId == divePlanId);
		} 
		catch (SqlException ex)
		{ 
			_logger.LogError(ex, "Error retreiving Dive Plan: " + ex.Message);
			throw;
		}
		return divePlan;
	}
	public void Delete(int id)
	{
		try { 
		var divePlan = _context.DivePlans.Find(id);
		if (divePlan != null)
		{
			_context.DivePlans.Remove(divePlan);
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
			_logger.LogError(ex, "Error deleting Dive Site {id}: {ErrorMessage}", id, ex.Message);
			throw;
		}
	}

}
