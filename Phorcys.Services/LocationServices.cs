using Phorcys.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phorcys.Domain;

namespace Phorcys.Services
{
	public class LocationServices
	{
		private readonly PhorcysContext _context;
		private const int systemUser = 6;
		public LocationServices(PhorcysContext context)
		{
			_context = context;
		}

		public IEnumerable<DiveLocation> GetLocations(int userId)
		{
			try
			{
				var locations = _context.DiveLocations.Where(r => r.UserId == userId || r.UserId == systemUser).OrderBy(l => l.Title).ToList();
				return locations;
			} catch(Exception ex) {
			    Console.WriteLine(ex.Message);
				throw new Exception("Can not connect to database");
			}
		}
	}
}
