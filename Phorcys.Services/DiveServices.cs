using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using Phorcys.Data;
using Phorcys.Domain;
using System.Collections;

namespace Phorcys.Services;

public class DiveServices
{
	PhorcysContext context = new PhorcysContext();

	public async Task<IEnumerable<Dive>> GetDivesAsync()
	{
		try
		{
			var dives = await context.Dives
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
		var dive = context.Dives.FirstOrDefault(d => d.DiveId == diveId);
		return dive;
	}
	public void SaveNewDive(Dive dive)
	{
		try
		{
			context.Dives.Add(dive);
			context.SaveChanges();
		}
		catch (SqlException ex)
		{
			Console.WriteLine(ex.Message + " Inner: " + ex.InnerException.Message);
		}
	}
	public void Delete(int id)
	{
		var dive = context.Dives.Find(id);
		if (dive != null)
		{
			context.Dives.Remove(dive);
			context.SaveChanges();
		}
	}

}

