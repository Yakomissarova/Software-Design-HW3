using AntiPlagiarism.StorageService.Entities;
using AntiPlagiarism.StorageService.Infrastructure.Data;
using AntiPlagiarism.StorageService.UseCases;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AntiPlagiarism.StorageService.Infrastructure.Repositories;

public class LocalFileRepository : IFileRepository
{
    private readonly StorageDbContext _dbContext;
    private readonly string _rootPath;

    public LocalFileRepository(StorageDbContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _rootPath = configuration["Storage:RootPath"] ?? throw new InvalidOperationException("Storage:RootPath is not configured");
        Directory.CreateDirectory(_rootPath);
    }

    public async Task<StoredFile> SaveAsync(
        Stream fileStream,
        string fileName,
        CancellationToken ct = default)
    {
        var id = Guid.NewGuid();
        var storedFileName = $"{id:N}_{fileName}";
        var fullPath = Path.Combine(_rootPath, storedFileName);

        await using (var file = File.Create(fullPath))
        {
            await fileStream.CopyToAsync(file, ct);
        }

        var fileInfo = new FileInfo(fullPath);

        var entity = new StoredFile
        {
            Id = id,
            FileName = fileName,
            Size = fileInfo.Length,
            UploadedAt = DateTime.UtcNow
        };

        var dto = StoredFileMapper.ToDto(entity, fullPath);

        _dbContext.Files.Add(dto);
        await _dbContext.SaveChangesAsync(ct);

        return entity;
    }

    public async Task<(StoredFile? meta, Stream? content)> GetAsync(
        Guid id,
        CancellationToken ct = default)
    {
        var dto = await _dbContext.Files
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (dto == null)
            return (null, null);

        if (!File.Exists(dto.Path))
            return (null, null);

        var entity = StoredFileMapper.ToEntity(dto);
        var stream = File.OpenRead(dto.Path);

        return (entity, stream);
    }
}
