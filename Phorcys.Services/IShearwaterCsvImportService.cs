using Phorcys.Data.DTOs;

namespace Phorcys.Services
{
    /// <summary>
    /// Parses a Shearwater Cloud CSV export and returns summary and profile sample data for a single dive.
    /// </summary>
    public interface IShearwaterCsvImportService
    {
        /// <summary>
        /// Reads one Shearwater CSV file stream and returns the summary row and all profile samples.
        /// The returned DTO's Samples list is populated from rows 4+ of the CSV.
        /// Returns null if the file cannot be parsed or contains no data rows.
        /// </summary>
        Task<ShearwaterDiveSummaryDto?> ParseAsync(Stream csvStream);
    }
}
