using Microsoft.AspNetCore.Mvc.Rendering;
using Phorcys.Data.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Phorcys.Web.Models
{
    public class DiveViewModel
    {
        public int DiveId { get; set; }

		[DisplayName("Dive Plan title")]
		public string DivePlanTitle { get; set; }
		
		[DisplayName("Title")]
		[Required]
        [MaxLength(60, ErrorMessage = "The Title cannot exceed 60 characters.")]
        public string Title { get; set; }
        public IList<SelectListItem> DivePlanList { get; set; }
        public int? DivePlanSelectedId { get; set; }

		[DisplayName("Dive Site")]
		public string DiveSite { get; set; }

		[DisplayName("User Name")]
		public string UserName { get; set; }

		[Range(0, int.MaxValue, ErrorMessage = "Please enter a valid number.")]

		[DisplayName("Minutes")] 
        public int? Minutes { get; set; }

		[DisplayName("Descent Time")]
		public DateTime? DescentTime { get; set; }

		[DisplayName("Average Depth")]
		public int? AvgDepth { get; set; }

        [DisplayName("Max Depth")]
        [Range(0,1100, ErrorMessage = "You didn't really go this deep.")]
        public int? MaxDepth { get; set; }  

		[DisplayName("temperature")]
		public int? Temperature { get; set; }

		public List<TanksOnDiveDto> Tanks { get; set; }

		[DisplayName("Additional Weight")]
		public int? AdditionalWeight { get; set; }

        public string DiveBuddies { get; set; } = string.Empty;

        [DisplayName("Notes")]
		public string Notes { get; set; }

		[DisplayName("Dive #")]
		[Range(0, int.MaxValue, ErrorMessage = "Please enter a valid number.")]
		public int DiveNumber { get; set; }

        [DisplayName("UserId")]
        public int? UserId { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastModified { get; set; }

        // -----------------------------------------------------------------------
        // Shearwater CSV import — populated after upload, stored as hidden fields,
        // used to create a DiveComputerLog record when the dive is saved.
        // -----------------------------------------------------------------------

        /// <summary>True when a Shearwater CSV has been uploaded and parsed for this form session.</summary>
        public bool HasImportedData { get; set; }

        /// <summary>Serial number from the imported CSV; used for Gear matching and DiveComputerLog.</summary>
        public string ImportedSerialNumber { get; set; }

        /// <summary>Computer product/model name from the imported CSV (e.g. "Perdix 2").</summary>
        public string ImportedProduct { get; set; }

        /// <summary>Firmware version string from the imported CSV.</summary>
        public string ImportedFirmwareVersion { get; set; }

        /// <summary>O2 CNS % at dive start from the imported CSV.</summary>
        public int? ImportedCnsBeforePercent { get; set; }

        /// <summary>O2 CNS % at dive end from the imported CSV.</summary>
        public int? ImportedCnsAfterPercent { get; set; }

        /// <summary>Battery level/voltage from the imported CSV.</summary>
        public float? ImportedBatteryVoltage { get; set; }

        /// <summary>Dive mode (e.g. OC, CC, Gauge) from the imported CSV.</summary>
        public string? ImportedDiveMode { get; set; }

        /// <summary>True when the dive computer used imperial units, from the imported CSV.</summary>
        public bool? ImportedIsImperial { get; set; }

        /// <summary>Dive start timestamp from the imported CSV (local/device time).</summary>
        public DateTime? ImportedDescended { get; set; }

        /// <summary>Dive end timestamp from the imported CSV (local/device time).</summary>
        public DateTime? ImportedSurfaced { get; set; }
    }
}
