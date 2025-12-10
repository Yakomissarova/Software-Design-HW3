namespace AntiPlagiarism.CheckService.Entities;

public sealed class Submission
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string StudentId { get; init; }
    public string AssignmentId { get; init; }
    public string FileId { get; init; }
    public DateTimeOffset SubmittedAt { get; init; }

    public Submission(string studentId, string assignmentId, string fileId)
    {
        StudentId = studentId;
        AssignmentId = assignmentId;
        FileId = fileId;
        SubmittedAt = DateTimeOffset.UtcNow;
    }
}
