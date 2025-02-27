using HealthQ_API.Entities;

namespace HealthQ_API.Repositories.Interfaces;

public interface IObservationRepository
{
    public Task<string> GetJsonByIdAsync(Guid observationId, CancellationToken ct);
    
    public Task AddObservationAsync(ObservationModel observationModel, CancellationToken ct);
}