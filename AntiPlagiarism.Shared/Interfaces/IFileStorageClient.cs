namespace AntiPlagiarism.Shared.Interfaces;
using Dto;

public interface IFileStorageClient
{
    Task<FileMeta> UploadAsync(Stream file, string fileName, CancellationToken ct = default);
    Task<Stream?> DownloadAsync(string fileId, CancellationToken ct = default);
}