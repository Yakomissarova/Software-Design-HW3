using Microsoft.AspNetCore.Http;

namespace AntiPlagiarism.CheckService.Presentation;

// DTO только для входящего HTTP-запроса
public sealed class CreateSubmissionRequest
{
    public string StudentId { get; set; } = default!;
    public string AssignmentId { get; set; } = default!;
    public IFormFile File { get; set; } = default!;
}