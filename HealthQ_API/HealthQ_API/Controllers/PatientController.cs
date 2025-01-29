using HealthQ_API.Services;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthQ_API.Controllers;

[Authorize]
[Route("[controller]/[action]")]
[ApiController]
public class PatientController : ControllerBase
{
    private readonly UserService _userService;
    private readonly QuestionnaireService _questionnaireService;
    private readonly PatientService _patientService;

    public PatientController(
        UserService userService, 
        QuestionnaireService questionnaireService, 
        PatientService patientService)
    {
        _userService = userService;
        _questionnaireService = questionnaireService;
        _patientService = patientService;
    }

    [HttpGet("{email}")]
    public async Task<ActionResult> GetQuestionnaires(string email, CancellationToken ct)
    {
        try
        {
            var patient = await _patientService.GetPatientAsync(email, ct);
            if (patient == null) return NotFound();
            
            FhirJsonParser parser = new FhirJsonParser();
            
            var questionnaires = patient.Questionnaires
                .Select(q => parser.Parse<Questionnaire>(q.QuestionnaireContent))
                .ToList();
            
            return Ok(questionnaires);

        }
        catch(OperationCanceledException)
        {
            return StatusCode(StatusCodes.Status499ClientClosedRequest, "{\"message\":\"Operation was canceled\"}");
        }
        catch(Exception e)
        {
            return StatusCode(StatusCodes.Status409Conflict, $"{{\"message\":\"{e.Message}\"}}");
        }
    }
}