using AntiPlagiarism.Shared.Interfaces;
using AntiPlagiarism.CheckService.UseCases.Interfaces;
using AntiPlagiarism.CheckService.Entities;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AntiPlagiarism.CheckService.UseCases.Handlers;

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
        // 1. Загружаем файл в StorageService
        var meta = await _storageClient.UploadAsync(file, fileName, ct);

        // 2. Создаём доменную сущность Submission
        var submission = new Submission(studentId, assignmentId, meta.FileId);

        // 3. Сохраняем в БД через репозиторий
        await _repo.AddAsync(submission, ct);

        // 4. Возвращаем Id сдачи
        return submission.Id;
    }
}