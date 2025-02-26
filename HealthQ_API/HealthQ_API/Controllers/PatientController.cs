using System.Text.Json;
using HealthQ_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthQ_API.Controllers;

[Authorize]
[Route("[controller]/[action]")]
[ApiController]
public class PatientController : BaseController
{
    private readonly UserService _userService;
    private readonly QuestionnaireService _questionnaireService;
    private readonly ClinicalImpressionService _clinicalImpressionService;

    public PatientController(
        UserService userService,
        QuestionnaireService questionnaireService,
        ClinicalImpressionService clinicalImpressionService
    )
    {
        _userService = userService;
        _questionnaireService = questionnaireService;
        _clinicalImpressionService = clinicalImpressionService;
    }

    [HttpGet("{email}")]
    public Task<ActionResult> GetQuestionnaires(string email, CancellationToken ct) =>
        ExecuteSafely(async () =>
        {
            var questionnaires = await _questionnaireService.GetQuestionnairesByPatientAsync(email, ct);

            return Ok(questionnaires);
        });

    [HttpPost("{questionnaireId}")]
    public Task<ActionResult> SubmitClinicalImpression(string questionnaireId, [FromBody] JsonElement questionnaireJson) =>
        ExecuteSafely(async () =>
        {
            await _clinicalImpressionService.SubmitClinicalImpression(questionnaireId, questionnaireJson);
            
            return Ok();
        });

// [HttpPost("{email}")]
    // public async Task<ActionResult> UploadFile(string email, CancellationToken ct)
    // {
    //     try
    //     {
    //         var questionnaires = await _questionnaireService.GetQuestionnairesByPatientAsync(email, ct);
    //         
    //         return Ok(questionnaires);
    //
    //     }
    //     catch(OperationCanceledException)
    //     {
    //         return StatusCode(StatusCodes.Status499ClientClosedRequest, "{\"message\":\"Operation was canceled\"}");
    //     }
    //     catch(Exception e)
    //     {
    //         return StatusCode(StatusCodes.Status409Conflict, $"{{\"message\":\"{e.Message}\"}}");
    //     }
    // }
}