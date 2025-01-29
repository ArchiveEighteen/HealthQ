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
            return null;

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
        if( await GetUserByEmailAsync(user.Email, ct) != null)
            throw new Exception($"User with email {user.Email} already exists");
        
        var (hash, salt) = HashingUtility.HashPassword(user.Password!);

        
        if (!Enum.TryParse<EGender>(user.Gender, out var gender))
            throw new InvalidCastException("Invalid gender value");

        if (!Enum.TryParse<EUserType>(user.UserType, out var role))
            throw new InvalidCastException("Invalid user type value");
        
        var userModel = new UserModel
        {
            Email = user.Email,
            Username = user.Username,
            FirstName = user.FirstName,
            LastName = user.LastName,
            BirthDate = DateOnly.FromDateTime(user.BirthDate),
            Gender = gender,
            PhoneNumber = user.PhoneNumber,
            UserType = role,
            PasswordHash = hash,
            PasswordSalt = salt,

        };
        await _context.Users.AddAsync(userModel, ct);

        if (role == EUserType.Patient)
            await _context.Patients.AddAsync(new PatientModel { UserEmail = user.Email }, ct);
        else if (role == EUserType.Doctor)
        {
            await _context.Doctors.AddAsync(new DoctorModel { UserEmail = user.Email }, ct);
        }
        await _context.SaveChangesAsync(ct);
        
        user.Password = "";
        return user;
    }

    public async Task<UserDTO> VerifyUserAsync(UserDTO user, CancellationToken ct)
    {
        var existingUser = await _context.Users.FindAsync([user.Email], cancellationToken: ct);
        if (existingUser == null)
            throw new Exception($"User with email {user.Email} does not exist");

        if(existingUser.PasswordHash != HashingUtility.HashPassword(user.Password!, existingUser.PasswordSalt).Hash)
            throw new Exception("Wrong password");
        
        return (await GetUserByEmailAsync(user.Email, ct))!;
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