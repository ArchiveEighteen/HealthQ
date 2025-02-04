using HealthQ_API.Entities;
using HealthQ_API.Repositories;

namespace HealthQ_API.Services;

public class DoctorService
{
    private readonly IDoctorRepository _doctorRepository;

    public DoctorService(IDoctorRepository doctorRepository)
    {
        _doctorRepository = doctorRepository;
    }

    public async Task<IEnumerable<string>> GetPatientIds(string doctorEmail, CancellationToken ct)
    {
        var doctor = await _doctorRepository.GetDoctorWithPatientsAsync(doctorEmail, ct);
        if(doctor == null)
            throw new NullReferenceException("Doctor not found");
        
        return doctor.Patients.Select(p => p.UserEmail);
    }
}