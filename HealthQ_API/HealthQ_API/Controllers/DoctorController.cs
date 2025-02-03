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
    public async Task<ActionResult> GetDoctorQuestionnaires(string email)
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
    
    [HttpGet("{doctorEmail}/{patientEmail}")]
    public async Task<ActionResult> GetDoctorPatientQuestionnaires(string doctorEmail, string patientEmail)
    {
        try
        {
            var questionnaires = await _questionnaireService.GetAllDoctorPatientSurveysAsync(doctorEmail, patientEmail);
            
            return Ok(questionnaires);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
    
    [HttpGet("{email}")]
    public async Task<ActionResult> GetAllDoctorPatients(string email, CancellationToken ct)
    {
        try
        {
            var patientsIds = await _doctorPatientService.GetAllDoctorPatientsAsync(email);
            
            var usersDto = new List<UserDTO?>();
            foreach (var patientId in patientsIds)
            {
                var user = await _userService.GetUserByEmailAsync(patientId, ct);
                if(user == null) continue;
                
                usersDto.Add(new UserDTO
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
                });
            }

            return Ok(usersDto);
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
            return Ok(result?.QuestionnaireContent);
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