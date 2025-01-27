using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthQ_API.Entities.Auxiliary;

[Table("user_questionnaire", Schema = "public")]
public class UserQuestionnaire
{
    [MaxLength(250)]
    public string UserId { get; set; }
    public Guid QuestionnaireId { get; set; }
    
    public required UserModel User { get; set; }
    public required QuestionnaireModel Questionnaire { get; set; }
    
}
