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
            content.Add(fileContent, "file", fileName);

            using var response = await _http.PostAsync("/files", content, ct);

            if (!response.IsSuccessStatusCode)
            {
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
            throw;
        }
        catch (HttpRequestException ex)
        {
            throw new StorageUnavailableException("Storage service is unavailable.", ex);
        }
    }
}
