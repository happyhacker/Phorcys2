using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using Phorcys.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Phorcys.Data;

public class PhorcysContext : IdentityDbContext
{

	public PhorcysContext(DbContextOptions<PhorcysContext> options)
	 : base(options)
	{
	}

	public DbSet<Dive> Dives { get; set; }
	public DbSet<DivePlan> DivePlans { get; set; }
	public DbSet<User> Users { get; set; }
	public DbSet<Contact> Contacts { get; set; }
	public DbSet<DiveSite> DiveSites { get; set; }
	public DbSet<DiveLocation> Locations { get; set; }
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		//optionsBuilder.UseSqlServer("Data Source=sql2k1401.discountasp.net;Initial Catalog=SQL2014_754043_larryhack;User Id=SQL2014_754043_larryhack_user;password=nnihuee")
		optionsBuilder.UseSqlServer("Data Source=HACKSOFT\\MSSQLSERVER01;Initial Catalog=SCUBA;User Id=sheckexley;password=Measureless2Man;TrustServerCertificate=True;")
		//optionsBuilder.UseSqlServer("Server=tcp:hacksoft.database.windows.net,1433;Initial Catalog=Phorcys2;Persist Security Info=False;User ID=larry.hack;Password=Brenda1964!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;")
			.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

	}
}
