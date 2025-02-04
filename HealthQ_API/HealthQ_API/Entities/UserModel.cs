using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Runtime.CompilerServices;
using HealthQ_API.Entities.Auxiliary;

namespace HealthQ_API.Entities;

[Table("users", Schema = "public")]
public class UserModel
{
    [Key]
    [Required]
    [Column("email")]
    [MaxLength(254)]
    public required string Email { get; set; }
    
    [Required]
    [Column("username")]
    [MaxLength(64)]
    public required string Username { get; set; }
    
    [Required]
    [Column("password_salt")]
    [MaxLength(64)]
    public string? PasswordSalt { get; set; }
    
    [Required]
    [Column("password_hash")]
    [MaxLength(128)]
    public string? PasswordHash { get; set; }
    
    [Required]
    [Column("first_name")]
    [MaxLength(50)]
    public required string FirstName { get; set; }
    
    [Required]
    [Column("last_name")]
    [MaxLength(50)]
    public required string LastName { get; set; }
    
    [Required]
    [Column("birth_date")]
    public required DateOnly BirthDate { get; set; }
    
    [Required]
    [Column("gender")]
    public required EGender Gender { get; set; }
    
    [Required]
    [Column("phone_number")]
    [MaxLength(13)]
    [MinLength(13)]
    public required string PhoneNumber { get; set; }
    
    [Required]
    [Column("user_type")]
    public required EUserType UserType { get; set; }

    public DoctorModel? Doctor { get; set; }
    public PatientModel? Patient { get; set; }
}