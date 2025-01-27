using HealthQ_API.Entities;
using HealthQ_API.Entities.Auxiliary;
using Microsoft.EntityFrameworkCore;

namespace HealthQ_API.Context;

public sealed class HealthqDbContext : DbContext
{
    public DbSet<UserModel> Users { get; set; }
    public DbSet<QuestionnaireModel> Questionnaires { get; set; }
    public DbSet<UserQuestionnaire> UserQuestionnaires { get; set; }
    
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

        // UserQuestionnaire
        modelBuilder.Entity<UserQuestionnaire>()
            .HasKey(e => new { e.UserId, e.QuestionnaireId });
        
        modelBuilder.Entity<UserQuestionnaire>()
            .HasOne(uq => uq.User)
            .WithMany(u => u.UserQuestionnaires)
            .HasForeignKey(uj => uj.UserId);
        
        modelBuilder.Entity<UserQuestionnaire>()
            .HasOne(uj => uj.Questionnaire)
            .WithMany(q => q.UserQuestionnaires)
            .HasForeignKey(uj => uj.QuestionnaireId);
        
        // UserModel
        modelBuilder.Entity<UserModel>();

        // QuestionnaireModel
        modelBuilder.Entity<QuestionnaireModel>();

    }
    
}