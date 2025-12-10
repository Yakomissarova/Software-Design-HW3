FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AntiPlagiarism.CheckService.Host/AntiPlagiarism.CheckService.Host.csproj", "AntiPlagiarism.CheckService.Host/"]
COPY ["AntiPlagiarism.CheckService.Infrastructure/AntiPlagiarism.CheckService.Infrastructure.csproj", "AntiPlagiarism.CheckService.Infrastructure/"]
COPY ["AntiPlagiarism.CheckService.UseCases/AntiPlagiarism.CheckService.UseCases.csproj", "AntiPlagiarism.CheckService.UseCases/"]
COPY ["AntiPlagiarism.CheckService.Entities/AntiPlagiarism.CheckService.Entities.csproj", "AntiPlagiarism.CheckService.Entities/"]
COPY ["AntiPlagiarism.Shared/AntiPlagiarism.Shared.csproj", "AntiPlagiarism.Shared/"]
COPY ["AntiPlagiarism.CheckService.Presentation/AntiPlagiarism.CheckService.Presentation.csproj", "AntiPlagiarism.CheckService.Presentation/"]
RUN dotnet restore "AntiPlagiarism.CheckService.Host/AntiPlagiarism.CheckService.Host.csproj"
COPY . .
WORKDIR "/src/AntiPlagiarism.CheckService.Host"
RUN dotnet build "./AntiPlagiarism.CheckService.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AntiPlagiarism.CheckService.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AntiPlagiarism.CheckService.Host.dll"]
