using HealthQ_API.Context;
using HealthQ_API.Entities;
using Hl7.Fhir.Model;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace HealthQ_API.Repositories;

public class QuestionnaireRepository : IQuestionnaireRepository
{
    private readonly HealthqDbContext _context;

    public QuestionnaireRepository(HealthqDbContext context)
    {
        _context = context;
    }


    public async Task<QuestionnaireModel?> GetQuestionnaireAsync(Guid id, CancellationToken ct)
    {
        return await _context.Questionnaires.FindAsync([id], cancellationToken: ct);
    }

    public async Task<IEnumerable<QuestionnaireModel>> GetQuestionnairesByOwnerAsync(string doctorEmail, CancellationToken ct)
    {
        var questionnaires =  await _context.Questionnaires
            .Where(q => q.OwnerId == doctorEmail)
            .ToListAsync(ct);
        
        return questionnaires;
    }

    public async Task<IEnumerable<QuestionnaireModel>> GetQuestionnairesByDoctorAndPatientAsync(
        string doctorEmail,
        string patientEmail,
        CancellationToken ct)
    {
        
        var doctorQuestionnaireIds = await _context.Questionnaires
            .Where(q => q.OwnerId == doctorEmail)
            .Select(q => q.Id)
            .ToListAsync(cancellationToken: ct);

        var patientQuestionnaires = await _context.PatientQuestionnaire
            .Where(pq => pq.PatientId == patientEmail && doctorQuestionnaireIds.Contains(pq.QuestionnaireId))
            .Select(pq => pq.Questionnaire)
            .ToListAsync(cancellationToken: ct);
        
        return patientQuestionnaires;
    }
    
    public async Task UpdateQuestionnaireAsync(QuestionnaireModel questionnaire, CancellationToken ct)
    {
        _context.Questionnaires.Update(questionnaire);
        await _context.SaveChangesAsync(ct);
    }

    public async Task CreateQuestionnaireAsync(QuestionnaireModel questionnaire, CancellationToken ct)
    {
        await _context.Questionnaires.AddAsync(questionnaire, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteQuestionnaireAsync(Guid id, CancellationToken ct)
    {
        var questionnaire = await _context.Questionnaires.FindAsync([id], cancellationToken: ct);
        if (questionnaire != null)
        {
            _context.Questionnaires.Remove(questionnaire);
            await _context.SaveChangesAsync(ct);
        }
    }
}