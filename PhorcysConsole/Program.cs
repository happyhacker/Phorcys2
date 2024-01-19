using Microsoft.EntityFrameworkCore;
using Phorcys.Data;
using Phorcys.Domain;

using (PhorcysContext context = new PhorcysContext())
{

}

GetDives();

void GetDives()
{
    using var context = new PhorcysContext();
    //var dives = context.Dives.ToList(); Lazy loading not working
    var dives = context.Dives.Include(dive => dive.DivePlan).ToList();
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
}