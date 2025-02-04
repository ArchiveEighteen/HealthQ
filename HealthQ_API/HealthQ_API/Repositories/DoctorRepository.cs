using HealthQ_API.Context;
using HealthQ_API.Entities;
using Microsoft.EntityFrameworkCore;

namespace HealthQ_API.Repositories;

public class DoctorRepository : IDoctorRepository
{
    private readonly HealthqDbContext _context;

    public DoctorRepository(HealthqDbContext context)
    {
        _context = context;
    }
    
    public async Task<DoctorModel?> GetDoctorAsync(string email, CancellationToken ct)
    {
        return await _context.Doctors.FindAsync([email], cancellationToken: ct);
    }

    public async Task<IEnumerable<DoctorModel>> GetAllDoctorsAsync(CancellationToken ct)
    {
        return await _context.Doctors.ToListAsync(ct);
    }

    public async Task UpdateDoctorAsync(DoctorModel doctor, CancellationToken ct)
    {
        _context.Doctors.Update(doctor);
        await _context.SaveChangesAsync(ct);
    }

    public async Task CreateDoctorAsync(DoctorModel doctor, CancellationToken ct)
    {
        await _context.Doctors.AddAsync(doctor, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteDoctorAsync(string email, CancellationToken ct)
    {
        var doctor = await _context.Doctors.FindAsync([email], cancellationToken: ct);
        if (doctor != null)
        {
            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync(ct);
        }
    }
}