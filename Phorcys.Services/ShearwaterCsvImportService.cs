using Microsoft.Extensions.Logging;
using Phorcys.Data.DTOs;
using System.Text;
using System.Collections.Generic;

namespace Phorcys.Services {
    /// <summary>
    /// Parses a Shearwater Cloud CSV export.
    ///
    /// Row 1: summary column headers (Dive Number, Start Date, Max Depth, …)
    /// Row 2: summary values for the dive
    /// Row 3: profile sample headers
    /// Row 4+: profile sample data (one row per sample interval)
    /// </summary>
    public class ShearwaterCsvImportService : IShearwaterCsvImportService {
        private readonly ILogger<ShearwaterCsvImportService> _logger;

        public ShearwaterCsvImportService(ILogger<ShearwaterCsvImportService> logger) {
            _logger = logger;
        }

        public async Task<ShearwaterDiveSummaryDto?> ParseAsync(Stream csvStream) {
            try {
                using var reader = new StreamReader(csvStream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, leaveOpen: true);

                // Read all non-empty lines
                var lines = new List<string>();
                string? line;
                while((line = await reader.ReadLineAsync()) != null) {
                    if(!string.IsNullOrWhiteSpace(line))
                        lines.Add(line);
                }

                if(lines.Count < 2) {
                    _logger.LogWarning("Shearwater CSV has fewer than 2 non-empty lines; cannot parse.");
                    return null;
                }

                // Parse summary header row and data row
                var headers = SplitCsvLine(lines[0]);
                var columnIndex = BuildColumnIndex(headers);
                var fields = SplitCsvLine(lines[1]);
                var dto = MapRow(fields, columnIndex);

                // Parse profile sample rows (row 3 = sample headers, rows 4+ = sample data)
                if(lines.Count >= 4) {
                    var sampleHeaders = SplitCsvLine(lines[2]);
                    var sampleIndex = BuildColumnIndex(sampleHeaders);
                    for(int i = 3; i < lines.Count; i++) {
                        var sampleFields = SplitCsvLine(lines[i]);
                        dto.Samples.Add(MapSampleRow(sampleFields, sampleIndex));
                    }
                }

                return dto;
            }
            catch(Exception ex) {
                _logger.LogError(ex, "Error parsing Shearwater CSV file.");
                return null;
            }
        }

        /// <summary>
        /// Maps a normalized column key to its 0-based index in the header row.
        /// Unknown columns are silently ignored.
        /// </summary>
        private static Dictionary<string, int> BuildColumnIndex(string[] headers) {
            var index = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            for(int i = 0; i < headers.Length; i++) {
                var normalized = NormalizeColName(headers[i]);
                if(!string.IsNullOrEmpty(normalized))
                    index.TryAdd(normalized, i);  // first occurrence wins on duplicate headers
            }
            return index;
        }

        /// <summary>
        /// Strips spaces, punctuation, and parenthesized units so "Max Depth (ft)"
        /// and "MaxDepth" both normalize to "MAXDEPTH".
        /// </summary>
        private static string NormalizeColName(string name) {
            var sb = new StringBuilder();
            foreach(char c in name.ToUpperInvariant()) {
                if(char.IsLetterOrDigit(c))
                    sb.Append(c);
            }
            return sb.ToString();
        }

        // Row mapping
        private ShearwaterDiveSummaryDto MapRow(string[] fields, Dictionary<string, int> idx) {
            var dto = new ShearwaterDiveSummaryDto();

            dto.DiveNumber = GetInt(fields, idx, "DIVENUMBER");
            dto.Descended = GetDateTime(fields, idx, "STARTDATE");
            dto.Surfaced = GetDateTime(fields, idx, "ENDDATE");
            dto.MaxDepth = GetInt(fields, idx, "MAXDEPTH");
            dto.SerialNumber = GetString(fields, idx, "COMPUTERSERIALNUMBER");
            dto.FirmwareVersion = GetString(fields, idx, "COMPUTERFIRMWAREVERSION");
            dto.CnsBeforePercent = GetInt(fields, idx, "STARTCNS");
            dto.CnsAfterPercent = GetInt(fields, idx, "ENDCNS");
            dto.BatteryVoltage = GetFloat(fields, idx, "ENDBATTERYVOLTAGE");
            dto.Product = GetString(fields, idx, "PRODUCT");
            var IsTrue = GetString(fields, idx, "IMPERIALUNITS");
            dto.IsImperial = IsTrue.Equals("TRUE", StringComparison.OrdinalIgnoreCase);
            dto.DiveMode = GetString(fields, idx, "MODE");

            // Max Time is in seconds; convert to rounded minutes
            int? durationSeconds = GetInt(fields, idx, "MAXTIME");
            if(durationSeconds.HasValue)
                dto.DurationMinutes = (int)Math.Round(durationSeconds.Value / 60.0);

            return dto;
        }

        // Field helpers — defensive getters that return null on any parse failure
        private static string? GetString(string[] fields, Dictionary<string, int> idx, string key) {
            if(idx.TryGetValue(key, out int col) && col < fields.Length) {
                var val = fields[col].Trim().Trim('"');
                if(!string.IsNullOrEmpty(val))
                    return val;
            }
            return null;
        }

        private static int? GetInt(string[] fields, Dictionary<string, int> idx, string key) {
            var raw = GetString(fields, idx, key);
            if(raw == null)
                return null;

            // Strip trailing "%" or unit suffixes (e.g. "100%", "82 ft")
            var cleaned = raw.Split(' ')[0].TrimEnd('%');
            if(int.TryParse(cleaned, out int result))
                return result;
            // Try parsing as double and rounding (e.g. "82.3")
            if(double.TryParse(cleaned, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out double dbl))
                return (int)Math.Round(dbl);
            return null;
        }

        private static float? GetFloat(string[] fields, Dictionary<string, int> idx, string key) {
            var raw = GetString(fields, idx, key);
            if(raw == null)
                return null;

            // Strip trailing "%" or unit suffixes (e.g. "1.6v", "82 ft")
            var cleaned = raw.Split(' ')[0].TrimEnd('%');
            if(float.TryParse(cleaned, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out float result))
                return result;

            return null;
        }

        private static DateTime? GetDateTime(string[] fields, Dictionary<string, int> idx, string key) {
            var raw = GetString(fields, idx, key);
            if(raw == null)
                return null;

            // Shearwater Cloud exports local/device time — do NOT convert to UTC.
            // Primary format: "2/18/2026 7:37:41 PM"
            string[] formats =
            {
                "M/d/yyyy h:mm:ss tt",
                "M/d/yyyy h:mm tt",
                "yyyy-MM-dd HH:mm:ss",
            };

            if(DateTime.TryParseExact(raw, formats,
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out DateTime result)) {
                return result;
            }

            // Fallback: try general parse
            if(DateTime.TryParse(raw, out DateTime fallback))
                return fallback;

            return null;
        }

        private static LogSampleDto MapSampleRow(string[] fields, Dictionary<string, int> idx) {
            return new LogSampleDto {
                ElapsedSeconds        = GetInt(fields, idx, "TIMESEC") ?? 0,
                Depth                 = GetDecimal(fields, idx, "DEPTH") ?? 0m,
                FirstDecoStopDepth    = GetDecimal(fields, idx, "FIRSTSTOPDEPTH") ?? 0m,
                TimeToSurfaceMinutes  = GetInt(fields, idx, "TIMETOSURFACEMIN") ?? 0,
                AvgPPO2               = GetDecimal(fields, idx, "AVERAGEPPO2") ?? 0m,
                FractionO2            = GetDecimal(fields, idx, "FRACTIONO2") ?? 0m,
                FractionHe            = GetDecimal(fields, idx, "FRACTIONHE") ?? 0m,
                FirstDecoStopMinutes  = GetInt(fields, idx, "FIRSTSTOPTIME") ?? 0,
                NoDecoLimitMinutes    = GetInt(fields, idx, "CURRENTNDL") ?? 0,
                CircuitMode           = (short)(GetInt(fields, idx, "CURRENTCIRCUITMODE") ?? 0),
                CCRMode               = (short)(GetInt(fields, idx, "CURRENTCCRMODE") ?? 0),
                Temperature           = GetInt(fields, idx, "WATERTEMP") ?? 0,
                GasSwitchNeeded       = GetBool(fields, idx, "GASSWITCHNEEDED"),
                ExternalPPO2Active    = GetBool(fields, idx, "EXTERNALPPO2"),
                SetPointType          = (short)(GetInt(fields, idx, "SETPOINTTYPE") ?? 0),
                CircuitSwitchType     = (short)(GetInt(fields, idx, "CIRCUITSWITCHTYPE") ?? 0),
                O2Sensor1Millivolts   = GetInt(fields, idx, "EXTERNALO2SENSOR1MV") ?? 0,
                O2Sensor2Millivolts   = GetInt(fields, idx, "EXTERNALO2SENSOR2MV") ?? 0,
                O2Sensor3Millivolts   = GetInt(fields, idx, "EXTERNALO2SENSOR3MV") ?? 0,
                BatteryVoltage        = GetDecimal(fields, idx, "BATTERYVOLTAGE") ?? 0m,
                AscentRate            = GetDecimal(fields, idx, "ASCENTRATE") ?? 0m,
                SafeAscentDepth       = GetDecimal(fields, idx, "SAFEASCENTDEPTH") ?? 0m,
                CO2Millibar           = GetInt(fields, idx, "CO2MBAR") ?? 0,
            };
        }

        private static decimal? GetDecimal(string[] fields, Dictionary<string, int> idx, string key) {
            var raw = GetString(fields, idx, key);
            if(raw == null) return null;
            var cleaned = raw.Split(' ')[0].TrimEnd('%');
            if(decimal.TryParse(cleaned, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out decimal result))
                return result;
            return null;
        }

        private static bool GetBool(string[] fields, Dictionary<string, int> idx, string key) {
            var raw = GetString(fields, idx, key);
            return string.Equals(raw, "true", StringComparison.OrdinalIgnoreCase);
        }

        // CSV line splitter — handles double-quoted fields with embedded commas
           private static string[] SplitCsvLine(string line) {
            var fields = new List<string>();
            var current = new StringBuilder();
            bool inQuotes = false;

            for(int i = 0; i < line.Length; i++) {
                char c = line[i];

                if(c == '"') {
                    // Handle escaped quotes ("")
                    if(inQuotes && i + 1 < line.Length && line[i + 1] == '"') {
                        current.Append('"');
                        i++;
                    } else {
                        inQuotes = !inQuotes;
                    }
                } else if(c == ',' && !inQuotes) {
                    fields.Add(current.ToString());
                    current.Clear();
                } else {
                    current.Append(c);
                }
            }

            fields.Add(current.ToString());
            return fields.ToArray();
        }
    }
}
