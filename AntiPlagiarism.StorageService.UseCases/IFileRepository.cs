using AntiPlagiarism.StorageService.Entities;

namespace AntiPlagiarism.StorageService.UseCases;

public interface IFileRepository
{
    Task<StoredFile> SaveAsync(
        Stream fileStream,
        string fileName,
        CancellationToken ct = default);

    Task<(StoredFile? meta, Stream? content)> GetAsync(
        Guid id,
        CancellationToken ct = default);
}