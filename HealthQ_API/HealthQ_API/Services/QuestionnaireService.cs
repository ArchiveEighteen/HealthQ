using HealthQ_API.Context;
using HealthQ_API.Entities;
using HealthQ_API.Entities.Auxiliary;
using Hl7.Fhir.Model;
using Microsoft.EntityFrameworkCore;

namespace HealthQ_API.Services;

public class QuestionnaireService
{
    private readonly HealthqDbContext _context;

    public QuestionnaireService(HealthqDbContext context)
    {
        _context = context;
    }

    public async Task<List<string>> GetAllDoctorSurveysAsync(string doctorEmail)
    {
        var questionnaireStrings =  await _context.Questionnaires
                .Where(q => q.OwnerId == doctorEmail)
                .Select(q => q.QuestionnaireContent)
                .ToListAsync();
        
        return questionnaireStrings;
    }

    public async Task<QuestionnaireModel?> AddSurveyAsync(QuestionnaireModel? questionnaire)
    {
        await _context.Questionnaires.AddAsync(questionnaire);
        await _context.SaveChangesAsync();
        
        return await _context.Questionnaires.FindAsync(questionnaire?.Id);
    }
    
    public async Task<QuestionnaireModel?> UpdateSurveyAsync(QuestionnaireModel? questionnaire)
    {
        _context.Questionnaires.Update(questionnaire);
        await _context.SaveChangesAsync();
        
        return await _context.Questionnaires.FindAsync(questionnaire?.Id);
    }
}