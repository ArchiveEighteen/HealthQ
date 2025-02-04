using HealthQ_API.Entities;

namespace HealthQ_API.Repositories;

public interface IPatientRepository
{
    public Task<PatientModel?> GetPatientAsync(string email, CancellationToken ct);
    public Task<IEnumerable<PatientModel>> GetAllPatientsAsync(CancellationToken ct);
    public Task UpdatePatientAsync(PatientModel patient, CancellationToken ct);
    public Task CreatePatientAsync(PatientModel patient, CancellationToken ct);
    public Task DeletePatientAsync(string email, CancellationToken ct);
}