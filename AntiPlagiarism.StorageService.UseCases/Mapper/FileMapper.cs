using AntiPlagiarism.Shared.Dto;
using AntiPlagiarism.StorageService.Entities;

namespace AntiPlagiarism.StorageService.UseCases.Mapper;

public static class FileMapper
{
    public static FileMeta ToDto(StoredFile entity)
    {
        return new FileMeta
        {
            FileId = entity.Id.ToString(),
            FileName = entity.FileName,
            Size = entity.Size,
            UploadedAt = entity.UploadedAt
        };
    }
}