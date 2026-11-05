using Phorcys.Data.DTOs;
using Phorcys.Domain;

namespace Phorcys.Services;

/// <summary>
/// The single place that decides how a Dive Plan and a Shearwater import share the
/// Create Dive Log form. Rule: import data always wins; the plan only fills fields the
/// import did not populate.
///
/// "Imported" is tracked by field name (not by value), so an imported 0 / false /
/// default value is never mistaken for "empty".
/// </summary>
public static class DivePlanDefaultsMerger
{
	/// <summary>Field names that both the import and a Dive Plan can set.</summary>
	public static class Fields
	{
		public const string DescentTime = "DescentTime";
		public const string Minutes = "Minutes";
		public const string MaxDepth = "MaxDepth";
		public const string AveragePO2 = "AveragePO2";
	}

	private const char Separator = ',';

	/// <summary>
	/// Returns the shared fields the import actually populated (it only reports a field
	/// when the CSV supplied a value for it, even if that value is 0).
	/// </summary>
	public static List<string> GetImportedFields(ShearwaterDiveSummaryDto summary)
	{
		var imported = new List<string>();
		if (summary == null) return imported;

		if (summary.Descended.HasValue) imported.Add(Fields.DescentTime);
		if (summary.DurationMinutes.HasValue) imported.Add(Fields.Minutes);
		if (summary.MaxDepth.HasValue) imported.Add(Fields.MaxDepth);
		if (summary.Samples != null && summary.Samples.Count > 0) imported.Add(Fields.AveragePO2);
		return imported;
	}

	/// <summary>Serializes the imported field names for a hidden form field.</summary>
	public static string Serialize(IEnumerable<string> importedFields)
		=> string.Join(Separator, importedFields ?? Enumerable.Empty<string>());

	/// <summary>Parses the hidden form field back into a case-insensitive set.</summary>
	public static HashSet<string> Parse(string? serialized)
	{
		var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		if (string.IsNullOrWhiteSpace(serialized)) return set;

		foreach (var part in serialized.Split(Separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
			set.Add(part);
		return set;
	}

	/// <summary>
	/// Builds the plan's defaults and lists which fields the plan may apply.
	/// Title is plan-only (the import never sets it), so it is always applied.
	/// </summary>
	public static DivePlanDefaultsDto Merge(DivePlan plan, ISet<string>? importedFields)
	{
		importedFields ??= new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		var result = new DivePlanDefaultsDto
		{
			Title = plan.Title,
			DescentTime = plan.ScheduledTime.ToString("yyyy-MM-ddTHH:mm"),
			Minutes = plan.Minutes,
			MaxDepth = plan.MaxDepth,
			AveragePO2 = plan.AveragePO2
		};

		result.FieldsToApply.Add("Title");
		foreach (var field in new[] { Fields.DescentTime, Fields.Minutes, Fields.MaxDepth, Fields.AveragePO2 })
		{
			if (!importedFields.Contains(field))
				result.FieldsToApply.Add(field);
		}
		return result;
	}
}
