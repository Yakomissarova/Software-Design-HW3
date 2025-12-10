using AntiPlagiarism.Shared.Dto;
using AntiPlagiarism.StorageService.UseCases.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AntiPlagiarism.StorageService.Presentation;

[ApiController]
[Route("files")]
public class FilesController : ControllerBase
{
    private readonly ISaveFileHandler _saveFileHandler;
    private readonly IGetFileHandler _getFileHandler;

    public FilesController(
        ISaveFileHandler saveFileHandler,
        IGetFileHandler getFileHandler)
    {
        _saveFileHandler = saveFileHandler;
        _getFileHandler = getFileHandler;
    }

    /// <summary>
    /// Загрузка файла в хранилище.
    /// </summary>
    [HttpPost]
    [RequestSizeLimit(50_000_000)]
    [ProducesResponseType(typeof(FileMeta), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FileMeta>> Upload(
        IFormFile file,
        CancellationToken ct)
    {
        if (file is null || file.Length == 0)
            return BadRequest(new { error = "File is empty" });

        await using var stream = file.OpenReadStream();

        var result = await _saveFileHandler.HandleAsync(stream, file.FileName, ct);

        return CreatedAtAction(
            nameof(Download),
            new { id = result.FileId },
            result);
    }

    /// <summary>
    /// Скачивание файла по идентификатору.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Download(Guid id, CancellationToken ct)
    {
        var result = await _getFileHandler.HandleAsync(id, ct);
        if (result is null)
            return NotFound(new { error = "File not found" });

        var (meta, content) = result.Value;
        return File(content, "application/octet-stream", meta.FileName);
    }
}