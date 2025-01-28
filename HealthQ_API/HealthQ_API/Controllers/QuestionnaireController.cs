using System.Text.Json;
using HealthQ_API.Entities;
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
public class QuestionnaireController : ControllerBase
{
    private readonly QuestionnaireService _questionnaireService;

    public QuestionnaireController(QuestionnaireService questionnaireService)
    {
        _questionnaireService = questionnaireService;
    }
    
    [HttpGet]
    public async Task<ActionResult> GetByEmail([FromBody] string email)
    {
        try
        {
            var questionnaires = await _questionnaireService.GetAllSurveysAsync(email);
            
            return Ok(questionnaires);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
 
    [HttpPost("{email}")]
    public async Task<ActionResult> AddByEmail(string email, [FromBody] JsonElement questionnaireJson)
    {
        try
        {
            var parser = new FhirJsonParser();
            var serializer = new FhirJsonSerializer();
            
            var questionnaire = await parser.ParseAsync<Questionnaire>(questionnaireJson.GetRawText());
            
                var questionnaireModel = new QuestionnaireModel
                {
                    QuestionnaireContent = questionnaireJson.GetRawText(),
                    Id = Guid.NewGuid(),
                };

                var userQuestionnaire = await _questionnaireService.AddSurveyAsync(email, questionnaireModel);
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
}