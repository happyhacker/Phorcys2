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

        public async Task<IEnumerable<Dive>> GetDivesAsync(int userId) {
            try {
                var dives = await _context.Dives
                    .Where(d => d.UserId == userId)
                    .Include(d => d.DivePlan!)
                        .ThenInclude(dp => dp.DiveSite)
                    .Include(d => d.DivePlan!)
                        .ThenInclude(dp => dp.DiveTeams)
                            .ThenInclude(dt => dt.Diver)
                                .ThenInclude(diver => diver.Contact)
                    .AsNoTracking()
                    .OrderByDescending(d => d.DiveNumber)
                    .ToListAsync();

                return dives;
            }
            catch(Exception ex) {
                _logger.LogError(ex, "Error occurred while getting dives for user {UserId}", userId);
                throw new Exception("Can't connect to database");
            }
        }

        public Dive? GetDive(int diveId)
		{
			var dive = _context.Dives.FirstOrDefault(d => d.DiveId == diveId);
			return dive;
		}

		public Dive? GetDiveWithPlan(int diveId)
		{
			var dive = _context.Dives
				.Include(d => d.DivePlan!)
					.ThenInclude(dp => dp.DiveSite)
				.FirstOrDefault(d => d.DiveId == diveId);
			return dive;
		}

		public void SaveNewDive(Dive dive, List<TanksOnDiveDto>? tanks = null)
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
				var dive = GetDive(diveDto.DiveId)
					?? throw new KeyNotFoundException($"Dive {diveDto.DiveId} not found.");

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
						existingTank.FillDate = tankDto.FillDate;
						existingTank.FillCost = tankDto.FillCost;
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


        /// <summary>
        /// Persists a DiveComputerLog record for the given dive.
        /// Call this after SaveNewDive so DiveId is available.
        /// Note: DiveComputerLog.GearId is [NotMapped] and will not be persisted;
        /// the matched gear is identified via SerialNumber in the stored row.
        /// </summary>
        public void SaveDiveComputerLog(DiveComputerLog log)
        {
            try
            {
                _context.DiveComputerLogs.Add(log);
                _context.SaveChanges();
                _logger.LogInformation("Saved DiveComputerLog for DiveId {DiveId}", log.DiveId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving DiveComputerLog for DiveId {DiveId}", log.DiveId);
                throw;
            }
        }

        public void SaveLogSamples(int diveComputerLogId, List<LogSampleDto> sampleDtos)
        {
            try
            {
                var samples = sampleDtos.Select(dto => new LogSample
                {
                    DiveComputerLogId    = diveComputerLogId,
                    ElapsedSeconds       = dto.ElapsedSeconds,
                    Depth                = dto.Depth,
                    FirstDecoStopDepth   = dto.FirstDecoStopDepth,
                    TimeToSurfaceMinutes = dto.TimeToSurfaceMinutes,
                    AvgPPO2              = dto.AvgPPO2,
                    FractionO2           = dto.FractionO2,
                    FractionHe           = dto.FractionHe,
                    FirstDecoStopMinutes = dto.FirstDecoStopMinutes,
                    NoDecoLimitMinutes   = dto.NoDecoLimitMinutes,
                    CircuitMode          = dto.CircuitMode,
                    CCRMode              = dto.CCRMode,
                    Temperature          = dto.Temperature,
                    GasSwitchNeeded      = dto.GasSwitchNeeded,
                    ExternalPPO2Active   = dto.ExternalPPO2Active,
                    SetPointType         = dto.SetPointType,
                    CircuitSwitchType    = dto.CircuitSwitchType,
                    O2Sensor1Millivolts  = dto.O2Sensor1Millivolts,
                    O2Sensor2Millivolts  = dto.O2Sensor2Millivolts,
                    O2Sensor3Millivolts  = dto.O2Sensor3Millivolts,
                    BatteryVoltage       = dto.BatteryVoltage,
                    AscentRate           = dto.AscentRate,
                    SafeAscentDepth      = dto.SafeAscentDepth,
                    CO2Millibar          = dto.CO2Millibar,
                }).ToList();

                _context.LogSamples.AddRange(samples);
                _context.SaveChanges();
                _logger.LogInformation("Saved {Count} log samples for DiveComputerLogId {Id}", samples.Count, diveComputerLogId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving log samples for DiveComputerLogId {Id}", diveComputerLogId);
                throw;
            }
        }

        public HashSet<int> GetDiveIdsWithComputerLogs(int userId)
        {
            return _context.DiveComputerLogs
                .Where(log => log.DiveId.HasValue && log.Dive != null && log.Dive.UserId == userId)
                .Select(log => log.DiveId!.Value)
                .ToHashSet();
        }

        public IEnumerable<LogSample> GetLogSamplesForDive(int diveId)
        {
            try
            {
                return _context.DiveComputerLogs
                    .Where(log => log.DiveId == diveId)
                    .Include(log => log.LogSamples)
                    .SelectMany(log => log.LogSamples)
                    .OrderBy(s => s.ElapsedSeconds)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading log samples for diveId {DiveId}", diveId);
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
				throw;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}
