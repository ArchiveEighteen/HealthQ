using HealthQ_API.Context;
using HealthQ_API.DTOs;
using Hl7.Fhir.Model;

namespace HealthQ_API.Services;

public class QuestionnaireService
{
    private readonly HealthqDbContext _context;

    public QuestionnaireService(HealthqDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Questionnaire>> GetAllSurveysAsync(CancellationToken ct)
    {
        var users = await _context.UserQuestionnaires.;
        return users.Select(user => new UserDTO
            {
                Email = user.Email,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                BirthDate = user.BirthDate.ToDateTime(new TimeOnly(0, 0)),
                Gender = user.Gender.ToString(),
                UserType = user.UserType.ToString(),
                Password = ""
            })
            .ToList();
    }
}