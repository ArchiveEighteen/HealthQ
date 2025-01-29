using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HealthQ_API.Entities.Auxiliary;

namespace HealthQ_API.Entities;

[Table("doctors", Schema = "public")]
public class DoctorModel
{
    [Key]
    [Required]
    [Column("user_email")]
    [MaxLength(254)]
    public required string UserEmail { get; set; }

    public UserModel User { get; set; } = null!;
    public ICollection<QuestionnaireModel> Questionnaires { get; set; } = new List<QuestionnaireModel>();
    public ICollection<PatientModel> Patients { get; set; } = new List<PatientModel>();
    public ICollection<DoctorPatient> DoctorPatients { get; set; } = new List<DoctorPatient>();
}