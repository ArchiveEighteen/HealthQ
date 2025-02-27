using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthQ_API.Entities;


[Table("templates", Schema = "public")]
public class TemplateModel
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
    public required string OwnerId { get; set; }
}