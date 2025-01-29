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
    public DbSet<PatientQuestionnaire> PatientQuestionnaire { get; set; }
    public DbSet<DoctorPatient> DoctorPatients { get; set; }
    
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
                .IsRequired(false);
            
            entity
                .HasOne(u => u.Patient)
                .WithOne(d => d.User)
                .HasForeignKey<PatientModel>(d => d.UserEmail)
                .IsRequired(false);
        });

        // PatientModel Many to Many with QuestionnaireModel
        modelBuilder.Entity<PatientModel>(entity =>
        {
            entity
                .HasMany(p => p.Questionnaires)
                .WithMany(q => q.Patients)
                .UsingEntity<PatientQuestionnaire>(
                    r => r
                        .HasOne<QuestionnaireModel>()
                        .WithMany(q => q.PatientQuestionnaires)
                        .HasForeignKey(pq => pq.QuestionnaireId),
                    l => l
                        .HasOne<PatientModel>()
                        .WithMany(p => p.PatientQuestionnaires)
                        .HasForeignKey(pq => pq.PatientEmail));

        });
        
        // DoctorPatient
        // modelBuilder.Entity<DoctorPatient>(entity =>
        // {
        //     entity.HasKey(dp => new { dp.DoctorEmail, dp.PatientEmail });
        // });

        // DoctorModel Many to Many with PatientModel
        modelBuilder.Entity<DoctorModel>(entity =>
        {
            entity
                .HasMany(d => d.Patients)
                .WithMany(p => p.Doctors)
                .UsingEntity<DoctorPatient>(
                    r => r
                        .HasOne<PatientModel>()
                        .WithMany(p => p.DoctorPatients)
                        .HasForeignKey(dp => dp.PatientEmail),
                    l => l
                        .HasOne<DoctorModel>()
                        .WithMany(p => p.DoctorPatients)
                        .HasForeignKey(dp => dp.DoctorEmail));
        });

        //QuestionnaireModel
        modelBuilder.Entity<QuestionnaireModel>(entity =>
        {
            entity
                .HasOne(q => q.Owner)
                .WithMany(o => o.Questionnaires)
                .HasForeignKey(q => q.OwnerEmail);

        });

    }
    
}