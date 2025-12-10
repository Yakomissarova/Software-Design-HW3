using AntiPlagiarism.Shared.Dto;
using AntiPlagiarism.StorageService.UseCases.Interfaces;
using AntiPlagiarism.StorageService.UseCases.Mapper;

namespace AntiPlagiarism.StorageService.UseCases.Handlers;

public class SaveFileHandler : ISaveFileHandler
{
    private readonly IFileRepository _fileRepository;

    public SaveFileHandler(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }

    public async Task<FileMeta> HandleAsync(
        Stream fileStream,
        string fileName,
        CancellationToken ct = default)
    {
        // Доменная операция: репозиторий сам решает, как сохранить файл
        var storedFile = await _fileRepository.SaveAsync(fileStream, fileName, ct);

        // Конвертируем домен → DTO для отдачи наружу
        return FileMapper.ToDto(storedFile);
    }
}