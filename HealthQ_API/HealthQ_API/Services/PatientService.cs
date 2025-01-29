using HealthQ_API.Context;
using HealthQ_API.Entities;
using Microsoft.EntityFrameworkCore;

namespace HealthQ_API.Services;

public class PatientService
{
    private readonly HealthqDbContext _context;

    public PatientService(HealthqDbContext context)
    {
        _context = context;
    }

    public async Task<PatientModel?> GetPatientAsync(string patientEmail, CancellationToken ct)
    {
        
        return await _context.Patients.Include(x => x.Questionnaires).FirstOrDefaultAsync(x => x.UserEmail == patientEmail, ct);
    }
    
    
}