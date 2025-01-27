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
    
    public ICollection<UserQuestionnaire> UserQuestionnaires { get; set; } = new List<UserQuestionnaire>();
}