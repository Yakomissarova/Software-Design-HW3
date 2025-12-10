using System;

namespace AntiPlagiarism.CheckService.Infrastructure.Data.Dto;

public sealed record SubmissionDto(
    Guid Id,
    string StudentId,
    string AssignmentId,
    string FileId,
    DateTimeOffset SubmittedAt
);
