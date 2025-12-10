using AntiPlagiarism.Shared.Dto;

namespace AntiPlagiarism.CheckService.UseCases.Interfaces;

public interface IGetReportsByAssignmentHandler
{
    Task<IReadOnlyList<AnalyzeResultDto>> HandleAsync(
        string assignmentId,
        CancellationToken ct = default);
}