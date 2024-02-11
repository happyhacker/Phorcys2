using Microsoft.EntityFrameworkCore;
using Phorcys.Data;
using Phorcys.Domain;
using System.Collections;

namespace Phorcys.Services;

public class DiveServices
{
	PhorcysContext context = new PhorcysContext();

	public IEnumerable<Dive> GetDives()
	{
		var dives = context.Dives.Include(d => d.DivePlan.DiveSite).ThenInclude(u => u.User).OrderByDescending(dive => dive.DiveNumber).ToList();
		return dives;
	}

	public void SaveNewDive(Dive dive)
	{
		try
		{
			context.Dives.Add(dive);
			context.SaveChanges();
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message + " Inner: " + ex.InnerException.Message);
		}


	}
}
