using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Phorcys.Domain;
using System.Reflection.Emit;

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

		builder.Entity<Contact>()
	 .HasOne(c => c.Diver)
	 .WithOne(d => d.Contact)
	 .HasForeignKey<Diver>(d => d.ContactId)
	 .OnDelete(DeleteBehavior.Cascade); // Delete Diver if Contact is deleted

		builder.Entity<Contact>()
			.HasOne(c => c.Instructor)
			.WithOne(i => i.Contact)
			.HasForeignKey<Instructor>(i => i.ContactId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.Entity<Contact>()
			.HasOne(c => c.DiveShop)
			.WithOne(ds => ds.Contact)
			.HasForeignKey<DiveShop>(ds => ds.ContactId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.Entity<Contact>()
			.HasOne(c => c.Manufacturer)
			.WithOne(m => m.Contact)
			.HasForeignKey<Manufacturer>(m => m.ContactId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.Entity<Contact>()
			.HasOne(c => c.DiveAgency)
			.WithOne(da => da.Contact)
			.HasForeignKey<DiveAgency>(da => da.ContactId)
			.OnDelete(DeleteBehavior.Cascade);

		// DivePlan <-> DiveTypes
		builder.Entity<DivePlan>()
			.HasMany(dp => dp.DiveTypes)
			.WithMany(dt => dt.DivePlans)
			.UsingEntity<Dictionary<string, object>>(
				"DivePlanDiveType",
				j => j
					.HasOne<DiveType>()
					.WithMany()
					.HasForeignKey("DiveTypeId")
					.OnDelete(DeleteBehavior.Cascade),
				j => j
					.HasOne<DivePlan>()
					.WithMany()
					.HasForeignKey("DivePlanId")
					.OnDelete(DeleteBehavior.Cascade),
				j =>
				{
					j.HasKey("DivePlanId", "DiveTypeId");
					j.ToTable("DivePlansDiveTypes");
				});

		builder.Entity<DivePlan>()
			.HasMany(dp => dp.Gears)
			.WithMany(g => g.DivePlans)
			.UsingEntity<Dictionary<string, object>>(
		"DivePlanGear",
		j => j
			.HasOne<Gear>()
			.WithMany()
			.HasForeignKey("GearId")
			.OnDelete(DeleteBehavior.Cascade),
		j => j
			.HasOne<DivePlan>()
			.WithMany()
			.HasForeignKey("DivePlanId")
			.OnDelete(DeleteBehavior.Cascade),
		j =>
		{
			j.HasKey("DivePlanId", "GearId");
			j.ToTable("DivePlanGear");
		});

		builder.Entity<TanksOnDive>()
			.HasKey(t => new { t.DivePlanId, t.GearId });

		builder.Entity<TanksOnDive>()
			.HasOne(t => t.DivePlan)
			.WithMany(dp => dp.TanksOnDives)
			.HasForeignKey(t => t.DivePlanId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.Entity<TanksOnDive>()
			.HasOne(t => t.Tank)
			.WithMany()
			.HasForeignKey(t => t.GearId)  // This connects to the GearId in Tank
			.OnDelete(DeleteBehavior.Cascade);

	}

	public DbSet<DiveType> DiveTypes { get; set; }
	public DbSet<DiveShop> DiveShops { get; set; }
	public DbSet<Dive> Dives { get; set; }
	public DbSet<DivePlan> DivePlans { get; set; }
	public DbSet<User> Users { get; set; }
	public DbSet<Contact> Contacts { get; set; }
	public DbSet<Country> Countries { get; set; }
	public DbSet<DiveSite> DiveSites { get; set; }
	public DbSet<DiveLocation> DiveLocations { get; set; }
	public DbSet<Gear> Gear { get; set; }
    public DbSet<Tank> Tanks { get; set; }
	public DbSet<TanksOnDive> TanksOnDives { get; set; }
	public DbSet<vwMyCertification> vwMyCertifications { get; set; }
	public DbSet<DiverCertification> DiverCertifications { get; set;}
	public DbSet<DiveAgency> DiveAgencies { get; set; }
	public DbSet<Certification> Certifications { get; set; }
	public DbSet<Instructor> Instructors { get; set; }
	public DbSet<Diver> Divers { get; set; }
	public DbSet<Manufacturer> Manufacturers { get; set; }
}
