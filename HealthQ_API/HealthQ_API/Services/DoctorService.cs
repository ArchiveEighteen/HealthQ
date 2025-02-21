using AutoMapper;
using HealthQ_API.DTOs;
using HealthQ_API.Entities;
using HealthQ_API.Repositories;

namespace HealthQ_API.Services;

public class DoctorService
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public DoctorService(
        IDoctorRepository doctorRepository, 
        IPatientRepository patientRepository,
        IUserRepository userRepository,
        IMapper mapper
    )
    {
        _doctorRepository = doctorRepository;
        _patientRepository = patientRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDTO>> GetNotOwnedPatients(string doctorId, CancellationToken ct)
    {
        var patients = await _patientRepository.GetAllPatientsAsync(ct);
        
        var doctor = await _doctorRepository.GetDoctorWithPatientsAsync(doctorId, ct);
        if(doctor == null)
            throw new NullReferenceException("Doctor not found");
        
        var notOwnedPatients = patients.Where(p => !doctor.Patients.Contains(p)).ToList();

        List<UserDTO> userPatients= new List<UserDTO>();
        foreach (var patient in notOwnedPatients)
        {
            userPatients.Add(_mapper.Map<UserDTO>(await _userRepository.GetUserAsync(patient.UserEmail, ct)));
        }

        return userPatients;
    }

    public async Task<IEnumerable<DoctorModel>> GetAllDoctors(CancellationToken ct)
    {
        return await _doctorRepository.GetAllDoctorsAsync(ct);
        
    }
    public async Task<IEnumerable<string>> GetPatientIds(string doctorEmail, CancellationToken ct)
    {
        var doctor = await _doctorRepository.GetDoctorWithPatientsAsync(doctorEmail, ct);
        if(doctor == null)
            throw new NullReferenceException("Doctor not found");
        
        return doctor.Patients.Select(p => p.UserEmail);
    }
}