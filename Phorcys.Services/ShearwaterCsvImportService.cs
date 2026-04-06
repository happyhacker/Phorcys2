using Microsoft.Extensions.Logging;
using Phorcys.Data.DTOs;
using System.Text;

namespace Phorcys.Services
{
    /// <summary>
    /// Parses the summary section of a Shearwater Cloud CSV export.
    ///
    /// Shearwater Cloud exports a CSV with one header row followed by one row per dive.
    /// Column names vary slightly across device models and export versions, so this
    /// parser uses flexible (normalized) column name matching.
    ///
    /// Phase 1 only: parses the first data row as the summary for a single dive.
    /// Profile sample rows are not parsed.
    /// </summary>
    public class ShearwaterCsvImportService : IShearwaterCsvImportService
    {
        private readonly ILogger<ShearwaterCsvImportService> _logger;

        public ShearwaterCsvImportService(ILogger<ShearwaterCsvImportService> logger)
        {
            _logger = logger;
        }

        public async Task<ShearwaterDiveSummaryDto?> ParseAsync(Stream csvStream)
        {
            try
            {
                using var reader = new StreamReader(csvStream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, leaveOpen: true);

                // Read all non-empty lines
                var lines = new List<string>();
                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                        lines.Add(line);
                }

                if (lines.Count < 2)
                {
                    _logger.LogWarning("Shearwater CSV has fewer than 2 non-empty lines; cannot parse.");
                    return null;
                }

                // Parse header row
                var headers = SplitCsvLine(lines[0]);
                var columnIndex = BuildColumnIndex(headers);

                // Parse first data row
                var fields = SplitCsvLine(lines[1]);

                return MapRow(fields, columnIndex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error parsing Shearwater CSV file.");
                return null;
            }
        }

        // -----------------------------------------------------------------------
        // Column index building — tolerant matching
        // -----------------------------------------------------------------------

        /// <summary>
        /// Maps a normalized column key to its 0-based index in the header row.
        /// Unknown columns are silently ignored.
        /// </summary>
        private static Dictionary<string, int> BuildColumnIndex(string[] headers)
        {
            var index = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < headers.Length; i++)
            {
                var normalized = NormalizeColName(headers[i]);
                if (!string.IsNullOrEmpty(normalized))
                    index.TryAdd(normalized, i);  // first occurrence wins on duplicate headers
            }
            return index;
        }

        /// <summary>
        /// Strips spaces, punctuation, and parenthesized units so "Max Depth (ft)"
        /// and "MaxDepth" both normalize to "MAXDEPTH".
        /// </summary>
        private static string NormalizeColName(string name)
        {
            var sb = new StringBuilder();
            foreach (char c in name.ToUpperInvariant())
            {
                if (char.IsLetterOrDigit(c))
                    sb.Append(c);
            }
            return sb.ToString();
        }

        // -----------------------------------------------------------------------
        // Row mapping
        // -----------------------------------------------------------------------

        private ShearwaterDiveSummaryDto MapRow(string[] fields, Dictionary<string, int> idx)
        {
            var dto = new ShearwaterDiveSummaryDto();

            dto.DiveNumber      = GetInt(fields, idx, "DIVENUMBER", "DIVE");
            dto.StartTime       = GetDateTime(fields, idx, "STARTDATE", "STARTTIME", "DIVETIME", "DATE");
            dto.MaxDepth        = GetInt(fields, idx, "MAXDEPTHFT", "MAXDEPTHM", "MAXDEPTH", "DEEPEST");
            dto.SerialNumber    = GetString(fields, idx, "COMPUTERSERIALNUMBER", "SERIALNO", "SN", "SERIAL");
            dto.FirmwareVersion = GetString(fields, idx, "COMPUTERFIRMWAREVERSION", "FIRMWARE", "FWVERSION", "FW");
            dto.CnsBeforePercent = GetInt(fields, idx, "STARTCNS", "STARTO2CNSPERCENT", "STARTCNS", "CNSBEFORE", "CNSBEFORE");
            dto.CnsAfterPercent  = GetInt(fields, idx, "ENDCNS", "ENDO2CNSPERCENT", "ENDCNS", "CNSAFTER");
            dto.BatteryVoltage   = GetFloat(fields, idx, "ENDBATTERYVOLTAGE", "BATTERYPERCENT", "BATTERYVOLTS", "BATTERYVOLTAGE");

            // "Computer Name" or "Device" is the product/model name
            dto.Product = GetString(fields, idx, "PRODUCT", "COMPUTER", "DEVICE", "DEVICENAME", "DIVERCOMPUTER");

            // Duration is reported in seconds by Shearwater; convert to rounded minutes
            int? durationSeconds = GetInt(fields, idx, "MAXTIME", "DURATION", "DURATIONSSECONDS", "TIMES", "DIVETIME");
            if (durationSeconds.HasValue)
                dto.DurationMinutes = (int)Math.Round(durationSeconds.Value / 60.0);

            return dto;
        }

        // -----------------------------------------------------------------------
        // Field helpers — defensive getters that return null on any parse failure
        // -----------------------------------------------------------------------

        private static string? GetString(string[] fields, Dictionary<string, int> idx, params string[] candidates)
        {
            foreach (var key in candidates)
            {
                if (idx.TryGetValue(key, out int col) && col < fields.Length)
                {
                    var val = fields[col].Trim().Trim('"');
                    if (!string.IsNullOrEmpty(val))
                        return val;
                }
            }
            return null;
        }

        private static int? GetInt(string[] fields, Dictionary<string, int> idx, params string[] candidates)
        {
            var raw = GetString(fields, idx, candidates);
            if (raw == null) return null;

            // Strip trailing "%" or unit suffixes (e.g. "100%", "82 ft")
            var cleaned = raw.Split(' ')[0].TrimEnd('%');
            if (int.TryParse(cleaned, out int result))
                return result;
            // Try parsing as double and rounding (e.g. "82.3")
            if (double.TryParse(cleaned, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out double dbl))
                return (int)Math.Round(dbl);
            return null;
        }

        private static float? GetFloat(string[] fields, Dictionary<string, int> idx, params string[] candidates) {
            var raw = GetString(fields, idx, candidates);
            if(raw == null)
                return null;

            // Strip trailing "%" or unit suffixes (e.g. "1.6v", "82 ft")
            var cleaned = raw.Split(' ')[0].TrimEnd('%');
            if(float.TryParse(cleaned, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out float result))
                return result;

            return null;
        }

        private static DateTime? GetDateTime(string[] fields, Dictionary<string, int> idx, params string[] candidates)
        {
            var raw = GetString(fields, idx, candidates);
            if (raw == null) return null;

            // Common Shearwater export date formats (local/device time — do NOT convert to UTC)
            string[] formats =
            {
                "yyyy-MM-dd HH:mm:ss",
                "yyyy-MM-dd HH:mm",
                "yyyy-MM-ddTHH:mm:ss",
                "yyyy-MM-ddTHH:mm",
                "MM/dd/yyyy HH:mm:ss",
                "MM/dd/yyyy HH:mm",
                "MM/dd/yyyy h:mm:ss tt",
                "MM/dd/yyyy h:mm tt",
                "M/d/yyyy H:mm:ss",
                "M/d/yyyy H:mm",
            };

            if (DateTime.TryParseExact(raw, formats,
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out DateTime result))
            {
                return result;
            }

            // Fallback: try general parse
            if (DateTime.TryParse(raw, out DateTime fallback))
                return fallback;

            return null;
        }

        // -----------------------------------------------------------------------
        // CSV line splitter — handles double-quoted fields with embedded commas
        // -----------------------------------------------------------------------

        private static string[] SplitCsvLine(string line)
        {
            var fields = new List<string>();
            var current = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == '"')
                {
                    // Handle escaped quotes ("")
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        current.Append('"');
                        i++;
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (c == ',' && !inQuotes)
                {
                    fields.Add(current.ToString());
                    current.Clear();
                }
                else
                {
                    current.Append(c);
                }
            }

            fields.Add(current.ToString());
            return fields.ToArray();
        }
    }
}
