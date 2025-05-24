using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ZealandZooCase.Models;

namespace ZealandZooCase.Data;

public partial class ZealandDBContext : DbContext
{
    public ZealandDBContext()
    {
    }

    public ZealandDBContext(DbContextOptions<ZealandDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<AllEventSignup> AllEventSignups { get; set; }

    public virtual DbSet<OurEvent> AllOurEvents { get; set; }

    public virtual DbSet<OpenHour> OpenHours { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<ZipCode> ZipCodes { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK__Addresse__CAA247C8615B913F");

            entity.Property(e => e.AddressPostalcode).IsFixedLength();

            entity.HasOne(d => d.AddressPostalcodeNavigation).WithMany(p => p.Addresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Addresses_ZipCodes");
        });

        modelBuilder.Entity<AllEventSignup>(entity =>
        {
            entity.HasKey(e => new { e.EventId, e.UserId }).HasName("PK__allEvent__C8EB1457B772B2D0");

            entity.Property(e => e.SignupDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Event).WithMany(p => p.AllEventSignups)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_allEventSignups_Events");

            entity.HasOne(d => d.User).WithMany(p => p.AllEventSignups)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_allEventSignups_Users");
        });

        modelBuilder.Entity<OurEvent>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PK__AllOurEv__2370F72766D30366");

            entity.HasOne(d => d.Address).WithMany(p => p.AllOurEvents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Events_Addresses");
        });

        modelBuilder.Entity<OpenHour>(entity =>
        {
            entity.HasKey(e => e.OpenHoursId).HasName("PK__OpenHour__BA5097B89F4926B3");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__products__47027DF59D7BD6B3");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__B9BE370FBBABA0DB");
        });

        modelBuilder.Entity<ZipCode>(entity =>
        {
            entity.HasKey(e => e.Postalcode).HasName("PK__ZipCodes__30EE2C295C79F19C");

            entity.Property(e => e.Postalcode).IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
