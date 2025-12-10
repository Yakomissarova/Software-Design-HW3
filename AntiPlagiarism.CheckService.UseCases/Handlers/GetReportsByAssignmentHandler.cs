using AntiPlagiarism.Shared.Dto;
using AntiPlagiarism.CheckService.UseCases.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AntiPlagiarism.CheckService.UseCases.Handlers;

internal sealed class GetReportsByAssignmentHandler : IGetReportsByAssignmentHandler
{
    private readonly ISubmissionRepository _repo;

    public GetReportsByAssignmentHandler(ISubmissionRepository repo)
    {
        _repo = repo;
    }

    public async Task<IReadOnlyList<AnalyzeResultDto>> HandleAsync(
        string assignmentId,
        CancellationToken ct = default)
    {
        var submissions = await _repo.GetByAssignmentAsync(assignmentId, ct);

        var results = new List<AnalyzeResultDto>(submissions.Count);

        foreach (var submission in submissions)
        {
            bool isPlagiarism = submissions.Any(s =>
                s.Id != submission.Id &&
                s.FileId == submission.FileId &&
                s.SubmittedAt < submission.SubmittedAt);

            double similarity = isPlagiarism ? 1.0 : 0.0;

            var dto = new AnalyzeResultDto(
                submission.Id,
                similarity,
                isPlagiarism,
                isPlagiarism ? "Possible plagiarism: same file submitted earlier" : "No plagiarism detected");

            results.Add(dto);
        }

        return results;
    }
}