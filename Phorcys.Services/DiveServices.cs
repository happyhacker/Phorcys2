using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Phorcys.Data;
using Phorcys.Data.DTOs;
using Phorcys.Domain;

namespace Phorcys.Services
{
	public class DiveServices
	{
		private readonly PhorcysContext _context;
		private readonly ILogger<DiveServices> _logger;

		public DiveServices(PhorcysContext context, ILogger<DiveServices> logger) // Modify the constructor
		{
			_context = context;
			_logger = logger;
			_logger.LogInformation("DiveServices initialized.");
		}

		public async Task<IEnumerable<Dive>> GetDivesAsync(int userId)
		{
			try
			{
				var dives = await _context.Dives
										 .Where(r => r.UserId == userId)
										 .Include(d => d.DivePlan.DiveSite)
										 .ThenInclude(u => u.User)
										 .AsNoTracking()
										 .OrderByDescending(dive => dive.DiveNumber)
										 .ToListAsync();
				return dives;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred while getting dives for user {UserId}", userId); // Replace Console.WriteLine
				throw new Exception("Can't connect to database");
			}
		}

		public Dive GetDive(int diveId)
		{
			var dive = _context.Dives.FirstOrDefault(d => d.DiveId == diveId);
			return dive;
		}

		public void SaveNewDive(Dive dive, List<TanksOnDiveDto> tanks = null)
		{
			try
			{
				_context.Dives.Add(dive);
				_context.SaveChanges();

                if (tanks != null && tanks.Any() && dive.DivePlanId.HasValue)
                {
                    SaveTankPressures(dive.DivePlanId.Value, tanks);
                }
                _logger.LogInformation("Successfully logged new dive");

            }
            catch (SqlException ex)
			{
				_logger.LogError(ex, "Error occurred while saving a new dive");
			}
		}

		public void Edit(DiveDto diveDto)
		{
			try
			{
				var dive = GetDive(diveDto.DiveId);

				dive.DivePlanId = diveDto.DivePlanId;
				dive.DiveNumber = diveDto.DiveNumber;
				dive.Title = diveDto.Title;
				dive.Minutes = diveDto.Minutes;
				dive.DescentTime = diveDto.DescentTime;
				dive.AvgDepth = diveDto.AvgDepth;
				dive.MaxDepth = diveDto.MaxDepth;
				dive.Temperature = diveDto.Temperature;
				dive.AdditionalWeight = diveDto.AdditionalWeight;
				dive.Notes = diveDto.Notes ?? "";
				dive.LastModified = DateTime.Now;

				_context.SaveChanges();
				_logger.LogInformation("Updated dive {DiveId} for user {UserId}", diveDto.DiveId, dive.UserId);
			}
			catch (SqlException ex)
			{
				_logger.LogError(ex, "Error occurred while editing a dive"); // Replace Console.WriteLine
				throw;
			}
		}

        private void SaveTankPressures(int divePlanId, List<TanksOnDiveDto> tanks)
        {
            try
            {
                // Get existing TanksOnDive records for this dive plan
                var existingTanks = _context.TanksOnDives
                    .Where(t => t.DivePlanId == divePlanId)
                    .ToList();

                // Update pressure data for each tank
                foreach (var tankDto in tanks)
                {
                    var existingTank = existingTanks.FirstOrDefault(t => t.GearId == tankDto.GearId);
                    if (existingTank != null)
                    {
                        existingTank.StartingPressure = tankDto.StartingPressure;
                        existingTank.EndingPressure = tankDto.EndingPressure;
                        existingTank.OxygenPercent = tankDto.OxygenPercent;
                        existingTank.HeliumPercent = tankDto.HeliumPercent;
                        existingTank.GasContentTitle = tankDto.GasContentTitle;
                    }
                }

                _context.SaveChanges();
                _logger.LogInformation("Successfully saved tank pressures for dive plan {DivePlanId}", divePlanId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving tank pressures for dive plan {DivePlanId}", divePlanId);
                throw;
            }
        }


        public void Delete(int id)
		{
			try
			{
				var dive = _context.Dives.Find(id);
				if (dive != null)
				{
					_context.Dives.Remove(dive);
					_context.SaveChanges();
				}
			}
			catch (DbUpdateException ex)
			{
				_logger.LogError(ex, "Error deleting Dive Site {id}: {ErrorMessage}", id, ex.Message);
				throw ex;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}
