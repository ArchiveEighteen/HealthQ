using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthQ_API.Entities;

public class FileModel
{
    [Key]
    [Required]
    [Column("id")]
    public int Id { get; set; }
    
    [Required]
    [Column("file_name")]
    public required string FileName { get; set; }
    
    [Required]
    [Column("file_data")]
    public required byte[] FileData { get; set; }
    
    [Required]
    [Column("content_type")]
    public required string ContentType { get; set; }
}