using AntiPlagiarism.Shared.Dto;

namespace AntiPlagiarism.StorageService.UseCases.Interfaces;

public interface IGetFileHandler
{
    Task<(FileMeta meta, Stream content)?> HandleAsync(
        Guid id,
        CancellationToken ct = default);
}