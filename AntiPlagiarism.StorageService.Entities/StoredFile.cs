namespace AntiPlagiarism.StorageService.Entities;

public class StoredFile
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = default!;
    public long Size { get; set; }
    public DateTime UploadedAt { get; set; }
}