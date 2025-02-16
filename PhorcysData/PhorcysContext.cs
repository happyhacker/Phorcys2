using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Phorcys.Domain;

namespace Phorcys.Data;

public class PhorcysContext : IdentityDbContext<IdentityUser>
{
	public PhorcysContext(DbContextOptions<PhorcysContext> options)
		: base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
		// Customize the ASP.NET Identity model and override the defaults if needed.
		// For example, you can rename the ASP.NET Identity table names and more.
		// Add your customizations after calling base.OnModelCreating(builder);
		// Define the primary key for vwMyCertification
		builder.Entity<vwMyCertification>()
			.HasKey(c => c.DiverCertificationId);
		builder.Entity<AgencyInstructor>().HasKey(a => a.InstructorId);

	}

	public DbSet<Dive> Dives { get; set; }
	public DbSet<DivePlan> DivePlans { get; set; }
	public DbSet<User> Users { get; set; }
	public DbSet<Contact> Contacts { get; set; }
	public DbSet<Country> Countries { get; set; }
	public DbSet<DiveSite> DiveSites { get; set; }
	public DbSet<DiveLocation> DiveLocations { get; set; }
	public DbSet<Gear> Gear { get; set; }
    public DbSet<Tank> Tanks { get; set; }
	public DbSet<vwMyCertification> vwMyCertifications { get; set; }
	public DbSet<DiverCertification> DiverCertifications { get; set;}
	public DbSet<DiveAgency> DiveAgencies { get; set; }
	public DbSet<Certification> Certifications { get; set; }
	public DbSet<Instructor> Instructors { get; set; }
	public DbSet<Diver> Divers { get; set; }
}
