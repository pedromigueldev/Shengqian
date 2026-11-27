using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shengqian.Domain.Model;

namespace Shengqian.AppDbContext;

public class CategoryIdConverter() : ValueConverter<CategoryId, int>(id => id.Key, id => new CategoryId(id));
public class TransactionIdConverter() : ValueConverter<TransactionId, int>(id => id.Key, id => new TransactionId(id));
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<CategoryId>().HaveConversion<CategoryIdConverter>();
        configurationBuilder.Properties<TransactionId>().HaveConversion<TransactionIdConverter>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaction>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .HasSentinel(new TransactionId(0));
    }
}