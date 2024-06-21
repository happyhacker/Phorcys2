using Microsoft.Data.SqlClient;
using Phorcys.Data;
using Phorcys.Data.DTOs;
using Phorcys.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Linq.Expressions;

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

        public IEnumerable<Gear> GetGearList(int userId)
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

        public GearDto GetGear(int gearId)
        {
            var gearDto = new GearDto();
            try
            {
                var gear = _context.Gear.Include(g => g.Tank).FirstOrDefault(g => g.GearId == gearId);
                if (gear != null)
                {
                    gearDto.GearId = gearId;
                    gearDto.Title = gear.Title;
                    gearDto.Acquired = gear.Acquired;
                    gearDto.RetailPrice = gear.RetailPrice;
                    gearDto.Paid = gear.Paid;
                    gearDto.Sn = gear.Sn;
                    gearDto.NoLongerUse = gear.NoLongerUse;
                    gearDto.Weight = gear.Weight;
                    gearDto.Notes = gear.Notes;
                    if (gear.Tank != null)
                    {
                        gearDto.Volume = gear.Tank.Volume;
                        gearDto.WorkingPressure = gear.Tank.WorkingPressure;
                        gearDto.ManufacturedMonth = gear.Tank.ManufacturedMonth;
                        gearDto.ManufacturedYear = gear.Tank.ManufacturedYear;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return gearDto;
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

        public void EditGear(GearDto gearDto)
        {
           try { 
            var gear = _context.Gear.Include(g => g.Tank).FirstOrDefault(g => g.GearId == gearDto.GearId);
                if (gear != null)
                {
                    gear.GearId = gearDto.GearId;
                    gear.Title = gearDto.Title;
                    gear.Acquired = gearDto.Acquired;
                    gear.RetailPrice = gearDto.RetailPrice;
                    gear.Paid = gearDto.Paid;
                    gear.Sn = gearDto.Sn;
                    gear.NoLongerUse = gearDto.NoLongerUse;
                    gear.Weight = gearDto.Weight;
                    gear.Notes = gearDto.Notes;
                    gear.LastModified = DateTime.Now;
                    if (
                        gear.Tank != null
                        | (
                          gearDto.Volume.HasValue && gearDto.Volume.Value != 0
                          | gearDto.WorkingPressure.HasValue && gearDto.WorkingPressure.Value != 0
                          | gearDto.ManufacturedYear.HasValue && gearDto.ManufacturedYear.Value != 0
                          | gearDto.ManufacturedMonth.HasValue && gearDto.ManufacturedMonth.Value != 0)
                        )
                    {
                        gear.Tank.Volume = gearDto.Volume;
                        gear.Tank.WorkingPressure = gearDto.WorkingPressure;
                        gear.Tank.ManufacturedMonth = gearDto.ManufacturedMonth;
                        gear.Tank.ManufacturedYear = gearDto.ManufacturedYear;
                    }
                    _context.Entry(gear).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
    }
}
