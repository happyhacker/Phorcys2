using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Phorcys.Data;
using Phorcys.Data.DTOs;
using Phorcys.Domain;

namespace Phorcys.Services;

public class DiveServices
{
	private readonly PhorcysContext _context;
	public DiveServices(PhorcysContext context)
	{
		_context = context;
	}

	//PhorcysContext context = new PhorcysContext();

	public async Task<IEnumerable<Dive>> GetDivesAsync(int userId)
	{
		try
		{
			var dives = await _context.Dives
									 .Where(r => r.UserId==userId)
									 .Include(d => d.DivePlan.DiveSite)
									 .ThenInclude(u => u.User)
									 .AsNoTracking()
									 .OrderByDescending(dive => dive.DiveNumber)
									 .ToListAsync();
			return dives;
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			throw new Exception("Can't connect to database");
		}
	}


	public Dive GetDive(int diveId)
	{
		var dive = _context.Dives.FirstOrDefault(d => d.DiveId == diveId);
		return dive;
	}
	public void SaveNewDive(Dive dive)
	{
		try
		{
			_context.Dives.Add(dive);
			_context.SaveChanges();
		}
		catch (SqlException ex)
		{
			Console.WriteLine(ex.Message + " Inner: " + ex.InnerException.Message);
		}
	}

	public void Edit(DiveDto diveDto)
	{
		try
		{
			var dive = GetDive(diveDto.DiveId);

			dive.DivePlanId = diveDto.DivePlanId;
			dive.DiveNumber = diveDto.DiveNumber;
			dive.Title = diveDto.Title;
			dive.Minutes = diveDto.Minutes;
			dive.DescentTime = diveDto.DescentTime;
			dive.AvgDepth = diveDto.AvgDepth;
			dive.MaxDepth = diveDto.MaxDepth;
			dive.Temperature = diveDto.Temperature;
			dive.AdditionalWeight = diveDto.AdditionalWeight;
			dive.Notes = diveDto.Notes;
			dive.LastModified = DateTime.Now;

			_context.SaveChanges();
		} catch (SqlException ex)
		{
			Console.WriteLine(ex.Message);
			throw;
		}
	}

	public void Delete(int id)
	{
		var dive = _context.Dives.Find(id);
		if (dive != null)
		{
			_context.Dives.Remove(dive);
			_context.SaveChanges();
		}
	}

}

