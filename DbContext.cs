using Microsoft.EntityFrameworkCore;
using Shengqian.Domain;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Category> Todos => Set<Category>();
}