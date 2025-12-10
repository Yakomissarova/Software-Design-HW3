using AntiPlagiarism.CheckService.UseCases.Interfaces;
using AntiPlagiarism.Shared.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AntiPlagiarism.CheckService.Presentation;

[ApiController]
[Route("assignments")]
public class AssignmentsController : ControllerBase
{
    private readonly IGetReportsByAssignmentHandler _reportsHandler;

    public AssignmentsController(IGetReportsByAssignmentHandler reportsHandler)
    {
        _reportsHandler = reportsHandler;
    }

    /// <summary>
    /// Получить отчёты по всем сдачам по заданию.
    /// </summary>
    [HttpGet("{assignmentId}/reports")]
    [ProducesResponseType(typeof(IReadOnlyList<AnalyzeResultDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<AnalyzeResultDto>>> GetReports(
        string assignmentId,
        CancellationToken ct)
    {
        var reports = await _reportsHandler.HandleAsync(assignmentId, ct);
        return Ok(reports);
    }
}