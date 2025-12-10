namespace AntiPlagiarism.CheckService.UseCases.Interfaces;

public interface ICreateSubmissionHandler
{
    Task<Guid> HandleAsync(
        string studentId,
        string assignmentId,
        Stream file,
        string fileName,
        CancellationToken ct = default);
}