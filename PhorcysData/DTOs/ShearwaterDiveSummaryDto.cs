namespace Phorcys.Data.DTOs
{
    /// <summary>
    /// Holds the parsed summary data from a single Shearwater CSV export row.
    /// Used to prefill the Create Dive Log form and later populate a DiveComputerLog record.
    /// </summary>
    public class ShearwaterDiveSummaryDto
    {
        /// <summary>Dive number as reported by the dive computer.</summary>
        public int? DiveNumber { get; set; }

        /// <summary>
        /// Dive start time in local/device time (as exported by Shearwater Cloud).
        /// Do NOT convert to UTC — store and display as-is.
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Dive duration in whole minutes, rounded from seconds.
        /// Example: 3599 seconds → 60 minutes.
        /// </summary>
        public int? DurationMinutes { get; set; }

        /// <summary>Max depth in the unit reported by the dive computer (ft or m).</summary>
        public int? MaxDepth { get; set; }

        /// <summary>Dive computer serial number, used for Gear matching.</summary>
        public string? SerialNumber { get; set; }

        /// <summary>Dive computer product/model name (e.g. "Perdix 2").</summary>
        public string? Product { get; set; }

        /// <summary>Firmware version string.</summary>
        public string? FirmwareVersion { get; set; }

        /// <summary>O2 CNS percentage at the start of the dive.</summary>
        public int? CnsBeforePercent { get; set; }

        /// <summary>O2 CNS percentage at the end of the dive.</summary>
        public int? CnsAfterPercent { get; set; }

        /// <summary>Battery level/voltage at end of dive (as integer percent or voltage).</summary>
        public float? BatteryVoltage { get; set; }
    }
}
