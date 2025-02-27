using HealthQ_API.Context;
using HealthQ_API.Entities;
using HealthQ_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthQ_API.Repositories;

public class ClinicalImpressionRepository : IClinicalImpressionRepository
{
    private readonly HealthqDbContext _context;

    public ClinicalImpressionRepository(HealthqDbContext context)
    {
        _context = context;
    }
    
    public async Task SubmitClinicalImpressionAsync(ClinicalImpressionModel clinicalImpressionModel, CancellationToken ct)
    {
        await _context.ClinicalImpressions.AddAsync(clinicalImpressionModel, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<string> GetClinicalImpressionContentByPatientAsync(string questionnaireId, string patientId, CancellationToken ct)
    {
        var clinicalImpression = await _context.ClinicalImpressions.FirstOrDefaultAsync(ci => ci.QuestionnaireId == Guid.Parse(questionnaireId) && ci.PatientId == patientId, ct);

        return clinicalImpression != null ?  clinicalImpression.ClinicalImpressionContent : "";
    }
}