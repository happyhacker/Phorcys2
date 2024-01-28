using Microsoft.EntityFrameworkCore;
using Phorcys.Domain;
using Phorcys.Services;


var diveServices = new DiveServices();
var dives = diveServices.GetDives();


foreach (var dive in dives)
{
	Console.Write("Dive # " + dive.DiveNumber + ",");
	if (dive.DivePlan != null)
	{
		Console.Write("Title: " + dive.DivePlan.Title + "," ?? ",");
	}
	Console.Write("Depth: " + dive.AvgDepth + ",");
	Console.WriteLine("Time: " + dive.DescentTime + "");
}