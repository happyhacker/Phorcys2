using System.Data;

namespace PhorcysDomain
{
    public class Dive
    {

            public int DiveId { get; set; }
            public required string Title { get; set; }
            public int? AdditionalWeight { get; set; }
            public int? AvgDepth { get; set; }
            public DateTime Created { get; set; }
            public DateTime? DescentTime { get; set; }
            public int DiveNumber { get; set; }
            public DateTime LastModified { get; set; }
            public int? MaxDepth { get; set; }
            public int? Minutes { get; set; }
            public string? Notes { get; set; }
            public int? Temperature { get; set; }
            //public DivePlan DivePlan { get; set; }
            //public Diver Diver { get; set; }
           // public Role Role { get; set; }
            //public User User { get; set; }

        }
    }
