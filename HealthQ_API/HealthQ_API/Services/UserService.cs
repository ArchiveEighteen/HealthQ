using HealthQ_API.Context;
using HealthQ_API.Entities;
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
        return await _context.Users.FindAsync(email, ct);
    }

    public async Task<UserModel> CreateUserAsync(UserModel user, CancellationToken ct)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync(ct);
        return user;
    }

    public async Task<UserModel> UpdateUserAsync(UserModel user, CancellationToken ct)
    {
        _context.Entry(user).State = EntityState.Modified;
        await _context.SaveChangesAsync(ct);
        return user;
    }

    public async Task DeleteUserAsync(string email, CancellationToken ct)
    {
        var user = await GetUserByEmailAsync(email, ct);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync(ct);
        }
    }
}