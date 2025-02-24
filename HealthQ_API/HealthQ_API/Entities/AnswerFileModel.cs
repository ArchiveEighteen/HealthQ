namespace HealthQ_API.Entities;

public class AnswerFileModel
{
    public int Id { get; set; }
    public string FileName { get; set; }
    public byte[] FileData { get; set; }
    public string ContentType { get; set; }
}