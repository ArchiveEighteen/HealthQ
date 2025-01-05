using HealthQ_API.Entities;
using Microsoft.EntityFrameworkCore;

namespace HealthQ_API.Context;

public sealed class HealthqDbContext : DbContext
{
    public DbSet<UserModel> Users { get; set; }
    
    public HealthqDbContext()
    {
        Database.EnsureCreated();
    }

    public HealthqDbContext(DbContextOptions<HealthqDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.HasDefaultSchema("public");
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<UserModel>(entity =>
        {
            
        });
    }
    
}