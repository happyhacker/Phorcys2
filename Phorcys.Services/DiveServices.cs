using Microsoft.EntityFrameworkCore;
using Phorcys.Data;
using Phorcys.Domain;

namespace Phorcys.Services;

public class DiveServices
{
	PhorcysContext context = new PhorcysContext();

	public IEnumerable<Dive> GetDives()
	{
		var dives = context.Dives.Include(d => d.DivePlan).ToList();
		return dives;
	}

}
