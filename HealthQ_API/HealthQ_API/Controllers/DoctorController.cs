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
public class DoctorController : BaseController
{
    private readonly QuestionnaireService _questionnaireService;
    private readonly DoctorService _doctorService;
    private readonly AdminService _adminService;
    private readonly UserService _userService;
    private readonly ClinicalImpressionService _clinicalImpressionService;
    private readonly ObservationService _observationService;

    public DoctorController(
        QuestionnaireService questionnaireService,
        DoctorService doctorService,
        AdminService adminService,
        UserService userService,
        ClinicalImpressionService clinicalImpressionService,
        ObservationService observationService)
    {
        _questionnaireService = questionnaireService;
        _doctorService = doctorService;
        _adminService = adminService;
        _userService = userService;
        _clinicalImpressionService = clinicalImpressionService;
        _observationService = observationService;
    }

    [HttpGet]
    public Task<ActionResult> GetAllDoctors(CancellationToken ct) =>
        ExecuteSafely(async () =>
        {
            var doctors = await _doctorService.GetAllDoctors(ct);

            var usersDto = new List<UserDTO?>();
            foreach (var doctor in doctors)
            {
                var user = await _userService.GetUserByEmailAsync(doctor.UserEmail, ct);
                if (user == null) continue;

                usersDto.Add(user);
            }

            return Ok(usersDto);

        });

    [HttpGet("{email}")]
    public Task<ActionResult> GetDoctorTemplates(string email, CancellationToken ct) =>
        ExecuteSafely(async () =>
        {
            var templates = await _questionnaireService.GetDoctorTemplatesAsync(email, ct);

            return Ok(templates);
        });

    [HttpGet("{doctorEmail}/{patientEmail}")]
    public Task<ActionResult> GetDoctorPatientQuestionnaires(string doctorEmail, string patientEmail,
        CancellationToken ct) =>
        ExecuteSafely(async () =>
        {
            var questionnaires =
                await _questionnaireService.GetAllDoctorPatientSurveysAsync(doctorEmail, patientEmail, ct);

            return Ok(questionnaires);
        });

    [HttpGet("{email}")]
    public Task<ActionResult> GetAllDoctorPatients(string email, CancellationToken ct) =>
        ExecuteSafely(async () =>
        {
            var patientsIds = await _doctorService.GetPatientIds(email, ct);

            var usersDto = new List<UserDTO?>();
            foreach (var patientId in patientsIds)
            {
                var user = await _userService.GetUserByEmailAsync(patientId, ct);
                if (user == null) continue;

                usersDto.Add(user);
            }

            return Ok(usersDto);
        });

    [HttpPost]
    public Task<ActionResult> AddByEmail([FromBody] JsonElement questionnaireJson, CancellationToken ct) =>
        ExecuteSafely(async () =>
        {
            var userQuestionnaire = await _questionnaireService.AddSurveyAsync(questionnaireJson, ct);
            return Ok(userQuestionnaire);
        });

    [HttpPost]
    public Task<ActionResult> SaveTemplate([FromBody] JsonElement templateJson, CancellationToken ct) =>
        ExecuteSafely(async () =>
        {
            var template = await _questionnaireService.AddTemplateAsync(templateJson, ct);
            return Ok(template);
        });

    [HttpPut]
    public Task<ActionResult> UpdateById([FromBody] JsonElement questionnaireJson, CancellationToken ct) =>
        ExecuteSafely(async () =>
        {
            var userQuestionnaire = await _questionnaireService.UpdateSurveyAsync(questionnaireJson, ct);
            return Ok(userQuestionnaire);
        });

    [HttpPost("{doctorEmail}/{patientEmail}")]
    public Task<ActionResult> AssignPatient(string doctorEmail, string patientEmail, CancellationToken ct) =>
        ExecuteSafely(async () =>
        {
            await _adminService.AssignPatientToDoctor(doctorEmail, patientEmail, ct);

            return Ok();
        });

    [HttpPut("{patientEmail}")]
    public Task<ActionResult> AssignToPatient(string patientEmail, [FromBody] JsonElement questionnaireJson,
        CancellationToken ct) => 
        ExecuteSafely(async () =>
        {
            var result =
                await _questionnaireService.AssignToPatientAsync(questionnaireJson, patientEmail, ct);

            return Ok(result?.QuestionnaireContent);
        });

    [HttpGet("{email}")]
    public Task<ActionResult> GetNotOwnedPatients(string email, CancellationToken ct) => 
        ExecuteSafely(async () =>
        {
            var patients = await _doctorService.GetNotOwnedPatients(email, ct);
            return Ok(patients);
        });

    [HttpDelete("{doctorId}/{patientId}")]
    public Task<ActionResult> DeletePatient(string doctorId, string patientId, CancellationToken ct) => 
        ExecuteSafely(async () =>
        {
            await _doctorService.RemovePatient(doctorId, patientId, ct);
            return Ok();
        });
    
    [HttpDelete]
    public Task<ActionResult> DeleteById([FromBody] JsonElement questionnaireJson, CancellationToken ct) =>
        ExecuteSafely(async () =>
        {
            var userQuestionnaire =
                await _questionnaireService.DeleteSurveyAsync(questionnaireJson, ct);
            return Ok(userQuestionnaire);
        });
    
    [HttpDelete("{templateId}")]
    public Task<ActionResult> DeleteTemplate(string templateId, CancellationToken ct) =>
        ExecuteSafely(async () =>
        {
            await _questionnaireService.DeleteTemplateAsync(templateId, ct);
            return Ok();
        });
    
    [HttpGet("{patientEmail}/{questionnaireId}")]
    public Task<ActionResult> GetClinicalImpressionContent(string patientEmail, string questionnaireId, CancellationToken ct) => 
        ExecuteSafely(async () =>
        {
            var clinicalImpressionContent = await _clinicalImpressionService.GetClinicalImpressionContentByPatientAsync(questionnaireId, patientEmail);
            return Ok(clinicalImpressionContent);
        });
    
    [HttpGet("{observationId}")]
    public Task<ActionResult> GetObservationById(string observationId, CancellationToken ct) => 
        ExecuteSafely(async () =>
        {
            var observationContent = await _observationService.GetByIdAsync(Guid.Parse(observationId));
            return Ok(observationContent);
        });
}