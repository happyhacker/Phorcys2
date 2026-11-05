using Phorcys.Data.DTOs;
using Phorcys.Domain;
using Phorcys.Services;
using Xunit;

namespace Phorcys.Tests
{
	public class DivePlanDefaultsMergerTests
	{
		private static DivePlan MakePlan() => new DivePlan
		{
			Title = "Plan Title",
			ScheduledTime = new DateTime(2026, 5, 1, 9, 30, 0),
			Minutes = 45,
			MaxDepth = 80,
			AveragePO2 = 1.2m
		};

		private static ShearwaterDiveSummaryDto MakeSummary() => new ShearwaterDiveSummaryDto
		{
			DiveNumber = 12,
			Descended = new DateTime(2026, 4, 30, 14, 0, 0),
			DurationMinutes = 60,
			MaxDepth = 100,
			Samples = new List<LogSampleDto> { new LogSampleDto { AvgPPO2 = 1.1m } }
		};

		[Fact]
		public void ImportFillsField_PlanDoesNotOverwrite()
		{
			var imported = DivePlanDefaultsMerger.Parse(
				DivePlanDefaultsMerger.Serialize(DivePlanDefaultsMerger.GetImportedFields(MakeSummary())));

			var result = DivePlanDefaultsMerger.Merge(MakePlan(), imported);

			Assert.DoesNotContain(DivePlanDefaultsMerger.Fields.DescentTime, result.FieldsToApply);
			Assert.DoesNotContain(DivePlanDefaultsMerger.Fields.Minutes, result.FieldsToApply);
			Assert.DoesNotContain(DivePlanDefaultsMerger.Fields.MaxDepth, result.FieldsToApply);
			Assert.DoesNotContain(DivePlanDefaultsMerger.Fields.AveragePO2, result.FieldsToApply);
			// Title is plan-only, so the plan still fills it.
			Assert.Contains("Title", result.FieldsToApply);
		}

		[Fact]
		public void ImportLeavesFieldEmpty_PlanFillsIt()
		{
			// CSV supplied only duration; everything else is empty.
			var summary = new ShearwaterDiveSummaryDto { DurationMinutes = 60 };
			var imported = DivePlanDefaultsMerger.Parse(
				DivePlanDefaultsMerger.Serialize(DivePlanDefaultsMerger.GetImportedFields(summary)));

			var result = DivePlanDefaultsMerger.Merge(MakePlan(), imported);

			Assert.DoesNotContain(DivePlanDefaultsMerger.Fields.Minutes, result.FieldsToApply);
			Assert.Contains(DivePlanDefaultsMerger.Fields.DescentTime, result.FieldsToApply);
			Assert.Contains(DivePlanDefaultsMerger.Fields.MaxDepth, result.FieldsToApply);
			Assert.Contains(DivePlanDefaultsMerger.Fields.AveragePO2, result.FieldsToApply);
			Assert.Equal("2026-05-01T09:30", result.DescentTime);
			Assert.Equal(80, result.MaxDepth);
			Assert.Equal(1.2m, result.AveragePO2);
		}

		[Fact]
		public void NoImport_PlanFillsEverything()
		{
			var result = DivePlanDefaultsMerger.Merge(MakePlan(), DivePlanDefaultsMerger.Parse(null));

			Assert.Equal(5, result.FieldsToApply.Count);
		}

		[Fact]
		public void ImportedZeroAndDefaultDateTime_AreTreatedAsImported()
		{
			var summary = new ShearwaterDiveSummaryDto
			{
				DurationMinutes = 0,
				MaxDepth = 0,
				Descended = default(DateTime)
			};

			var fields = DivePlanDefaultsMerger.GetImportedFields(summary);
			Assert.Contains(DivePlanDefaultsMerger.Fields.Minutes, fields);
			Assert.Contains(DivePlanDefaultsMerger.Fields.MaxDepth, fields);
			Assert.Contains(DivePlanDefaultsMerger.Fields.DescentTime, fields);

			var result = DivePlanDefaultsMerger.Merge(MakePlan(), DivePlanDefaultsMerger.Parse(DivePlanDefaultsMerger.Serialize(fields)));
			Assert.DoesNotContain(DivePlanDefaultsMerger.Fields.Minutes, result.FieldsToApply);
			Assert.DoesNotContain(DivePlanDefaultsMerger.Fields.MaxDepth, result.FieldsToApply);
			Assert.DoesNotContain(DivePlanDefaultsMerger.Fields.DescentTime, result.FieldsToApply);
		}

		[Fact]
		public void ImportedFields_SurviveSerializeParseRoundTrip()
		{
			var fields = new[] { "Minutes", "MaxDepth" };

			var parsed = DivePlanDefaultsMerger.Parse(DivePlanDefaultsMerger.Serialize(fields));

			Assert.Equal(2, parsed.Count);
			Assert.Contains("minutes", parsed); // case-insensitive
			Assert.Contains("MaxDepth", parsed);
		}

		[Fact]
		public void Parse_HandlesNullEmptyAndBlankEntries()
		{
			Assert.Empty(DivePlanDefaultsMerger.Parse(null));
			Assert.Empty(DivePlanDefaultsMerger.Parse(""));
			Assert.Single(DivePlanDefaultsMerger.Parse(" Minutes ,, "));
		}
	}
}
