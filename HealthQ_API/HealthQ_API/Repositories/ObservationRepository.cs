using HealthQ_API.Context;
using HealthQ_API.Entities;
using HealthQ_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthQ_API.Repositories;

public class ObservationRepository : IObservationRepository
{
    private HealthqDbContext _context;

    public ObservationRepository(HealthqDbContext context)
    {
        _context = context;
    }
    
    public async Task<string> GetJsonByIdAsync(Guid observationId, CancellationToken ct)
    {
        var observationJson = await _context.Observations.FirstOrDefaultAsync(o => o.Id == observationId, cancellationToken: ct);
        return observationJson != null ? observationJson.ObservationContent : "";
    }

    public async Task AddObservationAsync(ObservationModel observationModel, CancellationToken ct)
    {
        await _context.Observations.AddAsync(observationModel, cancellationToken: ct);
        await _context.SaveChangesAsync(ct);
    }
}