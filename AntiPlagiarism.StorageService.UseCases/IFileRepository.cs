using AntiPlagiarism.StorageService.Entities;

namespace AntiPlagiarism.StorageService.UseCases;

public interface IFileRepository
{
    /// <summary>
    /// Сохранить файл и вернуть доменную сущность с метаданными.
    /// </summary>
    Task<StoredFile> SaveAsync(
        Stream fileStream,
        string fileName,
        CancellationToken ct = default);

    /// <summary>
    /// Получить метаданные и содержимое файла по Id.
    /// null, null — если файл не найден.
    /// </summary>
    Task<(StoredFile? meta, Stream? content)> GetAsync(
        Guid id,
        CancellationToken ct = default);
}