using HealthQ_API.DTOs;
using HealthQ_API.Repositories;

namespace HealthQ_API.Services;

public class AdminService
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly IPatientRepository _patientRepository;

    public AdminService(IDoctorRepository doctorRepository, IPatientRepository patientRepository)
    {
        _doctorRepository = doctorRepository;
        _patientRepository = patientRepository;
    }

    public async Task AssignPatientToDoctor(string doctorId, string patientId, CancellationToken ct)
    {
        
        var doctor = await _doctorRepository.GetDoctorAsync(doctorId, ct);
        if(doctor == null) throw new NullReferenceException("Doctor not found");
            
        var patient = await _patientRepository.GetPatientAsync(patientId, ct);
        if(patient == null) throw new NullReferenceException("Patient not found");

        doctor.Patients.Add(patient);
        
        await _doctorRepository.UpdateDoctorAsync(doctor, ct);
    }
    
}