using HealthQ_API.Entities;
using HealthQ_API.Entities.Auxiliary;
using Hl7.Fhir.Model;
using Microsoft.EntityFrameworkCore;

namespace HealthQ_API.Context;

public sealed class HealthqDbContext : DbContext
{
    public DbSet<UserModel> Users { get; set; }
    public DbSet<DoctorModel> Doctors { get; set; }
    public DbSet<PatientModel> Patients { get; set; }
    public DbSet<QuestionnaireModel> Questionnaires { get; set; }
    public DbSet<TemplateModel> Templates { get; set; }
    public DbSet<PatientQuestionnaire> PatientQuestionnaire { get; set; }
    public DbSet<DoctorPatient> DoctorPatients { get; set; }
    public DbSet<ClinicalImpressionModel> ClinicalImpressions { get; set; }
    public DbSet<ObservationModel> Observations { get; set; }
    public DbSet<FileModel> Files { get; set; }
    
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
        
        // UserModel
        modelBuilder.Entity<UserModel>(entity =>
        {
            entity
                .HasOne(u => u.Doctor)
                .WithOne(d => d.User)
                .HasForeignKey<DoctorModel>(d => d.UserEmail)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity
                .HasOne(u => u.Patient)
                .WithOne(d => d.User)
                .HasForeignKey<PatientModel>(d => d.UserEmail)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // PatientModel Many to Many with QuestionnaireModel
        modelBuilder.Entity<PatientModel>(entity =>
        {
            entity
                .HasMany(p => p.Questionnaires)
                .WithMany(q => q.Patients)
                .UsingEntity<PatientQuestionnaire>(
                    r => r
                        .HasOne<QuestionnaireModel>(pq => pq.Questionnaire)
                        .WithMany(q => q.PatientQuestionnaires)
                        .HasForeignKey(pq => pq.QuestionnaireId)
                        .OnDelete(DeleteBehavior.Cascade),
                    l => l
                        .HasOne<PatientModel>(pq => pq.Patient)
                        .WithMany(p => p.PatientQuestionnaires)
                        .HasForeignKey(pq => pq.PatientId)
                        .OnDelete(DeleteBehavior.Cascade));

        });

        // DoctorModel Many to Many with PatientModel
        modelBuilder.Entity<DoctorModel>(entity =>
        {
            entity
                .HasMany(d => d.Patients)
                .WithMany(p => p.Doctors)
                .UsingEntity<DoctorPatient>(
                    r => r
                        .HasOne<PatientModel>(dp => dp.Patient)
                        .WithMany(p => p.DoctorPatients)
                        .HasForeignKey(dp => dp.PatientId)
                        .OnDelete(DeleteBehavior.Cascade),
                    l => l
                        .HasOne<DoctorModel>(dp => dp.Doctor)
                        .WithMany(p => p.DoctorPatients)
                        .HasForeignKey(dp => dp.DoctorId)
                        .OnDelete(DeleteBehavior.Cascade));
        });

        // QuestionnaireModel
        modelBuilder.Entity<QuestionnaireModel>(entity =>
        {
            entity
                .HasOne(q => q.Owner)
                .WithMany(o => o.Questionnaires)
                .HasForeignKey(q => q.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

        });

        // ClinicalImpressionModel
        modelBuilder.Entity<ClinicalImpressionModel>(entity =>
        {
            entity
                .HasOne(ci => ci.Questionnaire)
                .WithMany(q => q.ClinicalImpressions)
                .HasForeignKey(ci => ci.QuestionnaireId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity
                .HasOne(ci => ci.Patient)
                .WithMany(p => p.ClinicalImpressions)
                .HasForeignKey(ci => ci.PatientId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ObservationModel>(entity =>
        {
            entity
                .HasOne(o => o.ClinicalImpression)
                .WithMany(c => c.Observations)
                .HasForeignKey(o => o.ClinicalImpressionId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
    
}