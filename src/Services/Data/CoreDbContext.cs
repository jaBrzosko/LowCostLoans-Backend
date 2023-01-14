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
        SeedDatabase(modelBuilder);
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

    private static void SeedDatabase(ModelBuilder modelBuilder)
    {
        // seed users
        
        // Kowalski
        var personalDataKowalski =
            new PersonalData("Jan", "Kowalski", "96032887193", GovernmentIdType.Pesel, JobType.SomeJobType);
        var guidKowalski = "46d31f49-598d-462b-99c5-a923c484eefa";
        
        // Adamkiewicz
        var personalDataAdamkiewicz = 
            new PersonalData("Mariusz", "Adamkiewicz", "61123192191", GovernmentIdType.Pesel, JobType.SomeJobType);
        var guidAdamkiewicz = "15aa5faf-083f-4057-b2db-108fd7d8b09c";
        
        modelBuilder.Entity<User>().HasData(new User(guidKowalski,
            personalDataKowalski));
        modelBuilder.Entity<User>().HasData(new User(guidAdamkiewicz,
            personalDataAdamkiewicz));
        modelBuilder.Entity<User>().HasData(new User("a98d71e3-15f7-4840-960f-eadcf8668190",
            new PersonalData("Zofia", "Kaczmarek", "91120392522", GovernmentIdType.Pesel, JobType.SomeJobType)));
        
        // seed inquiries
        
        // Kowalski
        var inquireKowalski1 = new Inquire(guidKowalski, personalDataKowalski, 10_000_00, 24);
        var inquireKowalski2 = new Inquire(guidKowalski, personalDataKowalski, 250_000_00, 40);
        
        // Adamkiewicz
        var inquireAdamkiewicz1 = new Inquire(guidAdamkiewicz, personalDataAdamkiewicz, 450_000_12, 90);
        var inquireAdamkiewicz2 = new Inquire(guidAdamkiewicz, personalDataAdamkiewicz, 1_050_000_01, 123);
        var inquireAdamkiewicz3 = new Inquire(guidAdamkiewicz, personalDataAdamkiewicz, 1_000_00, 10);
        
        modelBuilder.Entity<Inquire>().HasData(inquireKowalski1);
        modelBuilder.Entity<Inquire>().HasData(inquireKowalski2);
        modelBuilder.Entity<Inquire>().HasData(inquireAdamkiewicz1);
        modelBuilder.Entity<Inquire>().HasData(inquireAdamkiewicz2);
        modelBuilder.Entity<Inquire>().HasData(inquireAdamkiewicz3);
        
        // seed offers

        // Kowalski
        SeedSingleOffer(modelBuilder, inquireKowalski1, 10);
        SeedSingleOffer(modelBuilder, inquireKowalski1, 21);
        SeedSingleOffer(modelBuilder, inquireKowalski2, 18);
        
        // Adamkiewicz
        SeedSingleOffer(modelBuilder, inquireAdamkiewicz1, 14);
        SeedSingleOffer(modelBuilder, inquireAdamkiewicz1, 23);
        SeedSingleOffer(modelBuilder, inquireAdamkiewicz1, 18);
        SeedSingleOffer(modelBuilder, inquireAdamkiewicz2, 35);
        SeedSingleOffer(modelBuilder, inquireAdamkiewicz3, 28);

    }

    private static void SeedSingleOffer(ModelBuilder modelBuilder, Inquire inquire, int promiles)
    {
        modelBuilder.Entity<Offer>().HasData(new Offer(inquire.Id, promiles, inquire.MoneyInSmallestUnit,
            inquire.NumberOfInstallments, OfferSourceBank.NoBank, String.Empty));
    }
}
