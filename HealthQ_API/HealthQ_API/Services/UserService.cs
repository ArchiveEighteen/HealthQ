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

    public async Task<IEnumerable<UserModel>> GetAllUsersAsync(CancellationToken ct)
    {
        return await _context.Users.ToListAsync(ct);
    }

    public async Task<UserModel?> GetUserByEmailAsync(string email, CancellationToken ct)
    {
        return await _context.Users.FindAsync([email], cancellationToken: ct);
    }

    public async Task<UserModel> CreateUserAsync(UserModel user, string password, CancellationToken ct)
    {
        if( await GetUserByEmailAsync(user.Email, ct) != null)
            throw new Exception($"User with email {user.Email} already exists");
        
        var (hash, salt) = HashingUtility.HashPassword(password);
        
        user.PasswordHash = hash;
        user.PasswordSalt = salt;

        await _context.Users.AddAsync(user, ct);

        if (user.UserType == EUserType.Patient)
            await _context.Patients.AddAsync(new PatientModel { UserEmail = user.Email }, ct);
        else if (user.UserType == EUserType.Doctor)
        {
            await _context.Doctors.AddAsync(new DoctorModel { UserEmail = user.Email }, ct);
        }
        await _context.SaveChangesAsync(ct);
        
        return user;
    }

    public async Task<UserModel> VerifyUserAsync(string userEmail, string password, CancellationToken ct)
    {
        var existingUser = await _context.Users.FindAsync([userEmail], cancellationToken: ct);
        if (existingUser == null)
            throw new Exception($"User with email {userEmail} does not exist");

        if(existingUser.PasswordHash != HashingUtility.HashPassword(password, existingUser.PasswordSalt).Hash)
            throw new Exception("Wrong password");
        
        return (await GetUserByEmailAsync(userEmail, ct))!;
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

    public async Task<UserModel> UpdateUserAsync(UserModel user,CancellationToken ct)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync(ct);
        return user;
    }
}