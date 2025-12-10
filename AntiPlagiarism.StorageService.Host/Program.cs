using AntiPlagiarism.StorageService.Infrastructure;
using AntiPlagiarism.StorageService.Infrastructure.Data;
using AntiPlagiarism.StorageService.UseCases;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace AntiPlagiarism.StorageService.Host;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // 1. Application / UseCases
        builder.Services.AddUseCases();

        // 2. Infrastructure (SQLite + репозиторий)
        builder.Services.AddInfrastructure(builder.Configuration);

        // 3. Presentation (контроллеры)
        builder.Services.AddControllers();

        // 4. Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Storage Service API",
                Version = "v1"
            });
        });

        var app = builder.Build();

        // 5. Автоматические миграции
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<StorageDbContext>();
            db.Database.Migrate();
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Storage Service API v1");
                c.RoutePrefix = "swagger";
            });
        }


        app.UseRouting();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
