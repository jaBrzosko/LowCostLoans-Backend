using Domain.Examples;
using Microsoft.EntityFrameworkCore;

namespace Services.Data;

public class CoreDbContext : DbContext
{
    public DbSet<Example> Examples { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(databaseName: "CoreDb");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureExamples(modelBuilder);
    }

    private static void ConfigureExamples(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Example>().HasKey(e => e.Id);
        modelBuilder.Entity<Example>().Property(e => e.Name).HasMaxLength(StringLengths.ShortString);
    }
}
