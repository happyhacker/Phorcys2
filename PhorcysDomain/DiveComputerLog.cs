using System.ComponentModel.DataAnnotations.Schema;

namespace Phorcys.Domain
{
    /// <summary>
    /// Records metadata imported from a dive computer log file.
    /// Linked one-to-many from Dive (a dive may have logs from multiple computers in future).
    /// </summary>
    public class DiveComputerLog
    {
        public int DiveComputerLogId { get; set; }

        public int? DiveId { get; set; }

        /// <summary>
        /// Matched Gear item for this computer by serial number.
        /// NOTE: The DiveComputerLogs table does not currently have a GearId column.
        /// This property is not persisted. To enable persistence, add a GearId column
        /// (INT NULL, FK to Gear.GearId) to the DiveComputerLogs table.
        /// The serial number (SerialNumber) is stored and can be used to re-derive the match.
        /// </summary>
        [NotMapped]
        public int? GearId { get; set; }

        public string? Vendor { get; set; }

        public string? Product { get; set; }

        public string? Model { get; set; }

        public string? SerialNumber { get; set; }

        /// <summary>
        /// Maps to the "FirmareVersion" column in DiveComputerLogs (typo in schema preserved).
        /// </summary>
        [Column("FirmareVersion")]
        public string? FirmwareVersion { get; set; }

        public int? DiveNumber { get; set; }

        public DateTime ImportedDateTime { get; set; }

        /// <summary>
        /// O2 CNS percentage at the start of the dive. Maps to the "StartCNS" column.
        /// </summary>
        [Column("StartCNS")]
        public int? CnsBeforePercent { get; set; }

        /// <summary>
        /// O2 CNS percentage at the end of the dive. Maps to the "EndCNS" column.
        /// </summary>
        [Column("EndCNS")]
        public int? CnsAfterPercent { get; set; }

        /// <summary>
        /// Battery voltage/level at the end of the dive. Maps to the "BatteryVoltageEnd" column.
        /// </summary>
        [Column("BatteryVoltageEnd")]
        public float? BatteryVoltage { get; set; }

        public string? DiveMode { get; set; }

        public bool? IsEmperial { get; set; }

        /// <summary>Timestamp when the diver descended (dive start). Maps to "Start Date" in the CSV.</summary>
        public DateTime? Descended { get; set; }

        /// <summary>Timestamp when the diver surfaced (dive end). Maps to "End Date" in the CSV.</summary>
        public DateTime? Surfaced { get; set; }

        public int? MaxDepth { get; set; }

        public int? Minutes { get; set; }

        public virtual Dive? Dive { get; set; }
    }
}
