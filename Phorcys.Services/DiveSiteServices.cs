using Phorcys.Data;
using Phorcys.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phorcys.Services
{

	public class DiveSiteServices
	{
	PhorcysContext context = new PhorcysContext();
	public IEnumerable<DiveSite> GetDiveSites()
		{
			try
			{
				var diveSites = context.DiveSites.OrderByDescending(ds => ds.Title).ToList();
				/*var diveSites = new List<DiveSite>
				{
					new DiveSite {Title = "Site 1", DiveSiteId = 1},
					new DiveSite {Title = "Site 2", DiveSiteId = 2}
				};*/
				
				return diveSites;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw new Exception("Can't connect to database");
			}
		}

	}
}
