using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Phorcys.Domain;

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
}
