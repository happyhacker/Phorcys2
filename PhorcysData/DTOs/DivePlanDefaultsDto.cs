namespace Phorcys.Data.DTOs
{
    /// <summary>
    /// Values a selected Dive Plan offers to the Create Dive Log form, plus the list of
    /// fields the plan is actually allowed to apply (fields the Shearwater import already
    /// populated are excluded — import data always wins).
    /// </summary>
    public class DivePlanDefaultsDto
    {
        public string? Title { get; set; }
        public string? DescentTime { get; set; }
        public int? Minutes { get; set; }
        public int? MaxDepth { get; set; }
        public decimal? AveragePO2 { get; set; }

        /// <summary>Names of the fields (see DivePlanDefaultsMerger.Fields) the client may overwrite.</summary>
        public List<string> FieldsToApply { get; set; } = new List<string>();
    }
}
