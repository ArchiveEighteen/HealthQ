using System.Text.Json;
using HealthQ_API.DTOs;
using HealthQ_API.Entities;
using HealthQ_API.Entities.Auxiliary;
using HealthQ_API.Services;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HealthQ_API.Controllers;

[Authorize]
[Route("[controller]/[action]")]
[ApiController]
public class DoctorController : ControllerBase
{
    private readonly QuestionnaireService _questionnaireService;
    private readonly PatientQuestionnaireService _patientQuestionnaireService;
    private readonly DoctorPatientService _doctorPatientService;
    private readonly UserService _userService;

    public DoctorController(QuestionnaireService questionnaireService, PatientQuestionnaireService patientQuestionnaireService, 
        DoctorPatientService doctorPatientService, UserService userService)
    {
        _questionnaireService = questionnaireService;
        _patientQuestionnaireService = patientQuestionnaireService;
        _doctorPatientService = doctorPatientService;
        _userService = userService;
    }
    
    [HttpGet("{email}")]
    public async Task<ActionResult> GetDoctorByEmail(string email)
    {
        try
        {
            var questionnaires = await _questionnaireService.GetAllDoctorSurveysAsync(email);
            
            return Ok(questionnaires);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
    
    [HttpGet("{email}")]
    public async Task<ActionResult> GetAllDoctorPatients(string email)
    {
        try
        {
            var patientsIds = await _doctorPatientService.GetAllDoctorPatientsAsync(email);
            
            var users = new List<UserDTO?>();
            foreach (var patientId in patientsIds)
            {
                users.Add(await _userService.GetUserByEmailAsync(patientId, CancellationToken.None));
            }

            return Ok(users);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
 
    [HttpPost]
    public async Task<ActionResult> AddByEmail([FromBody] JsonElement questionnaireJson)
    {
        try
        {
            var parse = new FhirJsonParser();
            
            var questionnaire = await parse.ParseAsync<Questionnaire>(questionnaireJson.GetRawText());

            if (questionnaire == null)
            {
                return StatusCode(StatusCodes.Status409Conflict, "Invalid questionnaire structure");
            }
            
            var questionnaireModel = new QuestionnaireModel
            {
                OwnerId = questionnaire.Publisher,
                QuestionnaireContent = questionnaireJson.GetRawText(),
                Id = Guid.Parse(questionnaire.Id),
            };

            var userQuestionnaire = await _questionnaireService.AddSurveyAsync(questionnaireModel);
            return Ok(userQuestionnaire);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(500, "Internal Server Error");
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status409Conflict, $"{{\"message\":\"{e.Message}\"}}");
        }
    }
    
    [HttpPut]
    public async Task<ActionResult> UpdateById([FromBody] JsonElement questionnaireJson)
    {
        try
        {
            var parse = new FhirJsonParser();
            
            var questionnaire = await parse.ParseAsync<Questionnaire>(questionnaireJson.GetRawText());

            if (questionnaire == null)
            {
                return StatusCode(StatusCodes.Status409Conflict, "Invalid questionnaire structure");
            }
            
            var questionnaireModel = new QuestionnaireModel
            {
                OwnerId = questionnaire.Publisher,
                QuestionnaireContent = questionnaireJson.GetRawText(),
                Id = Guid.Parse(questionnaire.Id),
            };

            var userQuestionnaire = await _questionnaireService.UpdateSurveyAsync(questionnaireModel);
            return Ok(userQuestionnaire);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(500, "Internal Server Error");
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status409Conflict, $"{{\"message\":\"{e.Message}\"}}");
        }
    }
    
    [HttpPut("{patientEmail}")]
    public async Task<ActionResult> AssignToPatient(string patientEmail, [FromBody] JsonElement questionnaireJson)
    {
        try
        {
            var parse = new FhirJsonParser();
            
            var questionnaire = await parse.ParseAsync<Questionnaire>(questionnaireJson.GetRawText());

            if (questionnaire == null)
            {
                return StatusCode(StatusCodes.Status409Conflict, "Invalid questionnaire structure");
            }

            var patientQuestionnaire = new PatientQuestionnaire()
            {
                QuestionnaireId = Guid.Parse(questionnaire.Id),
                PatientId = patientEmail
            };

            var result = await _patientQuestionnaireService.AssignPatientAsync(patientQuestionnaire);
            return Ok(result);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(500, "Internal Server Error");
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status409Conflict, $"{{\"message\":\"{e.Message}\"}}");
        }
    }
}