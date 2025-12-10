using AntiPlagiarism.Shared.Interfaces;
using AntiPlagiarism.CheckService.UseCases.Interfaces;
using AntiPlagiarism.CheckService.Entities;
using System.Security.Cryptography;

internal sealed class CreateSubmissionHandler : ICreateSubmissionHandler
{
    private readonly ISubmissionRepository _repo;
    private readonly IFileStorageClient _storageClient;

    public CreateSubmissionHandler(
        ISubmissionRepository repo,
        IFileStorageClient storageClient)
    {
        _repo = repo;
        _storageClient = storageClient;
    }

    public async Task<Guid> HandleAsync(
        string studentId,
        string assignmentId,
        Stream file,
        string fileName,
        CancellationToken ct = default)
    {
        await using var buffer = new MemoryStream();
        await file.CopyToAsync(buffer, ct);

        // Считаем SHA256 по содержимому
        buffer.Position = 0;
        string contentHash;
        using (var sha256 = SHA256.Create())
        {
            var hashBytes = await sha256.ComputeHashAsync(buffer, ct);
            contentHash = Convert.ToHexString(hashBytes);
        }

        buffer.Position = 0;
        var meta = await _storageClient.UploadAsync(buffer, fileName, ct);

        var submission = new Submission(
            studentId,
            assignmentId,
            meta.FileId,
            contentHash);

        await _repo.AddAsync(submission, ct);
        return submission.Id;
    }
}
