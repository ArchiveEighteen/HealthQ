using System.Text.Json;
using HealthQ_API.Context;
using HealthQ_API.Entities;
using HealthQ_API.Entities.Auxiliary;
using HealthQ_API.Repositories;
using HealthQ_API.Repositories.Interfaces;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace HealthQ_API.Services;

public class QuestionnaireService
{
    private readonly IQuestionnaireRepository _questionnaireRepository;
    private readonly ITemplateRepository _templateRepository;
    private readonly IPatientRepository _patientRepository;

    public QuestionnaireService(
        IQuestionnaireRepository questionnaireRepository,
        IPatientRepository patientRepository,
        ITemplateRepository templateRepository)
    {
        _questionnaireRepository = questionnaireRepository;
        _patientRepository = patientRepository;
        _templateRepository = templateRepository;
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

    public async Task<List<string>> GetQuestionnairesByPatientAsync(string patientEmail, CancellationToken ct)
    {
        
        var patient = await _patientRepository.GetPatientWithQuestionnairesAsync(patientEmail, ct);
        if (patient == null)
            throw new NullReferenceException("Patient not found");
        
        var questionnaires = patient.Questionnaires
            .Select(q => q.QuestionnaireContent)
            .ToList();

        return questionnaires;
    }
    
    public async Task<QuestionnaireModel?> DeleteSurveyAsync(JsonElement questionnaireJson, CancellationToken ct)
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
        
        await _questionnaireRepository.DeleteQuestionnaireAsync(questionnaireModel.Id, ct);
        return questionnaireModel;
    }

    public async Task<Questionnaire> AddTemplateAsync(JsonElement templateJson, CancellationToken ct)
    {
        var parse = new FhirJsonParser();
            
        var template = await parse.ParseAsync<Questionnaire>(templateJson.GetRawText());
        if (template == null)
            throw new InvalidCastException("Invalid questionnaire structure");

        var templateModel = new TemplateModel
        {
            OwnerId = template.Publisher,
            QuestionnaireContent = templateJson.GetRawText(),
            Id = Guid.Parse(template.Id)
        };
        
        await _templateRepository.CreateTemplateAsync(templateModel, ct);
        return template;
    }

    public async Task<IEnumerable<string>> GetDoctorTemplatesAsync(string email, CancellationToken ct)
    {
        var ownedTemplates = await _templateRepository.GetTemplatesByOwnerAsync(email, ct);
        var sharedTemplates = await _templateRepository.GetTemplatesByOwnerAsync("shared", ct);
        
        return ownedTemplates.Union(sharedTemplates).Select(t => t.QuestionnaireContent);
    }

    public async Task DeleteTemplateAsync(string templateId, CancellationToken ct)
    {
        await _templateRepository.DeleteTemplateAsync(Guid.Parse(templateId), ct);
    }
}