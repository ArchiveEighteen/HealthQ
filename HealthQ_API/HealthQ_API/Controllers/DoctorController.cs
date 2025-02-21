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
    private readonly DoctorService _doctorService;
    private readonly UserService _userService;

    public DoctorController(QuestionnaireService questionnaireService, 
        DoctorService doctorService, UserService userService)
    {
        _questionnaireService = questionnaireService;
        _doctorService = doctorService;
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult> GetAllDoctors( CancellationToken ct)
    {
        try
        {
            var doctors = await _doctorService.GetAllDoctors( ct);
            
            var usersDto = new List<UserDTO?>();
            foreach (var doctor in doctors)
            {
                var user = await _userService.GetUserByEmailAsync(doctor.UserEmail, ct);
                if(user == null) continue;
                
                usersDto.Add(user);
            }

            return Ok(usersDto);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(StatusCodes.Status499ClientClosedRequest, "{\"message\":\"Operation was canceled\"}");
        }
        
    }
    
    [HttpGet("{email}")]
    public async Task<ActionResult> GetDoctorQuestionnaires(string email, CancellationToken ct)
    {
        try
        {
            var questionnaires = await _questionnaireService.GetAllDoctorSurveysAsync(email, ct);
            
            return Ok(questionnaires);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(StatusCodes.Status499ClientClosedRequest, "{\"message\":\"Operation was canceled\"}");
        }
    }
    
    [HttpGet("{doctorEmail}/{patientEmail}")]
    public async Task<ActionResult> GetDoctorPatientQuestionnaires(string doctorEmail, string patientEmail, CancellationToken ct)
    {
        try
        {
            var questionnaires = 
                await _questionnaireService.GetAllDoctorPatientSurveysAsync(doctorEmail, patientEmail, ct);
            
            return Ok(questionnaires);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(StatusCodes.Status499ClientClosedRequest, "{\"message\":\"Operation was canceled\"}");
        }
    }
    
    [HttpGet("{email}")]
    public async Task<ActionResult> GetAllDoctorPatients(string email, CancellationToken ct)
    {
        try
        {
            var patientsIds = await _doctorService.GetPatientIds(email, ct);
            
            var usersDto = new List<UserDTO?>();
            foreach (var patientId in patientsIds)
            {
                var user = await _userService.GetUserByEmailAsync(patientId, ct);
                if(user == null) continue;
                
                usersDto.Add(user);
            }

            return Ok(usersDto);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(StatusCodes.Status499ClientClosedRequest, "{\"message\":\"Operation was canceled\"}");
        }
    }
 
    [HttpPost]
    public async Task<ActionResult> AddByEmail([FromBody] JsonElement questionnaireJson, CancellationToken ct)
    {
        try
        {

            var userQuestionnaire = await _questionnaireService.AddSurveyAsync(questionnaireJson, ct);
            return Ok(userQuestionnaire);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(StatusCodes.Status499ClientClosedRequest, "{\"message\":\"Operation was canceled\"}");
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status409Conflict, $"{{\"message\":\"{e.Message}\"}}");
        }
    }
    
    [HttpPut]
    public async Task<ActionResult> UpdateById([FromBody] JsonElement questionnaireJson, CancellationToken ct)
    {
        try
        {
            var userQuestionnaire = await _questionnaireService.UpdateSurveyAsync(questionnaireJson, ct);
            return Ok(userQuestionnaire);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(StatusCodes.Status499ClientClosedRequest, "{\"message\":\"Operation was canceled\"}");
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status409Conflict, $"{{\"message\":\"{e.Message}\"}}");
        }
    }
    
    [HttpPut("{patientEmail}")]
    public async Task<ActionResult> AssignToPatient(string patientEmail, [FromBody] JsonElement questionnaireJson, CancellationToken ct)
    {
        try
        {
            var result = 
                await _questionnaireService.AssignToPatientAsync(questionnaireJson, patientEmail, ct);
            
            return Ok(result?.QuestionnaireContent);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(StatusCodes.Status499ClientClosedRequest, "{\"message\":\"Operation was canceled\"}");
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status409Conflict, $"{{\"message\":\"{e.Message}\"}}");
        }
    }
}