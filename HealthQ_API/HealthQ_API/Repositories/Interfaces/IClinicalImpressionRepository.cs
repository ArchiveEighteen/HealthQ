using HealthQ_API.Entities;

namespace HealthQ_API.Repositories;

public interface IClinicalImpressionRepository
{
    public Task SubmitClinicalImpressionAsync(ClinicalImpressionModel clinicalImpressionModel, CancellationToken ct);
}