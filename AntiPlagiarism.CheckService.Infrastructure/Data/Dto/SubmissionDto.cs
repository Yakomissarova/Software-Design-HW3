namespace AntiPlagiarism.CheckService.Infrastructure.Data.Dto;

public sealed record SubmissionDto(
    Guid Id,
    string StudentId,
    string AssignmentId,
    string FileId,
    string ContentHash,
    DateTimeOffset SubmittedAt
);
