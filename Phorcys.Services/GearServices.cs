using Microsoft.Data.SqlClient;
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
				var gearList = _context.Gear.Where(r => r.UserId == userId || r.UserId == systemUser).OrderByDescending(l => l.Acquired).ToList();
				return gearList;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw new Exception("Can not connect to database");
			}
		}

		public void Delete(int id)
		{
			var gear = _context.Gear.Find(id);
			if (gear != null)
			{
				_context.Gear.Remove(gear);
				_context.SaveChanges();
			}
		}

		public void SaveNewGear(Gear gear)
		{
			try
			{
				_context.Gear.Add(gear);
				_context.SaveChanges();
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
		}


	}
}
