using HealthQ_API.Context;
using HealthQ_API.Entities;
using HealthQ_API.Entities.Auxiliary;
using HealthQ_API.Repositories;
using Hl7.Fhir.Model;
using Microsoft.EntityFrameworkCore;

namespace HealthQ_API.Services;

public class QuestionnaireService
{
    private readonly IQuestionnaireRepository _questionnaireRepository;

    public QuestionnaireService(
        IQuestionnaireRepository questionnaireRepository)
    {
        _questionnaireRepository = questionnaireRepository;
    }

    public async Task<IEnumerable<string>> GetAllDoctorSurveysAsync(string doctorEmail, CancellationToken ct)
    {
        return (await _questionnaireRepository.GetQuestionnairesByOwnerAsync(doctorEmail, ct))
            .Select(x => x.QuestionnaireContent).ToList();
    }
    
    public async Task<IEnumerable<string>> GetAllDoctorPatientSurveysAsync(string doctorEmail, string patientEmail, CancellationToken ct)
    {
        return (await _questionnaireRepository.GetQuestionnairesByDoctorAndPatientAsync(doctorEmail, patientEmail, ct))
            .Select(x => x.QuestionnaireContent).ToList();
    }

    public async Task<QuestionnaireModel?> AddSurveyAsync(QuestionnaireModel questionnaire, CancellationToken ct)
    {
        await _questionnaireRepository.CreateQuestionnaireAsync(questionnaire, ct);
    }
    
    public async Task<QuestionnaireModel?> UpdateSurveyAsync(QuestionnaireModel questionnaire)
    {
        _context.Questionnaires.Update(questionnaire);
        await _context.SaveChangesAsync();
        
        return await _context.Questionnaires.FindAsync(questionnaire?.Id);
    }
}