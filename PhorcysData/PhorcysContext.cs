using Microsoft.EntityFrameworkCore;
using PhorcysDomain;

namespace PhorcysData
{

    public class PhorcysContext: DbContext
    {
        public DbSet<Dive> Dives { get; set; }
        public DbSet<DivePlan> DivePlans { get; set; }

    }
}
