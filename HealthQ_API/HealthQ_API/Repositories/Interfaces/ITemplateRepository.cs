using HealthQ_API.Entities;

namespace HealthQ_API.Repositories.Interfaces;

public interface ITemplateRepository
{
    public Task<TemplateModel?> GetTemplateAsync(Guid id, CancellationToken ct);
    public Task<IEnumerable<TemplateModel>> GetAllTemplatesAsync(CancellationToken ct);
    public Task<IEnumerable<TemplateModel>> GetTemplatesByOwnerAsync(string email, CancellationToken ct);
    public Task UpdateTemplateAsync(TemplateModel template, CancellationToken ct);
    public Task CreateTemplateAsync(TemplateModel template, CancellationToken ct);
    public Task DeleteTemplateAsync(Guid id, CancellationToken ct);
}