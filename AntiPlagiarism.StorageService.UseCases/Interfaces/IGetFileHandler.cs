using AntiPlagiarism.Shared.Dto;

namespace AntiPlagiarism.StorageService.UseCases.Interfaces;

public interface IGetFileHandler
{
    /// <summary>
    /// Возвращает кортеж (метаданные, поток содержимого) или null, если файл не найден.
    /// </summary>
    Task<(FileMeta meta, Stream content)?> HandleAsync(
        Guid id,
        CancellationToken ct = default);
}