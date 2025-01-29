using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthQ_API.Entities.Auxiliary;

[Table("patient_questionnaire", Schema = "public")]
public class PatientQuestionnaire
{
    [MaxLength(254)]
    [Required]
    public required string PatientId { get; set; }
    
    [Required]
    public required Guid QuestionnaireId { get; set; }
    
    public PatientModel Patient { get; set; } = null!;
    public QuestionnaireModel Questionnaire { get; set; } = null!;
}