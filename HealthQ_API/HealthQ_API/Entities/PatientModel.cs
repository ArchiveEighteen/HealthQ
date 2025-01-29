using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HealthQ_API.Entities.Auxiliary;

namespace HealthQ_API.Entities;

[Table("patients", Schema = "public")]
public class PatientModel
{
    
    [Key]
    [Required]
    [Column("user_email")]
    [MaxLength(254)]
    public required string UserEmail { get; set; }

    public UserModel User { get; set; } = null!;
    public ICollection<QuestionnaireModel> Questionnaires { get; set; } = null!;
    public ICollection<PatientQuestionnaire> PatientQuestionnaires { get; set; } = new List<PatientQuestionnaire>();
    public ICollection<DoctorModel> Doctors { get; set; } = new List<DoctorModel>();
    public ICollection<DoctorPatient> DoctorPatients { get; set; } = new List<DoctorPatient>();
}