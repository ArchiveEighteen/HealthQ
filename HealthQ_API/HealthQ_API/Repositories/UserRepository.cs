using HealthQ_API.Context;
using HealthQ_API.Entities;
using Microsoft.EntityFrameworkCore;

namespace HealthQ_API.Repositories;

public class UserRepository : IUserRepository
{
    private readonly HealthqDbContext _context;

    UserRepository(HealthqDbContext context)
    {
        _context = context;
    }


    public async Task<UserModel?> GetUserAsync(string email, CancellationToken ct)
    {
        return await _context.Users.FindAsync([email], cancellationToken: ct);
    }

    public async Task<IEnumerable<UserModel>> GetAllUsersAsync(CancellationToken ct)
    {
        return await _context.Users.ToListAsync(ct);
    }

    public async Task UpdateUserAsync(UserModel user, CancellationToken ct)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync(ct);
    }

    public async Task CreateUserAsync(UserModel user, CancellationToken ct)
    {
        await _context.Users.AddAsync(user, ct);
        await _context.SaveChangesAsync(ct);
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