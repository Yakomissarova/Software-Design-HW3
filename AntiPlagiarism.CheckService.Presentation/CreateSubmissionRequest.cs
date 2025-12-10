using Microsoft.AspNetCore.Http;

namespace AntiPlagiarism.CheckService.Presentation;

public sealed class CreateSubmissionRequest
{
    public string StudentId { get; set; } = default!;
    public string AssignmentId { get; set; } = default!;
    public IFormFile File { get; set; } = default!;
}