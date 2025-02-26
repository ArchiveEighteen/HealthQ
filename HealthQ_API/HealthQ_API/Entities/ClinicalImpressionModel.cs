using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using HealthQ_API.Entities.Auxiliary;
using Microsoft.EntityFrameworkCore;

namespace HealthQ_API.Entities;

[Table("clinical_impressions", Schema = "public")]

public class ClinicalImpressionModel
{
    [Key]
    [Required]
    [Column("id")]
    public required Guid Id { get; set; }
    
    [Required]
    [Column("questionnaire_id")]
    public required Guid QuestionnaireId { get; set; }
    
    [Required]
    [Column("patient_id")]
    public required string PatientId { get; set; }
    
    [Required]
    [Column("questionnaire_content")]
    public required string ClinicalImpressionContent { get; set; }
    
    public QuestionnaireModel? Questionnaire { get; set; }
    public PatientModel? Patient { get; set; }
    
    public ICollection<ObservationModel> Observations { get; set; } = new List<ObservationModel>();
}