using HealthQ_API.Entities;
using HealthQ_API.Entities.Auxiliary;
using Task = System.Threading.Tasks.Task;

namespace HealthQ_API.Repositories.Interfaces;

public interface IQuestionnaireRepository
{
    public Task<QuestionnaireModel?> GetQuestionnaireAsync(Guid id, CancellationToken ct);
    public Task<IEnumerable<QuestionnaireModel>> GetQuestionnairesByOwnerAsync(string doctorEmail, CancellationToken ct);
    public Task<IEnumerable<QuestionnaireModel>> GetQuestionnairesByDoctorAndPatientAsync(
        string doctorEmail,
        string patientEmail,
        CancellationToken ct);
    public Task UpdateQuestionnaireAsync(QuestionnaireModel questionnaire, CancellationToken ct);
    public Task CreateQuestionnaireAsync(QuestionnaireModel questionnaire, CancellationToken ct);
    public Task DeleteQuestionnaireAsync(Guid id, CancellationToken ct);
    public Task CreatePatientQuestionnaireAsync(PatientQuestionnaire patientQuestionnaire, CancellationToken ct);
    // public Task UploadFileAsync(byte[] file, Guid questionnaireId, CancellationToken ct);
}