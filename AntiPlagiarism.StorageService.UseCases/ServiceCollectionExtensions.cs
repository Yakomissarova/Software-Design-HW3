using AntiPlagiarism.StorageService.UseCases.Handlers;
using AntiPlagiarism.StorageService.UseCases.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AntiPlagiarism.StorageService.UseCases;

public static class ServiceCollectionExtensions
{
    public static void AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<ISaveFileHandler, SaveFileHandler>();
        services.AddScoped<IGetFileHandler, GetFileHandler>();
    }
}