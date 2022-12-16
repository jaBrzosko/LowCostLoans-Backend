using Domain.Examples;
using Domain.Inquires;
using Domain.Offers;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Services.Data;

public class CoreDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Inquire> Inquiries { get; set; }
    public DbSet<Offer> Offers { get; set; }

    public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureUsers(modelBuilder);
        ConfigureInquiries(modelBuilder);
        ConfigureOffers(modelBuilder);
    }

    private static void ConfigureUsers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(cfg =>
        {
            cfg.HasKey(e => e.Id);
            cfg.Property(e => e.Id).HasMaxLength(StringLengths.ShortString);
            cfg.OwnsOne(e => e.PersonalData, inner =>
            {
                inner.Property(e => e.FirstName).HasMaxLength(StringLengths.ShortString);
                inner.Property(e => e.LastName).HasMaxLength(StringLengths.ShortString);
                inner.Property(e => e.GovernmentId).HasMaxLength(StringLengths.MediumString);
                inner.Property(e => e.GovernmentIdType);
                inner.Property(e => e.JobType);
            });
        });
    }
    
    private static void ConfigureInquiries(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Inquire>(cfg =>
        {
            cfg.HasKey(e => e.Id);
            cfg.Property(e => e.UserId);
            cfg.Property(e => e.MoneyInSmallestUnit);
            cfg.Property(e => e.NumberOfInstallments);
            cfg.Property(e => e.CreationTime);
            cfg.Property(e => e.Status);
            cfg.OwnsOne(e => e.PersonalData, inner =>
            {
                inner.Property(e => e.FirstName).HasMaxLength(StringLengths.ShortString);
                inner.Property(e => e.LastName).HasMaxLength(StringLengths.ShortString);
                inner.Property(e => e.GovernmentId).HasMaxLength(StringLengths.MediumString);
                inner.Property(e => e.GovernmentIdType);
                inner.Property(e => e.JobType);
            });

            cfg.HasIndex(e => e.UserId);
        });
    }
    
    private static void ConfigureOffers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Offer>(cfg =>
        {
            cfg.HasKey(e => e.Id);
            cfg.Property(e => e.InquireId);
            cfg.Property(e => e.InterestRateInPromiles);
            cfg.Property(e => e.MoneyInSmallestUnit);
            cfg.Property(e => e.NumberOfInstallments);
            cfg.Property(e => e.SourceBank).HasDefaultValue(OfferSourceBank.OurBank);
            cfg.Property(e => e.CreationTime);
            cfg.Property(e => e.BankId).HasMaxLength(StringLengths.ShortString);

            cfg.HasIndex(e => e.InquireId);
        });
    }
}
