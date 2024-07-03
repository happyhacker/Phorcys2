using Phorcys.Data;
using Phorcys.Data.DTOs;
using Phorcys.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis;

namespace Phorcys.Services
{

	public class DiveSiteServices
	{
		private readonly PhorcysContext _context;
		private const int systemUser = 6;
		public DiveSiteServices(PhorcysContext context)
		{
			_context = context;
		}

		public IEnumerable<DiveSite> GetDiveSites(int userId)
		{
			try
			{
				var diveSites = _context.DiveSites.Include(d => d.DiveLocation).Where(r => r.UserId == userId || r.UserId == systemUser).OrderBy(ds => ds.Title).ToList();
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

		public void Save(SiteDto siteDto) { 
		  try
			{
				var site = new DiveSite();
				site.UserId	= siteDto.UserId;				
				site.Title = siteDto.Title;
				site.DiveLocationId = siteDto.DiveLocationId;
				site.IsFreshWater = siteDto.IsFreshWater;
				site.MaxDepth = siteDto.MaxDepth;
				site.GeoCode = siteDto.GeoCode;
				site.Notes = siteDto.Notes;
				site.Created = DateTime.Now;
				site.LastModified = DateTime.Now;

				_context.DiveSites.Add(site);
				_context.SaveChanges();

			} catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
		}

		public DiveSite GetDiveSite(int siteId)
		{
			try
			{
				var site = _context.DiveSites.FirstOrDefault(s => s.DiveSiteId == siteId);
				return site ?? new DiveSite();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error fetching DiveSite with ID {siteId}: {ex.Message}");
				throw;
			}
		}

		public void Edit(SiteDto siteDto)
		{
			try
			{
				var site = _context.DiveSites.Find(siteDto.DiveSiteId);
				if (site != null)
				{
					site.Title = siteDto.Title;
					site.DiveLocationId = siteDto.DiveLocationId;
					site.IsFreshWater = siteDto.IsFreshWater;
					site.MaxDepth = siteDto.MaxDepth;
					site.GeoCode = siteDto.GeoCode;
					site.Notes = siteDto.Notes;
					site.LastModified = DateTime.Now;
					//_context.Entry(site).State = EntityState.Modified;
					_context.SaveChanges();
				}
			}catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
		}

		public SiteDto GetSiteDto(int siteId)
		{
			var site = GetDiveSite(siteId);
			var siteDto = new SiteDto();

			siteDto.DiveSiteId = site.DiveSiteId;
			siteDto.UserId = site.UserId;
			siteDto.Title = site.Title;
			siteDto.DiveLocationId = site.DiveLocationId;
			siteDto.LocationSelectedId = site.DiveLocationId;
			siteDto.IsFreshWater = site.IsFreshWater;
			siteDto.MaxDepth = site.MaxDepth;
			siteDto.GeoCode = site.GeoCode;
			siteDto.Notes = site.Notes;

			return siteDto;
		}

		public void Delete(int id)
		{
			try
			{
				var site = _context.DiveSites.Find(id);
				if (site != null)
				{
					_context.DiveSites.Remove(site);
					_context.SaveChanges();
				}
			}catch(DbUpdateException ex)
			{
				Console.WriteLine(ex.Message);
				throw ex;
			}catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

	}
}
