using Phorcys.Data;
using Phorcys.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phorcys.Services
{
	public class GearServices
	{
		private readonly PhorcysContext _context;
		private const int systemUser = 6;
		public GearServices(PhorcysContext context)
		{
			_context = context;
		}

		public IEnumerable<Gear> GetGear(int userId)
		{
			try
			{
				var gearList = _context.GearList.Where(r => r.UserId == userId || r.UserId == systemUser).OrderBy(l => l.Title).ToList();
				return gearList;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw new Exception("Can not connect to database");
			}
		}

	}
}
