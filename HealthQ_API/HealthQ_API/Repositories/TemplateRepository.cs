using HealthQ_API.Context;
using HealthQ_API.Entities;
using HealthQ_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthQ_API.Repositories;

public class TemplateRepository : ITemplateRepository
{
    private readonly HealthqDbContext _context;

    public TemplateRepository(HealthqDbContext context)
    {
        _context = context;
    }


    public async Task<TemplateModel?> GetTemplateAsync(Guid id, CancellationToken ct)
    {
        return await _context.Templates.FindAsync([id], cancellationToken: ct);
    }

    public async Task<IEnumerable<TemplateModel>> GetTemplatesByOwnerAsync(string email, CancellationToken ct)
    {
        return await _context.Templates.Where(t => t.OwnerId == email).ToListAsync(ct);
    }

    public async Task<IEnumerable<TemplateModel>> GetAllTemplatesAsync(CancellationToken ct)
    {
        return await _context.Templates.ToListAsync(ct);
    }

    public async Task UpdateTemplateAsync(TemplateModel template, CancellationToken ct)
    {
        _context.Templates.Update(template);
        await _context.SaveChangesAsync(ct);
    }

    public async Task CreateTemplateAsync(TemplateModel template, CancellationToken ct)
    {
        await _context.Templates.AddAsync(template, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteTemplateAsync(Guid id, CancellationToken ct)
    {
        var template = await _context.Templates.FindAsync([id], cancellationToken: ct);
        if (template != null)
        {
            _context.Templates.Remove(template);
            await _context.SaveChangesAsync(ct);
        }
    }
    
}