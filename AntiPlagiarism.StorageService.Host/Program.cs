using AntiPlagiarism.StorageService.Infrastructure;
using AntiPlagiarism.StorageService.Infrastructure.Data;
using AntiPlagiarism.StorageService.UseCases;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace AntiPlagiarism.StorageService.Host;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Application / UseCases
        builder.Services.AddUseCases();

        // Infrastructure (SQLite + репозиторий)
        builder.Services.AddInfrastructure(builder.Configuration);

        // Presentation (контроллеры)
        builder.Services.AddControllers();

        // Swagger
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

        // Миграции
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
