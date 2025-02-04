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

    public PatientController(
        UserService userService, 
        QuestionnaireService questionnaireService)
    {
        _userService = userService;
        _questionnaireService = questionnaireService;
    }

    [HttpGet("{email}")]
    public async Task<ActionResult> GetQuestionnaires(string email, CancellationToken ct)
    {
        try
        {
            var questionnaires = await _questionnaireService.GetQuestionnairesByPatientAsync(email, ct);
            
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