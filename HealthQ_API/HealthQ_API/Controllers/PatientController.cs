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

    public PatientController(
        UserService userService, 
        QuestionnaireService questionnaireService
    )
    {
        _userService = userService;
        _questionnaireService = questionnaireService;
    }

    [HttpGet("{email}")]
    public Task<ActionResult> GetQuestionnaires(string email, CancellationToken ct) =>
        ExecuteSafely(async () =>
        {
                var questionnaires = await _questionnaireService.GetQuestionnairesByPatientAsync(email, ct);

                return Ok(questionnaires);
        });
}