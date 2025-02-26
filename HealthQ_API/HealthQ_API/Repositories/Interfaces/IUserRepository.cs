using HealthQ_API.Entities;

namespace HealthQ_API.Repositories;

public interface IUserRepository
{
    public Task<UserModel?> GetUserAsync(string email, CancellationToken ct);
    public Task<IEnumerable<UserModel>> GetAllUsersAsync(CancellationToken ct);
    public Task UpdateUserAsync(UserModel user, CancellationToken ct);
    public Task CreateUserAsync(UserModel user, CancellationToken ct);
    public Task DeleteUserAsync(string email, CancellationToken ct);

}