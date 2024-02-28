using Microsoft.EntityFrameworkCore;
using Phorcys.Data;
using Phorcys.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phorcys.Services;

public class DivePlanServices
{
	PhorcysContext context = new PhorcysContext();

	public IEnumerable<DivePlan> GetDivePlans()
	{
		var divePlans = context.DivePlans.Include(d => d.DiveSite).ThenInclude(u => u.User).AsNoTracking().OrderByDescending(dp => dp.ScheduledTime).ToList();
		return divePlans;
	}

}
