namespace AntiPlagiarism.Shared.Dto;

public class FileMeta
{
    public string FileId { get; init; } = default!;
    public string FileName { get; init; } = default!;
    public long Size { get; init; }
    public DateTime UploadedAt { get; init; }
}