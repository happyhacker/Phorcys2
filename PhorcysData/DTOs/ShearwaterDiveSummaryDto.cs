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
        /// Timestamp when the diver descended (dive start), in local/device time.
        /// Do NOT convert to UTC — store and display as-is.
        /// </summary>
        public DateTime? Descended { get; set; }

        /// <summary>
        /// Timestamp when the diver surfaced (dive end), in local/device time.
        /// Do NOT convert to UTC — store and display as-is.
        /// </summary>
        public DateTime? Surfaced { get; set; }

        /// <summary>Dive mode (e.g. OC, CC, Gauge) as reported by the dive computer.</summary>
        public string? DiveMode { get; set; }

        /// <summary>True when the dive computer was configured to use imperial units.</summary>
        public bool? IsImperial { get; set; }

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
