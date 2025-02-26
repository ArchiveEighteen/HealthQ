using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthQ_API.Entities;

[Table("observations", Schema = "public")]
public class ObservationModel
{
    [Key]
    [Required]
    [Column("id")]
    public required Guid Id { get; set; }
    
    [Required]
    [Column("clinical_impression_id")]
    public required Guid ClinicalImpressionId { get; set; }
    
    [Required]
    [Column("observation_content")]
    public required string ObservationContent { get; set; }
    
    public ClinicalImpressionModel? ClinicalImpression { get; set; }
}