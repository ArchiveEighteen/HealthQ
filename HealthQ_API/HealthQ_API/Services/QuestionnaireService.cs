using System.Text.Json;
using HealthQ_API.Context;
using HealthQ_API.Entities;
using HealthQ_API.Entities.Auxiliary;
using HealthQ_API.Repositories;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Microsoft.EntityFrameworkCore;

namespace HealthQ_API.Services;

public class QuestionnaireService
{
    private readonly IQuestionnaireRepository _questionnaireRepository;
    private readonly IPatientRepository _patientRepository;

    public QuestionnaireService(
        IQuestionnaireRepository questionnaireRepository,
        IPatientRepository patientRepository)
    {
        _questionnaireRepository = questionnaireRepository;
        _patientRepository = patientRepository;
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

    public async Task<QuestionnaireModel> AddSurveyAsync(JsonElement questionnaireJson, CancellationToken ct)
    {
        
        var parse = new FhirJsonParser();
            
        var questionnaire = await parse.ParseAsync<Questionnaire>(questionnaireJson.GetRawText());
        if (questionnaire == null)
            throw new InvalidCastException("Invalid questionnaire structure");
            
        var questionnaireModel = new QuestionnaireModel
        {
            OwnerId = questionnaire.Publisher,
            QuestionnaireContent = questionnaireJson.GetRawText(),
            Id = Guid.Parse(questionnaire.Id),
        };
        
        await _questionnaireRepository.CreateQuestionnaireAsync(questionnaireModel, ct);
        return questionnaireModel;
    }
    
    public async Task<QuestionnaireModel?> UpdateSurveyAsync(JsonElement questionnaireJson, CancellationToken ct)
    {
        var parse = new FhirJsonParser();
            
        var questionnaire = await parse.ParseAsync<Questionnaire>(questionnaireJson.GetRawText());
        if (questionnaire == null)
            throw new InvalidCastException("Invalid questionnaire structure");
            
        var questionnaireModel = new QuestionnaireModel
        {
            OwnerId = questionnaire.Publisher,
            QuestionnaireContent = questionnaireJson.GetRawText(),
            Id = Guid.Parse(questionnaire.Id),
        };
        
        await _questionnaireRepository.UpdateQuestionnaireAsync(questionnaireModel, ct);
        return questionnaireModel;
    }
    
    
    public async Task<QuestionnaireModel?> AssignToPatientAsync(JsonElement questionnaireJson, string patientEmail, CancellationToken ct)
    {
        var parse = new FhirJsonParser();
            
        var questionnaire = await parse.ParseAsync<Questionnaire>(questionnaireJson.GetRawText());
        if (questionnaire == null)
            throw new InvalidCastException("Invalid questionnaire structure");

        var questionnaireId = Guid.Parse(questionnaire.Id);
        await _questionnaireRepository.CreatePatientQuestionnaireAsync(
            new PatientQuestionnaire
            {
                QuestionnaireId = questionnaireId,
                PatientId = patientEmail
            }, 
            ct);
        
        return await _questionnaireRepository.GetQuestionnaireAsync(questionnaireId, ct);
    }

    public async Task<List<Questionnaire?>> GetQuestionnairesByPatientAsync(string patientEmail, CancellationToken ct)
    {
        
        var patient = await _patientRepository.GetPatientWithQuestionnairesAsync(patientEmail, ct);
        if (patient == null)
            throw new NullReferenceException("Patient not found");
            
        FhirJsonParser parser = new FhirJsonParser();
        
        var questionnaires = patient.Questionnaires
            .Select(q => parser.Parse<Questionnaire>(q.QuestionnaireContent))
            .ToList();

        return questionnaires;
    }
}