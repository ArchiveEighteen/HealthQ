using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Runtime.CompilerServices;
using HealthQ_API.Entities.Auxiliary;
using Microsoft.EntityFrameworkCore;

namespace HealthQ_API.Entities;

[Table("questionnaires", Schema = "public")]

public class QuestionnaireModel
{
    [Key]
    [Required]
    [Column("id")]
    public required Guid Id { get; set; }
    
    [Required]
    [Column("questionnaire_content")]
    public required string QuestionnaireContent { get; set; }
    
    [Required]
    [Column("owner_email")]
    [MaxLength(254)]
    public string OwnerId { get; set; }
    
    public DoctorModel Owner { get; set; }
    public ICollection<PatientModel> Patients { get; set; } = new List<PatientModel>();
    public ICollection<PatientQuestionnaire> PatientQuestionnaires { get; set; } = new List<PatientQuestionnaire>();
}