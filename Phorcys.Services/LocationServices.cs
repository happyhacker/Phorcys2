using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phorcys.Domain;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Phorcys.Data;
using Phorcys.Data.DTOs;

namespace Phorcys.Services
{
	public class LocationServices
	{
		private readonly PhorcysContext _context;
		private readonly ILogger _logger;
		private const int systemUser = 6;
		public LocationServices(PhorcysContext context, ILogger<LocationServices> logger)
		{
			_context = context;
			_logger = logger;
		}

		public IEnumerable<DiveLocation> GetLocations(int userId)
		{
			try
			{
				var locations = _context.DiveLocations.Where(r => r.UserId == userId || r.UserId == systemUser).OrderBy(l => l.Title).ToList();
				return locations;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error retreiving Locations: {ErrorMessage}", ex.Message);

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
				_logger.LogError(ex, "Error creating new Location: {ErrorMessage}", ex.Message);
			}
		}

		public void EditLocation(LocationDto locationDto)
		{
			try
			{
				var location = GetLocation(locationDto.DiveLocationId);
				location.Title = locationDto.Title;
				location.Notes = locationDto.Notes;
				location.LastModified = DateTime.Now;

				_context.Entry(location).State = EntityState.Modified;
				_context.SaveChanges();
			}
			catch (SqlException ex)
			{
				_logger.LogError(ex, "Error editing Location: {ErrorMessage}", ex.Message);
			}
		}
		public DiveLocation GetLocation(int id)
		{
			try
			{
				var location = _context.DiveLocations.FirstOrDefault(l => l.DiveLocationId == id);
				return location ?? new DiveLocation();
			}
			catch (DbException dbEx)
			{
				_logger.LogError(dbEx, "A database error occurred trying to retrieve the location: {Message}", dbEx.Message);
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error retreiving a Location: {Message}", ex.Message);
				throw;
			}
		}

		public void Delete(int id)
		{
			try
			{
				var location = _context.DiveLocations.Find(id);
				if (location != null)
				{
					_context.DiveLocations.Remove(location);
					_context.SaveChanges();
				}

			}
			catch (DbUpdateException ex)
			{
				_logger.LogError(ex, "Error deleting Location {id}: {ErrorMessage}", id, ex.Message);
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error deleting Location {id}: {ErrorMessage}", id, ex.Message);
				throw;
			}
		}
	}
}
