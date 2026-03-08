using System.ComponentModel;

namespace Phorcys.Web.Models
{
    public class DiveCompareViewModel
    {
        [DisplayName("Field")]
        public string Field { get; set; }

        [DisplayName("Planned")]
        public string Planned { get; set; }

        [DisplayName("Actual")]
        public string Actual { get; set; }

        public bool IsMatch { get; set; }

        public bool HasPlanValue { get; set; }
    }
}
