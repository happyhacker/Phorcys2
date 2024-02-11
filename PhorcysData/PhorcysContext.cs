using Microsoft.EntityFrameworkCore;
using Phorcys.Domain;

namespace Phorcys.Data;

public class PhorcysContext : DbContext
{
	public DbSet<Dive> Dives { get; set; }
	public DbSet<DivePlan> DivePlans { get; set; }
	public DbSet<User> Users { get; set; }
	public DbSet<Contact> Contacts { get; set; }
	public DbSet<DiveSite> DiveSites { get; set; }
	public DbSet<DiveLocation> Locations { get; set; }
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlServer("Data Source=sql2k1401.discountasp.net;Initial Catalog=SQL2014_754043_larryhack;User Id=SQL2014_754043_larryhack_user;password=nnihuee")
			 //optionsBuilder.UseSqlServer("Data Source=HACKSOFT\\MSSQLSERVER01;Initial Catalog=SCUBA;User Id=sheckexley;password=Measureless2Man;TrustServerCertificate=True;")
			 .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

	}
}
