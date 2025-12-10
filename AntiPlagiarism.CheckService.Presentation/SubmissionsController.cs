using System;
using System.Threading;
using System.Threading.Tasks;
using AntiPlagiarism.CheckService.UseCases.Exceptions;
using AntiPlagiarism.CheckService.UseCases.Interfaces;
using AntiPlagiarism.Shared.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AntiPlagiarism.CheckService.Presentation;

[ApiController]
[Route("submissions")]
public class SubmissionsController : ControllerBase
{
    private readonly ICreateSubmissionHandler _createSubmissionHandler;
    private readonly IAnalyzeHandler _analyzeHandler;

    public SubmissionsController(
        ICreateSubmissionHandler createSubmissionHandler,
        IAnalyzeHandler analyzeHandler)
    {
        _createSubmissionHandler = createSubmissionHandler;
        _analyzeHandler = analyzeHandler;
    }

    /// <summary>
    /// Создать новую сдачу контрольной работы.
    /// Студент, задание, файл.
    /// </summary>
    [HttpPost]
    [RequestSizeLimit(50_000_000)] // 50 MB
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<Guid>> CreateSubmission(
        [FromForm] CreateSubmissionRequest request,
        CancellationToken ct)
    {
        if (request.File is null || request.File.Length == 0)
            return BadRequest(new { error = "File is empty" });

        if (string.IsNullOrWhiteSpace(request.StudentId))
            return BadRequest(new { error = "StudentId is required" });

        if (string.IsNullOrWhiteSpace(request.AssignmentId))
            return BadRequest(new { error = "AssignmentId is required" });

        await using var stream = request.File.OpenReadStream();

        try
        {
            var submissionId = await _createSubmissionHandler.HandleAsync(
                request.StudentId,
                request.AssignmentId,
                stream,
                request.File.FileName,
                ct);

            return CreatedAtAction(
                nameof(GetSubmissionAnalysis),
                new { id = submissionId },
                submissionId);
        }
        catch (StorageUnavailableException)
        {
            // StorageService упал / недоступен / отвечает ошибкой
            return StatusCode(
                StatusCodes.Status503ServiceUnavailable,
                new { error = "Storage service is unavailable. Please try again later." });
        }
    }

    /// <summary>
    /// Получить анализ конкретной сдачи.
    /// </summary>
    [HttpGet("{id:guid}/analysis")]
    [ProducesResponseType(typeof(AnalyzeResultDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<AnalyzeResultDto>> GetSubmissionAnalysis(
        Guid id,
        CancellationToken ct)
    {
        var result = await _analyzeHandler.HandleAsync(id, ct);
        return Ok(result);
    }
}
