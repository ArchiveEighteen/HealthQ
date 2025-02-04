using HealthQ_API.Entities;

namespace HealthQ_API.Repositories;

public interface IDoctorRepository
{
    public Task<DoctorModel?> GetDoctorAsync(string email, CancellationToken ct);
    public Task<IEnumerable<DoctorModel>> GetAllDoctorsAsync(CancellationToken ct);
    public Task UpdateDoctorAsync(DoctorModel doctor, CancellationToken ct);
    public Task CreateDoctorAsync(DoctorModel doctor, CancellationToken ct);
    public Task DeleteDoctorAsync(string email, CancellationToken ct);
}