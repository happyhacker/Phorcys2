using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Phorcys.Web.ModelsNew;

public partial class Sql2022754043PhorcysContext : DbContext
{
    public Sql2022754043PhorcysContext()
    {
    }

    public Sql2022754043PhorcysContext(DbContextOptions<Sql2022754043PhorcysContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Attribute> Attributes { get; set; }

    public virtual DbSet<AttributeAssociation> AttributeAssociations { get; set; }

    public virtual DbSet<Certification> Certifications { get; set; }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Dife> Dives { get; set; }

    public virtual DbSet<DiveAgency> DiveAgencies { get; set; }

    public virtual DbSet<DiveLocation> DiveLocations { get; set; }

    public virtual DbSet<DivePlan> DivePlans { get; set; }

    public virtual DbSet<DiveShop> DiveShops { get; set; }

    public virtual DbSet<DiveSite> DiveSites { get; set; }

    public virtual DbSet<DiveSiteUrl> DiveSiteUrls { get; set; }

    public virtual DbSet<DiveTeam> DiveTeams { get; set; }

    public virtual DbSet<DiveType> DiveTypes { get; set; }

    public virtual DbSet<DiveUrl> DiveUrls { get; set; }

    public virtual DbSet<Diver> Divers { get; set; }

    public virtual DbSet<DiverCertification> DiverCertifications { get; set; }

    public virtual DbSet<DiverQualification> DiverQualifications { get; set; }

    public virtual DbSet<Friend> Friends { get; set; }

    public virtual DbSet<GasMix> GasMixes { get; set; }

    public virtual DbSet<Gase> Gases { get; set; }

    public virtual DbSet<Gear> Gears { get; set; }

    public virtual DbSet<GearServiceEvent> GearServiceEvents { get; set; }

    public virtual DbSet<Instructor> Instructors { get; set; }

    public virtual DbSet<InsurancePolicy> InsurancePolicies { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<Qualification> Qualifications { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServiceSchedule> ServiceSchedules { get; set; }

    public virtual DbSet<SoldGear> SoldGears { get; set; }

    public virtual DbSet<Tank> Tanks { get; set; }

    public virtual DbSet<TanksOnDive> TanksOnDives { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VwCertification> VwCertifications { get; set; }

    public virtual DbSet<VwCertificationInstructor> VwCertificationInstructors { get; set; }

    public virtual DbSet<VwDiveShop> VwDiveShops { get; set; }

    public virtual DbSet<VwDiver> VwDivers { get; set; }

    public virtual DbSet<VwInstructor> VwInstructors { get; set; }

    public virtual DbSet<VwTanksOnDive> VwTanksOnDives { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=sql2k2201.discountasp.net;Initial Catalog=SQL2022_754043_phorcys;User ID=SQL2022_754043_phorcys_user;Password=nnihuee;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.Property(e => e.RoleId).IsRequired();

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("trg_AfterInsert_AspNetUsers"));

            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.Property(e => e.UserId).IsRequired();

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(128);
            entity.Property(e => e.UserId).IsRequired();

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.Name).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Attribute>(entity =>
        {
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastModified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Notes).IsUnicode(false);
            entity.Property(e => e.TableToAssociate)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.Attributes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Users_Attributes");
        });

        modelBuilder.Entity<AttributeAssociation>(entity =>
        {
            entity.HasKey(e => new { e.AttributeId, e.TableRowId });

            entity.Property(e => e.Notes).IsUnicode(false);
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            entity.HasOne(d => d.Attribute).WithMany(p => p.AttributeAssociations)
                .HasForeignKey(d => d.AttributeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Attributes_AttributeAssociations");
        });

        modelBuilder.Entity<Certification>(entity =>
        {
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastModified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Notes).IsUnicode(false);
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            entity.HasOne(d => d.DiveAgency).WithMany(p => p.Certifications)
                .HasForeignKey(d => d.DiveAgencyId)
                .HasConstraintName("DiveAgencies_Certifications");

            entity.HasOne(d => d.User).WithMany(p => p.Certifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Users_Certifications");
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.Property(e => e.Address1)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.Address2)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.CellPhone)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Company)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.CountryCode)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .IsRequired()
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.HomePhone)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.LastModified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Notes).IsUnicode(false);
            entity.Property(e => e.PostalCode)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.State)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.WorkPhone)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.CountryCodeNavigation).WithMany(p => p.Contacts)
                .HasForeignKey(d => d.CountryCode)
                .HasConstraintName("Countries_Contacts");

            entity.HasOne(d => d.User).WithMany(p => p.Contacts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Users_Contacts");

            entity.HasMany(d => d.DiveShopsNavigation).WithMany(p => p.Contacts)
                .UsingEntity<Dictionary<string, object>>(
                    "DiveShopStaff",
                    r => r.HasOne<DiveShop>().WithMany()
                        .HasForeignKey("DiveShopId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("DiveShops_DiveShopStaff"),
                    l => l.HasOne<Contact>().WithMany()
                        .HasForeignKey("ContactId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Contacts_DiveShopStaff"),
                    j =>
                    {
                        j.HasKey("ContactId", "DiveShopId");
                        j.ToTable("DiveShopStaff");
                    });
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.CountryCode);

            entity.Property(e => e.CountryCode)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Dife>(entity =>
        {
            entity.HasKey(e => e.DiveId);

            entity.Property(e => e.AdditionalWeight).HasDefaultValue(0);
            entity.Property(e => e.AvgDepth).HasDefaultValue(0);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DescentTime).HasColumnType("datetime");
            entity.Property(e => e.LastModified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MaxDepth).HasDefaultValue(0);
            entity.Property(e => e.Minutes).HasDefaultValue(0);
            entity.Property(e => e.Notes)
                .IsRequired()
                .IsUnicode(false);
            entity.Property(e => e.Temperature).HasDefaultValue(0);
            entity.Property(e => e.Title)
                .HasMaxLength(40)
                .IsUnicode(false);

            entity.HasOne(d => d.DivePlan).WithMany(p => p.Dives)
                .HasForeignKey(d => d.DivePlanId)
                .HasConstraintName("Dives_DiveDetails");

            entity.HasOne(d => d.User).WithMany(p => p.Dives)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("Users_Dives");
        });

        modelBuilder.Entity<DiveAgency>(entity =>
        {
            entity.Property(e => e.Notes).IsUnicode(false);

            entity.HasOne(d => d.Contact).WithMany(p => p.DiveAgencies)
                .HasForeignKey(d => d.ContactId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Contacts_DiveAgencies");
        });

        modelBuilder.Entity<DiveLocation>(entity =>
        {
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastModified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Notes).IsUnicode(false);
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            entity.HasOne(d => d.Contact).WithMany(p => p.DiveLocations)
                .HasForeignKey(d => d.ContactId)
                .HasConstraintName("Contacts_DiveLocations");

            entity.HasOne(d => d.User).WithMany(p => p.DiveLocations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Users_DiveLocations");
        });

        modelBuilder.Entity<DivePlan>(entity =>
        {
            entity.HasIndex(e => e.DivePlanId, "IDX_DivePlans_DiveId");

            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastModified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MaxDepth).HasDefaultValue(0);
            entity.Property(e => e.Notes).IsUnicode(false);
            entity.Property(e => e.ScheduledTime).HasColumnType("datetime");
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            entity.HasOne(d => d.DiveSite).WithMany(p => p.DivePlans)
                .HasForeignKey(d => d.DiveSiteId)
                .HasConstraintName("DiveSites_Dives");

            entity.HasOne(d => d.User).WithMany(p => p.DivePlans)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Users_DivePlans");
        });

        modelBuilder.Entity<DiveShop>(entity =>
        {
            entity.Property(e => e.Notes).IsUnicode(false);

            entity.HasOne(d => d.Contact).WithMany(p => p.DiveShops)
                .HasForeignKey(d => d.ContactId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Contacts_DiveShops");

            entity.HasMany(d => d.Services).WithMany(p => p.DiveShops)
                .UsingEntity<Dictionary<string, object>>(
                    "DiveShopService",
                    r => r.HasOne<Service>().WithMany()
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Services_DiveShopServices"),
                    l => l.HasOne<DiveShop>().WithMany()
                        .HasForeignKey("DiveShopId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("DiveShops_DiveShopServices"),
                    j =>
                    {
                        j.HasKey("DiveShopId", "ServiceId");
                        j.ToTable("DiveShopServices");
                    });
        });

        modelBuilder.Entity<DiveSite>(entity =>
        {
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.GeoCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.LastModified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Notes).IsUnicode(false);
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            entity.HasOne(d => d.DiveLocation).WithMany(p => p.DiveSites)
                .HasForeignKey(d => d.DiveLocationId)
                .HasConstraintName("DiveLocations_DiveSites");

            entity.HasOne(d => d.User).WithMany(p => p.DiveSites)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Users_DiveSites");
        });

        modelBuilder.Entity<DiveSiteUrl>(entity =>
        {
            entity.Property(e => e.Title)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.Url)
                .IsRequired()
                .HasMaxLength(1024)
                .IsUnicode(false)
                .HasColumnName("URL");

            entity.HasOne(d => d.DiveSite).WithMany(p => p.DiveSiteUrls)
                .HasForeignKey(d => d.DiveSiteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DiveSites_DiveSiteUrls");
        });

        modelBuilder.Entity<DiveTeam>(entity =>
        {
            entity.HasKey(e => new { e.DivePlanId, e.DiverId }).HasName("PK_DiveTeam");

            entity.HasOne(d => d.DivePlan).WithMany(p => p.DiveTeams)
                .HasForeignKey(d => d.DivePlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Dives_DiveTeam");

            entity.HasOne(d => d.Diver).WithMany(p => p.DiveTeams)
                .HasForeignKey(d => d.DiverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Divers_DiveTeam");

            entity.HasOne(d => d.Role).WithMany(p => p.DiveTeams)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("Roles_DiveTeams");
        });

        modelBuilder.Entity<DiveType>(entity =>
        {
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastModified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Notes).IsUnicode(false);
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.DiveTypes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Users_DiveTypes");

            entity.HasMany(d => d.DivePlans).WithMany(p => p.DiveTypes)
                .UsingEntity<Dictionary<string, object>>(
                    "DivePlansDiveType",
                    r => r.HasOne<DivePlan>().WithMany()
                        .HasForeignKey("DivePlanId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Dives_DiveTypesXref"),
                    l => l.HasOne<DiveType>().WithMany()
                        .HasForeignKey("DiveTypeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("DiveTypes_DiveTypesXref"),
                    j =>
                    {
                        j.HasKey("DiveTypeId", "DivePlanId");
                        j.ToTable("DivePlansDiveTypes");
                    });
        });

        modelBuilder.Entity<DiveUrl>(entity =>
        {
            entity.HasKey(e => e.DiveUrlId).HasName("PK_ContentLinks");

            entity.Property(e => e.Title)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.Url)
                .IsRequired()
                .HasMaxLength(512)
                .IsUnicode(false)
                .HasColumnName("URL");

            entity.HasOne(d => d.Dive).WithMany(p => p.DiveUrls)
                .HasForeignKey(d => d.DiveId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DiveDetails_ContentLinks");
        });

        modelBuilder.Entity<Diver>(entity =>
        {
            entity.Property(e => e.Notes).IsUnicode(false);
            entity.Property(e => e.RestingSacRate).HasDefaultValue(0f);
            entity.Property(e => e.WorkingSacRate).HasDefaultValue(0.0);

            entity.HasOne(d => d.Contact).WithMany(p => p.Divers)
                .HasForeignKey(d => d.ContactId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Contacts_Divers");

            entity.HasMany(d => d.Gears).WithMany(p => p.Divers)
                .UsingEntity<Dictionary<string, object>>(
                    "DiverGear",
                    r => r.HasOne<Gear>().WithMany()
                        .HasForeignKey("GearId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Gear_DiverGear"),
                    l => l.HasOne<Diver>().WithMany()
                        .HasForeignKey("DiverId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Divers_DiverGear"),
                    j =>
                    {
                        j.HasKey("DiverId", "GearId");
                        j.ToTable("DiverGear");
                    });
        });

        modelBuilder.Entity<DiverCertification>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("N"));

            entity.Property(e => e.DiverCertificationId).HasComment("N");
            entity.Property(e => e.CertificationId).HasComment("N");
            entity.Property(e => e.CertificationNum)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasComment("N");
            entity.Property(e => e.Certified).HasComment("N");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasComment("N")
                .HasColumnType("datetime");
            entity.Property(e => e.DiverId).HasComment("N");
            entity.Property(e => e.InstructorId).HasComment("N");
            entity.Property(e => e.LastModified)
                .HasDefaultValueSql("(getdate())")
                .HasComment("N")
                .HasColumnType("datetime");
            entity.Property(e => e.Notes)
                .IsUnicode(false)
                .HasComment("N");

            entity.HasOne(d => d.Certification).WithMany(p => p.DiverCertifications)
                .HasForeignKey(d => d.CertificationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Certifications_DiverCertifications");

            entity.HasOne(d => d.Diver).WithMany(p => p.DiverCertifications)
                .HasForeignKey(d => d.DiverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Divers_DiverCertifications");

            entity.HasOne(d => d.Instructor).WithMany(p => p.DiverCertifications)
                .HasForeignKey(d => d.InstructorId)
                .HasConstraintName("Instructors_DiverCertifications");
        });

        modelBuilder.Entity<DiverQualification>(entity =>
        {
            entity.HasKey(e => new { e.DiverId, e.QualificationId });

            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastModified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Notes)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Diver).WithMany(p => p.DiverQualifications)
                .HasForeignKey(d => d.DiverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Divers_DiverQualifications");

            entity.HasOne(d => d.Qualification).WithMany(p => p.DiverQualifications)
                .HasForeignKey(d => d.QualificationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Qualifications_DiverQualifications");
        });

        modelBuilder.Entity<Friend>(entity =>
        {
            entity.HasKey(e => new { e.RequestorUserId, e.RecipientUserId, e.DateRequested });

            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.RecipientUser).WithMany(p => p.FriendRecipientUsers)
                .HasForeignKey(d => d.RecipientUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Users_Friends2");

            entity.HasOne(d => d.RequestorUser).WithMany(p => p.FriendRequestorUsers)
                .HasForeignKey(d => d.RequestorUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Users_Friends1");
        });

        modelBuilder.Entity<GasMix>(entity =>
        {
            entity.HasKey(e => new { e.DivePlanId, e.GearId, e.GasId }).HasName("PK__GasMixes__56FD4E221D9B5BB6");

            entity.ToTable(tb => tb.HasComment("N"));

            entity.Property(e => e.DivePlanId).HasComment("N");
            entity.Property(e => e.GearId).HasComment("N");
            entity.Property(e => e.GasId).HasComment("N");
            entity.Property(e => e.CostPerVolumeOfMeasure)
                .HasComment("N")
                .HasColumnType("money");
            entity.Property(e => e.Percentage).HasComment("N");
            entity.Property(e => e.VolumeAdded).HasComment("N");

            entity.HasOne(d => d.Gas).WithMany(p => p.GasMixes)
                .HasForeignKey(d => d.GasId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Gases_GasMixes");

            entity.HasOne(d => d.TanksOnDive).WithMany(p => p.GasMixes)
                .HasForeignKey(d => new { d.DivePlanId, d.GearId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TanksOnDive_GasMixes");
        });

        modelBuilder.Entity<Gase>(entity =>
        {
            entity.HasKey(e => e.GasId);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Gear>(entity =>
        {
            entity.ToTable("Gear");

            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastModified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Notes).IsUnicode(false);
            entity.Property(e => e.Paid)
                .HasDefaultValue(0m)
                .HasColumnType("money");
            entity.Property(e => e.RetailPrice)
                .HasDefaultValue(0m)
                .HasColumnType("money");
            entity.Property(e => e.Sn)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SN");
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Weight).HasDefaultValue(0.0);

            entity.HasOne(d => d.Manufacturer).WithMany(p => p.Gears)
                .HasForeignKey(d => d.ManufacturerId)
                .HasConstraintName("Manufacturers_Gear");

            entity.HasOne(d => d.PurchasedFromContact).WithMany(p => p.Gears)
                .HasForeignKey(d => d.PurchasedFromContactId)
                .HasConstraintName("Contacts_Gear");

            entity.HasOne(d => d.User).WithMany(p => p.Gears)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Users_Gear");

            entity.HasMany(d => d.DivePlans).WithMany(p => p.Gears)
                .UsingEntity<Dictionary<string, object>>(
                    "GearOnDive",
                    r => r.HasOne<DivePlan>().WithMany()
                        .HasForeignKey("DivePlanId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("DivePlans_GearOnDive"),
                    l => l.HasOne<Gear>().WithMany()
                        .HasForeignKey("GearId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Gear_GearOnDive"),
                    j =>
                    {
                        j.HasKey("GearId", "DivePlanId");
                        j.ToTable("GearOnDive");
                    });

            entity.HasMany(d => d.InsurancePolicies).WithMany(p => p.Gears)
                .UsingEntity<Dictionary<string, object>>(
                    "InsuredGear",
                    r => r.HasOne<InsurancePolicy>().WithMany()
                        .HasForeignKey("InsurancePolicyId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("InsurancePolicies_InsuredGear"),
                    l => l.HasOne<Gear>().WithMany()
                        .HasForeignKey("GearId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Gear_InsuredGear"),
                    j =>
                    {
                        j.HasKey("GearId", "InsurancePolicyId");
                        j.ToTable("InsuredGear");
                    });
        });

        modelBuilder.Entity<GearServiceEvent>(entity =>
        {
            entity.HasKey(e => e.GearServiceEventsId);

            entity.Property(e => e.GearServiceEventsId).ValueGeneratedNever();
            entity.Property(e => e.Cost)
                .HasDefaultValue(0m)
                .HasColumnType("money");
            entity.Property(e => e.Notes).IsUnicode(false);
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            entity.HasOne(d => d.Gear).WithMany(p => p.GearServiceEvents)
                .HasForeignKey(d => d.GearId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Gear_GearServiceEvents");
        });

        modelBuilder.Entity<Instructor>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("N"));

            entity.Property(e => e.InstructorId).HasComment("N");
            entity.Property(e => e.ContactId).HasComment("N");
            entity.Property(e => e.Notes)
                .IsUnicode(false)
                .HasComment("N");

            entity.HasOne(d => d.Contact).WithMany(p => p.Instructors)
                .HasForeignKey(d => d.ContactId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Contacts_Instructors");

            entity.HasMany(d => d.Certifications).WithMany(p => p.Instructors)
                .UsingEntity<Dictionary<string, object>>(
                    "CertificationInstructor",
                    r => r.HasOne<Certification>().WithMany()
                        .HasForeignKey("CertificationId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Certifications_CertificationInstructors"),
                    l => l.HasOne<Instructor>().WithMany()
                        .HasForeignKey("InstructorId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Instructors_CertificationInstructors"),
                    j =>
                    {
                        j.HasKey("InstructorId", "CertificationId");
                        j.ToTable("CertificationInstructors");
                    });
        });

        modelBuilder.Entity<InsurancePolicy>(entity =>
        {
            entity.Property(e => e.Deductible)
                .HasDefaultValue(0m)
                .HasColumnType("money");
            entity.Property(e => e.Notes).IsUnicode(false);
            entity.Property(e => e.ValueAmount)
                .HasDefaultValue(0m)
                .HasColumnType("money");

            entity.HasOne(d => d.Contact).WithMany(p => p.InsurancePolicies)
                .HasForeignKey(d => d.ContactId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Contacts_InsurancePolicies");
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasOne(d => d.Contact).WithMany(p => p.Manufacturers)
                .HasForeignKey(d => d.ContactId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Contacts_Manufactures");
        });

        modelBuilder.Entity<Qualification>(entity =>
        {
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastModified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Notes).IsUnicode(false);
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.Qualifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Users_Qualifications");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.Notes).IsUnicode(false);
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.Roles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Users_Roles");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastModified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Notes).IsUnicode(false);
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ServiceSchedule>(entity =>
        {
            entity.Property(e => e.Notes).IsUnicode(false);
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            entity.HasOne(d => d.Gear).WithMany(p => p.ServiceSchedules)
                .HasForeignKey(d => d.GearId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Gear_ServiceSchedules");
        });

        modelBuilder.Entity<SoldGear>(entity =>
        {
            entity.HasKey(e => e.GearId);

            entity.ToTable("SoldGear");

            entity.Property(e => e.GearId).ValueGeneratedNever();
            entity.Property(e => e.Amount)
                .HasDefaultValue(0m)
                .HasColumnType("money");
            entity.Property(e => e.Notes).IsUnicode(false);

            entity.HasOne(d => d.Gear).WithOne(p => p.SoldGear)
                .HasForeignKey<SoldGear>(d => d.GearId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Gear_SoldGear");

            entity.HasOne(d => d.SoldToContact).WithMany(p => p.SoldGears)
                .HasForeignKey(d => d.SoldToContactId)
                .HasConstraintName("Contacts_SoldGear");
        });

        modelBuilder.Entity<Tank>(entity =>
        {
            entity.HasKey(e => e.GearId);

            entity.Property(e => e.GearId).ValueGeneratedNever();
            entity.Property(e => e.Volume).HasDefaultValue(0);
            entity.Property(e => e.WorkingPressure).HasDefaultValue(0);

            entity.HasOne(d => d.Gear).WithOne(p => p.Tank)
                .HasForeignKey<Tank>(d => d.GearId)
                .HasConstraintName("Gear_Tanks");
        });

        modelBuilder.Entity<TanksOnDive>(entity =>
        {
            entity.HasKey(e => new { e.DivePlanId, e.GearId });

            entity.ToTable("TanksOnDive", tb => tb.HasComment("N"));

            entity.Property(e => e.DivePlanId).HasComment("N");
            entity.Property(e => e.GearId).HasComment("N");
            entity.Property(e => e.EndingPressure)
                .HasDefaultValue(0)
                .HasComment("N");
            entity.Property(e => e.FillCost)
                .HasComment("N")
                .HasColumnType("money");
            entity.Property(e => e.FillDate)
                .HasComment("N")
                .HasColumnType("datetime");
            entity.Property(e => e.GasContentTitle)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasComment("N");
            entity.Property(e => e.StartingPressure)
                .HasDefaultValue(0)
                .HasComment("N");

            entity.HasOne(d => d.DivePlan).WithMany(p => p.TanksOnDives)
                .HasForeignKey(d => d.DivePlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DivePlans_TanksOnDive");

            entity.HasOne(d => d.Gear).WithMany(p => p.TanksOnDives)
                .HasForeignKey(d => d.GearId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Tanks_TanksOnDive");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.LoginId, "TUC_Users_1").IsUnique();

            entity.Property(e => e.AspNetUserId).HasMaxLength(450);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastModified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LoginCount).HasDefaultValue(0);
            entity.Property(e => e.LoginId)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.AspNetUser).WithMany(p => p.Users)
                .HasForeignKey(d => d.AspNetUserId)
                .HasConstraintName("AspNetUsers_Users");

            entity.HasOne(d => d.Contact).WithMany(p => p.Users)
                .HasForeignKey(d => d.ContactId)
                .HasConstraintName("Contacts_Users");
        });

        modelBuilder.Entity<VwCertification>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwCertifications");

            entity.Property(e => e.Agency)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.CertificationNum)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.DiverFirstName)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.DiverLastName)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.InstructorFirstName)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.InstructorLastName)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwCertificationInstructor>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwCertificationInstructors");

            entity.Property(e => e.Agency)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.Certification)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.Company)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwDiveShop>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwDiveShops");

            entity.Property(e => e.Address1)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.Address2)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.CellPhone)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Company)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.CountryCode)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .IsRequired()
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.HomePhone)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Notes)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PostalCode)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ShopNotes)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.State)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.WorkPhone)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwDiver>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwDivers");

            entity.Property(e => e.Address1)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.Address2)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.CellPhone)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Company)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.CountryCode)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.DiverNotes)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .IsRequired()
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.HomePhone)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Notes)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PostalCode)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.State)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.WorkPhone)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwInstructor>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwInstructors");

            entity.Property(e => e.Agency)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.CountryCode)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.InstructorAgencyId)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwTanksOnDive>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwTanksOnDive");

            entity.Property(e => e.DiveTitle)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.Tank)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
