using HealthQ_API.Context;
using HealthQ_API.Entities;

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
}