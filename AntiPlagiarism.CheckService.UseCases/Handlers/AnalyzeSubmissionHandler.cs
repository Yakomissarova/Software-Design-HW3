using AntiPlagiarism.Shared.Dto;
using AntiPlagiarism.CheckService.UseCases.Interfaces;

namespace AntiPlagiarism.CheckService.UseCases.Handlers;

internal sealed class AnalyzeSubmissionHandler : IAnalyzeHandler
{
    private readonly ISubmissionRepository _repo;

    public AnalyzeSubmissionHandler(ISubmissionRepository repo)
    {
        _repo = repo;
    }

    public async Task<AnalyzeResultDto> HandleAsync(
        Guid submissionId,
        CancellationToken ct = default)
    {
        var submission = await _repo.GetByIdAsync(submissionId, ct);
        if (submission is null)
            return new AnalyzeResultDto(
                submissionId,
                Similarity: 0,
                IsPlagiarism: false,
                Details: "Submission not found");

        var all = await _repo.GetByAssignmentAsync(submission.AssignmentId, ct);

        // правило: плагиат, если есть более ранняя сдача с тем же ContentHash
        bool isPlagiarism = all.Any(s =>
            s.Id != submission.Id &&
            s.ContentHash == submission.ContentHash &&
            s.SubmittedAt < submission.SubmittedAt);

        double similarity = isPlagiarism ? 1.0 : 0.0;

        return new AnalyzeResultDto(
            submission.Id,
            similarity,
            isPlagiarism,
            isPlagiarism ? "Possible plagiarism: same file submitted earlier" : "No plagiarism detected");
    }
}