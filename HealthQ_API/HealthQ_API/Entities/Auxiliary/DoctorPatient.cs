using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthQ_API.Entities.Auxiliary;

[Table("doctor_patient", Schema = "public")]
public class DoctorPatient
{
    [MaxLength(254)]
    public string? DoctorEmail { get; set; }
    
    [MaxLength(254)]
    public string? PatientEmail { get; set; }
    
    public required DoctorModel Doctor { get; set; }
    
    public required PatientModel Patient { get; set; }
}