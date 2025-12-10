using AntiPlagiarism.StorageService.Entities;

namespace AntiPlagiarism.StorageService.Infrastructure.Data;

public static class StoredFileMapper
{
    public static StoredFile ToEntity(StoredFileDto dto)
    {
        return new StoredFile
        {
            Id = dto.Id,
            FileName = dto.FileName,
            Size = dto.Size,
            UploadedAt = dto.UploadedAt
        };
    }

    public static StoredFileDto ToDto(StoredFile entity, string path)
    {
        return new StoredFileDto
        {
            Id = entity.Id,
            FileName = entity.FileName,
            Size = entity.Size,
            UploadedAt = entity.UploadedAt,
            Path = path
        };
    }
}