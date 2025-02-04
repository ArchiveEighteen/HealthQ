using HealthQ_API.Context;
using HealthQ_API.Entities;
using Microsoft.EntityFrameworkCore;

namespace HealthQ_API.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly HealthqDbContext _context;

    public PatientRepository(HealthqDbContext context)
    {
        _context = context;
    }
    
    public async Task<PatientModel?> GetPatientAsync(string email, CancellationToken ct)
    {
        return await _context.Patients.FindAsync([email], cancellationToken: ct);
    }

    public async Task<PatientModel?> GetPatientWithQuestionnairesAsync(string email, CancellationToken ct)
    {
        return await _context.Patients
            .Include(x => x.Questionnaires)
            .FirstOrDefaultAsync(x => x.UserEmail == email, ct);
    }

    public async Task<IEnumerable<PatientModel>> GetAllPatientsAsync(CancellationToken ct)
    {
        return await _context.Patients.ToListAsync(ct);
    }

    public async Task UpdatePatientAsync(PatientModel patient, CancellationToken ct)
    {
        _context.Patients.Update(patient);
        await _context.SaveChangesAsync(ct);
    }

    public async Task CreatePatientAsync(PatientModel patient, CancellationToken ct)
    {
        await _context.Patients.AddAsync(patient, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeletePatientAsync(string email, CancellationToken ct)
    {
        var patient = await _context.Patients.FindAsync([email], cancellationToken: ct);
        if (patient != null)
        {
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync(ct);
        }
    }
}