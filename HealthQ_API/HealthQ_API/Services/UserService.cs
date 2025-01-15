using HealthQ_API.Context;
using HealthQ_API.DTOs;
using HealthQ_API.Entities;
using HealthQ_API.Security;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace HealthQ_API.Services;

public class UserService
{
    private readonly HealthqDbContext _context;

    public UserService(HealthqDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserDTO>> GetAllUsersAsync(CancellationToken ct)
    {
        var users = await _context.Users.ToListAsync(ct);
        return users.Select(user => new UserDTO
            {
                Email = user.Email,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                BirthDate = user.BirthDate.ToDateTime(new TimeOnly(0, 0)),
                Gender = user.Gender.ToString(),
                UserType = user.UserType.ToString(),
                Password = ""
            })
            .ToList();
    }

    public async Task<UserDTO?> GetUserByEmailAsync(string email, CancellationToken ct)
    {
        var user = await _context.Users.FindAsync([email], cancellationToken: ct);

        if (user == null)
            throw new NullReferenceException($"User with email {email} does not exist");

        var userDto = new UserDTO
        {
            Email = user.Email,
            Username = user.Username,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            BirthDate = user.BirthDate.ToDateTime(new TimeOnly(0, 0)),
            Gender = user.Gender.ToString(),
            UserType = user.UserType.ToString(),
            Password = ""
        };
        
        return userDto;
    }

    public async Task<UserDTO> CreateUserAsync(UserDTO user, CancellationToken ct)
    {
        var (hash, salt) = HashingUtility.HashPassword(user.Password!);

        EGender userGender;
        EUserType userType;
        
        if (Enum.TryParse<EGender>(user.Gender, out var receivedGender))
            userGender = receivedGender;
        else
            throw new InvalidCastException("Invalid gender value");

        if (Enum.TryParse<EUserType>(user.UserType, out var receivedUserType))
            userType = receivedUserType;
        else
            throw new InvalidCastException("Invalid user type value");
        
        var userModel = new UserModel
        {
            Email = user.Email,
            Username = user.Username,
            FirstName = user.FirstName,
            LastName = user.LastName,
            BirthDate = DateOnly.FromDateTime(user.BirthDate),
            Gender = userGender,
            PhoneNumber = user.PhoneNumber,
            UserType = userType,
            PasswordHash = hash,
            PasswordSalt = salt,

        };
        _context.Users.Add(userModel);
        await _context.SaveChangesAsync(ct);
        user.Password = "";
        return user;
    }

    public async Task<UserDTO> UpdateUserAsync(UserDTO user, CancellationToken ct)
    {
        _context.Entry(user).State = EntityState.Modified;
        await _context.SaveChangesAsync(ct);
        return user;
    }

    public async Task DeleteUserAsync(string email, CancellationToken ct)
    {
        var user = await _context.Users.FindAsync([email], cancellationToken: ct);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync(ct);
        }
    }
}