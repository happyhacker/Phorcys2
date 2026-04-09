using Phorcys.Data.DTOs;

namespace Phorcys.Services
{
    /// <summary>
    /// Parses a Shearwater Cloud CSV export and returns summary data for a single dive.
    /// Phase 1 only: summary import. Profile samples are not parsed.
    /// </summary>
    public interface IShearwaterCsvImportService
    {
        /// <summary>
        /// Reads one Shearwater CSV file stream and returns the first summary row found.
        /// Returns null if the file cannot be parsed or contains no data rows.
        /// </summary>
        Task<ShearwaterDiveSummaryDto?> ParseAsync(Stream csvStream);
    }
}
