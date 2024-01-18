using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhorcysDomain
{
    public class DivePlan
    {
        public  DateTime Created { get; set; }
        public  int DivePlanId { get; set; }
        public  int DiveSiteId { get; set; }
        public  DateTime LastModified { get; set; }
        public  int MaxDepth { get; set; }
        public  string? Notes { get; set; }
        public  DateTime ScheduledTime { get; set; }
        public required string Title { get; set; }
        public  ISet<Dive>? Dives { get; set; }
        //public  ISet<Diver> Divers { get; set; }
       // public  DiveSite DiveSite { get; set; }
       //public  List<DiveType> DiveTypes { get; set; }
       // public  User User { get; set; }

    }
}
