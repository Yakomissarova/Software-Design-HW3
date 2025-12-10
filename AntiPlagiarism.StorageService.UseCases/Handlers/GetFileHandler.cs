using AntiPlagiarism.Shared.Dto;
using AntiPlagiarism.StorageService.UseCases.Interfaces;
using AntiPlagiarism.StorageService.UseCases.Mapper;

namespace AntiPlagiarism.StorageService.UseCases.Handlers;

public class GetFileHandler : IGetFileHandler
{
    private readonly IFileRepository _fileRepository;

    public GetFileHandler(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }

    public async Task<(FileMeta meta, Stream content)?> HandleAsync(
        Guid id,
        CancellationToken ct = default)
    {
        var (metaEntity, content) = await _fileRepository.GetAsync(id, ct);

        if (metaEntity is null || content is null)
            return null;

        var metaDto = FileMapper.ToDto(metaEntity);
        return (metaDto, content);
    }
}