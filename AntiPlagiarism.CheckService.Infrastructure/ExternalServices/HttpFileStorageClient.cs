using System.Net;
using System.Net.Http.Json;
using AntiPlagiarism.CheckService.UseCases.Exceptions;
using AntiPlagiarism.Shared.Dto;
using AntiPlagiarism.Shared.Interfaces;

namespace AntiPlagiarism.CheckService.Infrastructure.ExternalServices;

internal sealed class HttpFileStorageClient : IFileStorageClient
{
    private readonly HttpClient _http;

    public HttpFileStorageClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<FileMeta> UploadAsync(
        Stream file,
        string fileName,
        CancellationToken ct = default)
    {
        try
        {
            using var content = new MultipartFormDataContent();

            var fileContent = new StreamContent(file);
            // по-хорошему можно настроить Headers.ContentType, но для задания достаточно так
            content.Add(fileContent, "file", fileName);

            using var response = await _http.PostAsync("/files", content, ct);

            if (!response.IsSuccessStatusCode)
            {
                // Любая неуспешная попытка — это проблема со StorageService
                var status = (int)response.StatusCode;
                var reason = response.ReasonPhrase ?? "Unknown error";

                throw new StorageUnavailableException(
                    $"Storage service returned HTTP {status} ({reason}) when uploading file.");
            }

            var meta = await response.Content.ReadFromJsonAsync<FileMeta>(cancellationToken: ct);
            if (meta is null)
                throw new StorageUnavailableException("Storage service returned an empty response when uploading file.");

            return meta;
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            // если клиент отменил запрос — пробрасываем отмену дальше
            throw;
        }
        catch (HttpRequestException ex)
        {
            // проблемы сети / DNS / таймауты и т.п.
            throw new StorageUnavailableException("Storage service is unavailable.", ex);
        }
    }

    public async Task<Stream?> DownloadAsync(string fileId, CancellationToken ct = default)
    {
        try
        {
            using var response = await _http.GetAsync($"/files/{fileId}", ct);

            if (!response.IsSuccessStatusCode)
            {
                // 404 — это просто "файл не найден", возвращаем null
                if (response.StatusCode == HttpStatusCode.NotFound)
                    return null;

                var status = (int)response.StatusCode;
                var reason = response.ReasonPhrase ?? "Unknown error";

                throw new StorageUnavailableException(
                    $"Storage service returned HTTP {status} ({reason}) when downloading file.");
            }

            // поток нельзя оборачивать в using, его вернёт вызывающий код
            return await response.Content.ReadAsStreamAsync(ct);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            throw;
        }
        catch (HttpRequestException ex)
        {
            throw new StorageUnavailableException("Storage service is unavailable.", ex);
        }
    }
}
