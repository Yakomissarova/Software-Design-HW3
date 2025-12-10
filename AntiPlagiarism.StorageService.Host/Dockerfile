FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AntiPlagiarism.StorageService.Host/AntiPlagiarism.StorageService.Host.csproj", "AntiPlagiarism.StorageService.Host/"]
COPY ["AntiPlagiarism.Shared/AntiPlagiarism.Shared.csproj", "AntiPlagiarism.Shared/"]
COPY ["AntiPlagiarism.StorageService.Infrastructure/AntiPlagiarism.StorageService.Infrastructure.csproj", "AntiPlagiarism.StorageService.Infrastructure/"]
COPY ["AntiPlagiarism.StorageService.UseCases/AntiPlagiarism.StorageService.UseCases.csproj", "AntiPlagiarism.StorageService.UseCases/"]
COPY ["AntiPlagiarism.StorageService.Entities/AntiPlagiarism.StorageService.Entities.csproj", "AntiPlagiarism.StorageService.Entities/"]
COPY ["AntiPlagiarism.StorageService.Presentation/AntiPlagiarism.StorageService.Presentation.csproj", "AntiPlagiarism.StorageService.Presentation/"]
RUN dotnet restore "AntiPlagiarism.StorageService.Host/AntiPlagiarism.StorageService.Host.csproj"
COPY . .
WORKDIR "/src/AntiPlagiarism.StorageService.Host"
RUN dotnet build "./AntiPlagiarism.StorageService.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AntiPlagiarism.StorageService.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AntiPlagiarism.StorageService.Host.dll"]
