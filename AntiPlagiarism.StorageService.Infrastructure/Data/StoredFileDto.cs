namespace AntiPlagiarism.StorageService.Infrastructure.Data;

public class StoredFileDto
{
    public Guid Id { get; set; }

    public string FileName { get; set; } = default!;
    public long Size { get; set; }

    // Физический путь к файлу на диске
    public string Path { get; set; } = default!;

    public DateTime UploadedAt { get; set; }
}