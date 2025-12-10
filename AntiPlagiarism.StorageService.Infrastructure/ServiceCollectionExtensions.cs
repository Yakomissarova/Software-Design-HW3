using AntiPlagiarism.StorageService.Infrastructure.Data;
using AntiPlagiarism.StorageService.Infrastructure.Repositories;
using AntiPlagiarism.StorageService.UseCases;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AntiPlagiarism.StorageService.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("StorageDb") ?? "Data Source=storage.db";

        services.AddDbContext<StorageDbContext>(options =>
        {
            options.UseSqlite(connectionString);
        });

        services.AddScoped<IFileRepository, LocalFileRepository>();

        return services;
    }
}