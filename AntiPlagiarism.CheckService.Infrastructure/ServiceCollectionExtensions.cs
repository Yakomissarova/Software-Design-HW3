using AntiPlagiarism.CheckService.Infrastructure.Data;
using AntiPlagiarism.CheckService.Infrastructure.ExternalServices;
using AntiPlagiarism.CheckService.Infrastructure.Repositories;
using AntiPlagiarism.CheckService.UseCases.Interfaces;
using AntiPlagiarism.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AntiPlagiarism.CheckService.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // --- БД (PostgreSQL) ---
        var connectionString = configuration.GetConnectionString("DefaultConnection")
                               ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured");

        services.AddDbContext<AppDbContext>(opts =>
        {
            opts.UseSqlite(connectionString);
        });

        services.AddScoped<ISubmissionRepository, EfSubmissionRepository>();

        // --- Внешний сервис хранения файлов (StorageService) ---
        var storageBaseUrl = configuration["StorageService:BaseUrl"]
                             ?? throw new InvalidOperationException("StorageService:BaseUrl is not configured");

        services.AddHttpClient<IFileStorageClient, HttpFileStorageClient>(client =>
        {
            client.BaseAddress = new Uri(storageBaseUrl);
        });

        return services;
    }
}