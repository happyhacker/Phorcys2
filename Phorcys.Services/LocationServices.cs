using Phorcys.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phorcys.Domain;
using Microsoft.Data.SqlClient;

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

		public void SaveNewLocation(DiveLocation location)
		{
			try
			{
				_context.DiveLocations.Add(location);
				_context.SaveChanges();
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		public void Delete(int id)
		{
			var location = _context.DiveLocations.Find(id);
			if (location != null)
			{
				_context.DiveLocations.Remove(location);
				_context.SaveChanges();
			}
		}
	}
}
