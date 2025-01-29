using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthQ_API.Entities.Auxiliary;

[Table("patient_questionnaire", Schema = "public")]
public class PatientQuestionnaire
{
    [MaxLength(254)]
    public string? PatientEmail { get; set; }
    
    public Guid QuestionnaireId { get; set; }
    
    public UserModel User { get; set; }
    public QuestionnaireModel Questionnaire { get; set; }
}