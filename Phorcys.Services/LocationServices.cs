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
        private readonly ILogger<DiveServices> _logger;
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
            }
            catch (Exception ex)
            {
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
            } catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
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
               // _logger.LogError(dbEx, "A database error occurred trying to retrieve the location.");
                throw;
            }
            catch (Exception ex)
            {
               // _logger.LogError(ex, "An unexpected error occurred.");
                throw;
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
