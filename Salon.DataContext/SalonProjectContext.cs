using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Salon.DataContext;

public partial class SalonProjectContext : DbContext
{
    public SalonProjectContext()
    {
    }

    public SalonProjectContext(DbContextOptions<SalonProjectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<ClientManagment> ClientManagments { get; set; }

    public virtual DbSet<JobTitle> JobTitles { get; set; }

    public virtual DbSet<PersInfoClient> PersInfoClients { get; set; }

    public virtual DbSet<PriceList> PriceLists { get; set; }

    public virtual DbSet<Shift> Shifts { get; set; }

    public virtual DbSet<Worker> Workers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=.\\sqlexpress;Initial Catalog=SalonProject;Integrated Security=true;User ID=User;Password=***;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

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

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.Property(e => e.BookingId).ValueGeneratedNever();
            entity.Property(e => e.EmpleyeeId)
                .HasDefaultValueSql("(N'Уволенный сотрудник')")
                .IsFixedLength();

            entity.HasOne(d => d.Client).WithMany(p => p.Bookings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Booking_AspNetUsers");

            entity.HasOne(d => d.Empleyee).WithMany(p => p.Bookings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Booking_Workers");

            entity.HasOne(d => d.Service).WithMany(p => p.Bookings).HasConstraintName("FK_Booking_PriceList");
        });

        modelBuilder.Entity<JobTitle>(entity =>
        {
            entity.Property(e => e.PositionId).ValueGeneratedNever();
            entity.Property(e => e.Position).IsFixedLength();
        });

        modelBuilder.Entity<PersInfoClient>(entity =>
        {
            entity.Property(e => e.ClientId).ValueGeneratedNever();
            entity.Property(e => e.Email).IsFixedLength();
            entity.Property(e => e.FullName).IsFixedLength();
            entity.Property(e => e.TelNumber).IsFixedLength();
        });

        modelBuilder.Entity<PriceList>(entity =>
        {
            entity.Property(e => e.ServiceId).ValueGeneratedNever();
            entity.Property(e => e.Service).IsFixedLength();
        });

        modelBuilder.Entity<Shift>(entity =>
        {
            entity.Property(e => e.ShiftId).ValueGeneratedNever();
            entity.Property(e => e.WorkDays).IsFixedLength();
        });

        modelBuilder.Entity<Worker>(entity =>
        {
            entity.Property(e => e.PatentId).IsFixedLength();
            entity.Property(e => e.FullName).IsFixedLength();
            entity.Property(e => e.TelNumber).IsFixedLength();

            entity.HasOne(d => d.Position).WithMany(p => p.Workers)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Workers_JobTitles");

            entity.HasOne(d => d.Shift).WithMany(p => p.Workers)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Workers_Shifts");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
