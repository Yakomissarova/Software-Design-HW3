using AntiPlagiarism.Shared.Dto;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AntiPlagiarism.CheckService.UseCases.Interfaces;

public interface IGetReportsByAssignmentHandler
{
    Task<IReadOnlyList<AnalyzeResultDto>> HandleAsync(
        string assignmentId,
        CancellationToken ct = default);
}