namespace HealthQ_API.DTOs;

public class UserDTO
{
    public required string Email { get; set; } = string.Empty;
    public required string Username { get; set; } = string.Empty;
    public string? Password { get; set; } = string.Empty;
    public required string FirstName { get; set; } = string.Empty;
    public required string LastName { get; set; } = string.Empty;
    public required string PhoneNumber { get; set; } = string.Empty;
    public required DateTime BirthDate { get; set; }
    public required string Gender { get; set; } = string.Empty;
    public required string UserType { get; set; } = string.Empty;
}