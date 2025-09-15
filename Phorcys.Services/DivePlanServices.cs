﻿using Microsoft.Data.SqlClient;
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
using Microsoft.JSInterop.Infrastructure;

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
		var divePlans = _context.DivePlans.Where(r => r.UserId == userId).Include(d => d.DiveSite).ThenInclude(u => u.User).AsNoTracking().OrderByDescending(dp => dp.ScheduledTime).ToList();
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

	public void SaveNewDivePlan(DivePlan divePlan, List<int> gearIds, List<int> diveTypeIds)
	{
		try
		{
			// Load actual Gear entities from the database
			var gears = _context.Gear
				.Include(t => t.Tank)
				.Where(g => gearIds.Contains(g.GearId))
				.ToList();

			var diveTypes = _context.DiveTypes.Where(d => diveTypeIds.Contains(d.DiveTypeId))
				.ToList();

			foreach (var gear in gears)
			{
				divePlan.Gears.Add(gear);
				if(gear.Tank != null)
				{
					divePlan.TanksOnDives.Add(new TanksOnDive
					{
						DivePlan = divePlan,
						Tank = gear.Tank,
					});
				}
			}

			foreach(var diveType in diveTypes)
			{
				divePlan.DiveTypes.Add(diveType);
			}

			_context.DivePlans.Add(divePlan);
			_context.SaveChanges();
		}
		catch (SqlException ex)
		{
			Console.WriteLine(ex.Message);
			_logger.LogError(ex, "Error saving Dive Plan: " + ex.Message);
		}
	}

	public void EditDivePlan(DivePlanDto divePlanDto)
	{
		try
		{
			var divePlan = GetDivePlan(divePlanDto.DivePlanId);
			if (divePlan == null)
			{
				_logger.LogWarning("DivePlan not found for editing. ID = {DivePlanId}", divePlanDto.DivePlanId);
				return;
			}

			// Update scalar fields
			divePlan.Title = divePlanDto.Title;
			divePlan.ScheduledTime = divePlanDto.ScheduledTime;
			divePlan.MaxDepth = divePlanDto.MaxDepth;
			divePlan.Minutes = divePlanDto.Minutes;
			divePlan.Notes = divePlanDto.Notes ?? "";
			divePlan.DiveSiteId = divePlanDto.DiveSiteId;
			divePlan.LastModified = DateTime.Now;

			//Update the DiveType collection
			divePlan.DiveTypes.Clear();

			if(divePlanDto.SelectedDiveTypeIds != null && divePlanDto.SelectedDiveTypeIds.Any())
			{
				var selectedDiveTypes = _context.DiveTypes
					.Where(d => divePlanDto.SelectedDiveTypeIds.Contains(d.DiveTypeId))
					.ToList();
				foreach( var diveType in selectedDiveTypes)
				{
					divePlan.DiveTypes .Add(diveType);
				}
			}
			
			// Update the Gear collection
			divePlan.Gears.Clear(); // remove existing gear

			if (divePlanDto.SelectedGearIds != null && divePlanDto.SelectedGearIds.Any())
			{
				var selectedGears = _context.Gear
					.Where(g => divePlanDto.SelectedGearIds.Contains(g.GearId))
					.ToList();

				foreach (var gear in selectedGears)
				{
					divePlan.Gears.Add(gear);
				}
			}
			_context.SaveChanges();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error editing DivePlan with ID {DivePlanId}", divePlanDto.DivePlanId);
			throw;
		}
	}

	public DivePlan GetDivePlan(int divePlanId)
	{
		try
		{
			var divePlan = _context.DivePlans.Include(dp => dp.Gears)
				.Include(dp => dp.DiveTypes)
				.FirstOrDefault(dp => dp.DivePlanId == divePlanId);
			return divePlan;	
		}
		catch (SqlException ex)
		{
			_logger.LogError(ex, "Error retreiving Dive Plan with DivePlanId of {divePlanId}.",divePlanId);
			throw;
		}
	}

	public List<TanksOnDiveDto> GetTanksForDivePlan(int divePlanId)
	{
		return _context.TanksOnDives
			.Include(t => t.Tank)
			.ThenInclude(t => t.Gear)
			.Where(t => t.DivePlanId == divePlanId)
			.Select(t => new TanksOnDiveDto
			{
				GearId = t.GearId,
				StartingPressure = t.StartingPressure,
				EndingPressure = t.EndingPressure,
				GasContentTitle = t.GasContentTitle,
                OxygenPercent = t.OxygenPercent,
                HeliumPercent = t.HeliumPercent,
                GearTitle = t.Tank.Gear.Title
            })
			.ToList();
	}

	public void UpsertTanksOnDive(int divePlanId, IList<TanksOnDiveDto> posted)
	{
		posted ??= new List<TanksOnDiveDto>();

		// Load current rows for the plan
		var existing = _context.TanksOnDives
			.Where(t => t.DivePlanId == divePlanId)
			.ToList();

		// Index by GearId (or by TanksOnDiveId if you add that to the DTO)
		var existingByGear = existing.ToDictionary(x => x.GearId, x => x);

		// INSERT or UPDATE
		foreach (var dto in posted)
		{
			if (dto.GearId == 0) continue; // need a key to match

			if (existingByGear.TryGetValue(dto.GearId, out var row))
			{
				// UPDATE
				row.StartingPressure = dto.StartingPressure;
				row.EndingPressure = dto.EndingPressure;
				row.GasContentTitle = dto.GasContentTitle;
				row.OxygenPercent = dto.OxygenPercent;
				row.HeliumPercent = dto.HeliumPercent;

				_context.TanksOnDives.Update(row);
			}
			else
			{
				// INSERT
				var newRow = new TanksOnDive
				{
					DivePlanId = divePlanId,
					GearId = dto.GearId,
					StartingPressure = dto.StartingPressure,
					EndingPressure = dto.EndingPressure,
					GasContentTitle = dto.GasContentTitle,
					OxygenPercent = dto.OxygenPercent,
					HeliumPercent = dto.HeliumPercent,
				};
				_context.TanksOnDives.Add(newRow);
			}
		}

		// DELETE rows not posted (optional)
		var postedGearIds = new HashSet<int>(posted.Where(p => p.GearId != 0).Select(p => p.GearId));
		var toDelete = existing.Where(e => !postedGearIds.Contains(e.GearId)).ToList();
		if (toDelete.Count > 0)
			_context.TanksOnDives.RemoveRange(toDelete);

		_context.SaveChanges();
	}

	public void Delete(int id)
	{
		try
		{
			var divePlan = _context.DivePlans
			.Include(dp => dp.Gears)
			.Include(dt => dt.DiveTypes)
			.Include(tod => tod.TanksOnDives)
			.FirstOrDefault(dp => dp.DivePlanId == id);
			if (divePlan != null)
			{
				_context.DivePlans.Remove(divePlan);
				_context.SaveChanges();
			}
		}
		catch (DbUpdateException ex)
		{
			_logger.LogError(ex, "Error deleting Dive Plan {id}: {ErrorMessage}", id, ex.Message);
			throw;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error deleting Dive Plan {id}: {ErrorMessage}", id, ex.Message);
			throw;
		}
	}

}
