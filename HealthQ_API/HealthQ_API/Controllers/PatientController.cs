using System.Text.Json;
using HealthQ_API.Services;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
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
    private readonly ObservationService _observationService;

    public PatientController(
        UserService userService,
        QuestionnaireService questionnaireService,
        ClinicalImpressionService clinicalImpressionService,
        ObservationService observationService
    )
    {
        _userService = userService;
        _questionnaireService = questionnaireService;
        _clinicalImpressionService = clinicalImpressionService;
        _observationService = observationService;
    }

    [HttpGet("{email}")]
    public Task<ActionResult> GetQuestionnaires(string email, CancellationToken ct) =>
        ExecuteSafely(async () =>
        {
            var questionnaires = await _questionnaireService.GetQuestionnairesByPatientAsync(email, ct);

            return Ok(questionnaires);
        });

    [HttpPost("{questionnaireId}")]
    public Task<ActionResult> SubmitClinicalImpression(string questionnaireId, [FromBody] JsonElement jsonBody) =>
        ExecuteSafely(async () =>
        {
            var clinicalImpressionJson = jsonBody.GetProperty("clinicalImpression");
            var observationsJson = jsonBody.GetProperty("observations").EnumerateArray().ToList();
            
            await _clinicalImpressionService.SubmitClinicalImpression(questionnaireId, clinicalImpressionJson);

            var parser = new FhirJsonParser();
            var clinicalImpression = await parser.ParseAsync<ClinicalImpression>(clinicalImpressionJson.GetRawText());
            
            foreach (var observation in observationsJson)
            {
                await _observationService.AddObservationAsync(clinicalImpression.Id, observation);
            }
            
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