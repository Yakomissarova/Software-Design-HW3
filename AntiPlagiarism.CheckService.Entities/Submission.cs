namespace AntiPlagiarism.CheckService.Entities;

public sealed class Submission
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string StudentId { get;}
    public string AssignmentId { get;}
    public string FileId { get;}
    public string ContentHash { get; init; }
    public DateTimeOffset SubmittedAt { get; init; }

    public Submission(string studentId, string assignmentId, string fileId, string contentHash)
    {
        StudentId = studentId;
        AssignmentId = assignmentId;
        FileId = fileId;
        ContentHash = contentHash;
        SubmittedAt = DateTimeOffset.UtcNow;
    }
}
