using AntiPlagiarism.Shared.Dto;

namespace AntiPlagiarism.CheckService.UseCases.Interfaces;

public interface IAnalyzeHandler
{
    Task<AnalyzeResultDto> HandleAsync(
        Guid submissionId,
        CancellationToken ct = default);
}
