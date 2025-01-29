using HealthQ_API.Context;
using HealthQ_API.Entities;
using HealthQ_API.Entities.Auxiliary;
using Hl7.Fhir.Model;
using Microsoft.EntityFrameworkCore;

namespace HealthQ_API.Services;

public class PatientQuestionnaireService
{
    private readonly HealthqDbContext _context;
    
    public PatientQuestionnaireService(HealthqDbContext context)
    {
        _context = context;
    }
    
    public async Task<QuestionnaireModel?> AssignPatientAsync(PatientQuestionnaire patientQuestionnaire)
    {
        await _context.PatientQuestionnaire.AddAsync(patientQuestionnaire);
        await _context.SaveChangesAsync();
        
        return await _context.Questionnaires.FindAsync(patientQuestionnaire);
    }
}