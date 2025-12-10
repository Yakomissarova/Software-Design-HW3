using AntiPlagiarism.CheckService.UseCases.Handlers;
using AntiPlagiarism.CheckService.UseCases.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AntiPlagiarism.CheckService.UseCases;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<ICreateSubmissionHandler, CreateSubmissionHandler>();
        services.AddScoped<IAnalyzeHandler, AnalyzeSubmissionHandler>();
        services.AddScoped<IGetReportsByAssignmentHandler, GetReportsByAssignmentHandler>();

        return services;
    }
}