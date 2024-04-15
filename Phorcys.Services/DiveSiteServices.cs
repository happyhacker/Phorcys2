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
		private readonly PhorcysContext _context;
		public DiveSiteServices(PhorcysContext context)
		{
			_context = context;
		}

		public IEnumerable<DiveSite> GetDiveSites()
		{
			try
			{
				var diveSites = _context.DiveSites.OrderBy(ds => ds.Title).ToList();
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
