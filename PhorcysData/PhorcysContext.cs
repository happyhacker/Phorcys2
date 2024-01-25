using Microsoft.EntityFrameworkCore;
using Phorcys.Domain;

namespace Phorcys.Data;

    public class PhorcysContext: DbContext
    {
        public DbSet<Dive> Dives { get; set; }
        public DbSet<DivePlan> DivePlans { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        //optionsBuilder.UseSqlServer("Data Source=HACKSOFT\\MSSQLSERVER01;Initial Catalog=SCUBA;Integrated Security=SSPI;Encrypt=False;")
        optionsBuilder.UseSqlServer("Data Source=HACKSOFT\\MSSQLSERVER01;Initial Catalog=SCUBA;User Id=sheckexley;password=Measureless2Man;TrustServerCertificate=True;")
             .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
 
    }
}
