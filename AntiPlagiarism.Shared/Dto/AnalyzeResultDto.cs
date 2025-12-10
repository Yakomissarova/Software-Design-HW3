namespace AntiPlagiarism.Shared.Dto;

public record AnalyzeResultDto(
    Guid SubmissionId,
    double Similarity,
    bool IsPlagiarism,
    string Details
);