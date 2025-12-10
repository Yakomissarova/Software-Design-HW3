using AntiPlagiarism.Shared.Dto;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AntiPlagiarism.CheckService.UseCases.Interfaces;

public interface IAnalyzeHandler
{
    Task<AnalyzeResultDto> HandleAsync(
        Guid submissionId,
        CancellationToken ct = default);
}
