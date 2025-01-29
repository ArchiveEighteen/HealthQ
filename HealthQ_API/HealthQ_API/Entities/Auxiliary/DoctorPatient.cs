using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthQ_API.Entities.Auxiliary;

[Table("doctor_patient", Schema = "public")]
public class DoctorPatient
{
    [MaxLength(254)]
    [Required]
    public required string DoctorId { get; set; }
    
    [MaxLength(254)]
    [Required]
    public required string PatientId { get; set; }
    
    public required DoctorModel Doctor { get; set; } = null!;
    
    public required PatientModel Patient { get; set; } = null!;
}