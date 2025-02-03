using HealthQ_API.Context;
using HealthQ_API.Entities;
using HealthQ_API.Entities.Auxiliary;
using Hl7.Fhir.Model;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace HealthQ_API.Services;

public class DoctorPatientService
{
    private readonly HealthqDbContext _context;
    
    public DoctorPatientService(HealthqDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<string>> GetAllDoctorPatientsAsync(string doctorEmail)
    {
        var result =  await _context.DoctorPatients
            .Where(dp => dp.DoctorId == doctorEmail)
            .Select(dp => dp.PatientId)
            .ToListAsync();
        
        return result;
    }
    
    
}