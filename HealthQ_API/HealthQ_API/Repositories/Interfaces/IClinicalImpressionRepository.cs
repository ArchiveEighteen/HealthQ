using HealthQ_API.Entities;

namespace HealthQ_API.Repositories.Interfaces;

public interface IClinicalImpressionRepository
{
    public Task SubmitClinicalImpressionAsync(ClinicalImpressionModel clinicalImpressionModel, CancellationToken ct);
    public Task<string> GetClinicalImpressionContentByPatientAsync(string questionnaireId, string patientId, CancellationToken ct);
}