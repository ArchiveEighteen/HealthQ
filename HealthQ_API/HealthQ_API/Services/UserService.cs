using System.Globalization;
using AutoMapper;
using HealthQ_API.Context;
using HealthQ_API.DTOs;
using HealthQ_API.Entities;
using HealthQ_API.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace HealthQ_API.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly PasswordService _passwordService;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IPatientRepository _patientRepository;

    public UserService(
        IUserRepository userRepository, 
        IMapper mapper,
        PasswordService passwordService,
        IDoctorRepository doctorRepository,
        IPatientRepository patientRepository)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _passwordService = passwordService;
        _doctorRepository = doctorRepository;
        _patientRepository = patientRepository;
    }

    public async Task<IEnumerable<UserDTO>> GetAllUsersAsync(CancellationToken ct)
    {
        return (await _userRepository.GetAllUsersAsync(ct)).Select(userModel => _mapper.Map<UserDTO>(userModel))
            .ToList();
    }

    public async Task<UserDTO?> GetUserByEmailAsync(string email, CancellationToken ct)
    {
        var userModel = await _userRepository.GetUserAsync(email, ct);
        
        return userModel == null ?  null : _mapper.Map<UserDTO>(userModel);
    }

    public async Task<UserDTO> CreateUserAsync(UserDTO user, CancellationToken ct)
    {
        var userModel = await _userRepository.GetUserAsync(user.Email, ct);
        if (userModel != null)
            throw new Exception("User already exists");
        
        userModel = _mapper.Map<UserModel>(user);
        
        var (hash, salt) = _passwordService.HashPassword(user.Password!);
        userModel.PasswordHash = hash;
        userModel.PasswordSalt = salt;

        await _userRepository.CreateUserAsync(userModel, ct);

        if (userModel.UserType == EUserType.Patient)
            await _patientRepository.CreatePatientAsync(new PatientModel { UserEmail = user.Email }, ct);
        else if (userModel.UserType == EUserType.Doctor)
        {
            await _doctorRepository.CreateDoctorAsync(new DoctorModel { UserEmail = user.Email }, ct);
        }
        
        return user;
    }

    public async Task DeleteUserAsync(string email, CancellationToken ct)
    {
        await _userRepository.DeleteUserAsync(email, ct);
    }

    public async Task<UserDTO?> UpdateUserAsync(UserDTO user,CancellationToken ct)
    {
        var userModel = await _userRepository.GetUserAsync(user.Email, ct);
        if (userModel == null)
            throw new NullReferenceException("User not found");
        
        var userToUpdate = _mapper.Map<UserModel>(user);
        userToUpdate.PasswordHash = userModel.PasswordHash;
        userToUpdate.PasswordSalt = userModel.PasswordSalt;
        
        await _userRepository.UpdateUserAsync(userToUpdate, ct);
        return user;
    }
}