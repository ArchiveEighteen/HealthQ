using HealthQ_API.Context;
using HealthQ_API.Entities;
using HealthQ_API.Entities.Auxiliary;
using Microsoft.EntityFrameworkCore;

namespace HealthQ_API.Services;

public class QuestionnaireService
{
    private readonly HealthqDbContext _context;

    public QuestionnaireService(HealthqDbContext context)
    {
        _context = context;
    }

    public async Task<List<string>> GetAllSurveysAsync(string userEmail)
    {
        var questionnaireStrings = await _context.PatientQuestionnaire
            .Where(uq => uq.PatientId == userEmail)
            .Select(uq => uq.Questionnaire.QuestionnaireContent)
            .ToListAsync();

        return questionnaireStrings;
    }

    public async Task<PatientQuestionnaire> AddSurveyAsync(string userEmail, QuestionnaireModel questionnaire)
    {
        if( await _context.PatientQuestionnaire.Where(uq => uq.QuestionnaireId == questionnaire.Id || uq.PatientId == userEmail).AnyAsync())
            throw new Exception($"Questionnaire bound to user {userEmail} with id {questionnaire.Id} already exists");

        await _context.Questionnaires.AddAsync(questionnaire);

        var userQuestionnaire = new PatientQuestionnaire
        {
            PatientId = userEmail,
            QuestionnaireId = questionnaire.Id,
        };
        
        await _context.PatientQuestionnaire.AddAsync(userQuestionnaire);
        await _context.SaveChangesAsync();
        return userQuestionnaire;
    }
}