using AntiPlagiarism.Shared.Dto;

namespace AntiPlagiarism.StorageService.UseCases.Interfaces;

public interface ISaveFileHandler
{
    Task<FileMeta> HandleAsync(
        Stream fileStream,
        string fileName,
        CancellationToken ct = default);
}