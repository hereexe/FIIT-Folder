FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

LABEL maintainer="your-email@example.com"
LABEL description="FIIT-folder API - ASP.NET Core"

# Copy .csproj files
COPY src/Api/FIIT-folder.Api/FIIT-folder.Api.csproj src/Api/FIIT-folder.Api/
COPY src/Api/FIIT-folder.Application/FIIT-folder.Application.csproj src/Api/FIIT-folder.Application/
COPY src/Core/FIIT-folder.Domain/FIIT-folder.Domain.csproj src/Core/FIIT-folder.Domain/
COPY src/Infrastructure/FIIT-folder.Infrastructure/FIIT-folder.Infrastructure.csproj src/Infrastructure/FIIT-folder.Infrastructure/

RUN dotnet restore src/Api/FIIT-folder.Api/FIIT-folder.Api.csproj

# Copy and publish
COPY . .
WORKDIR /src/src/Api/FIIT-folder.Api
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# ============ Runtime ============
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

# Install curl for healthcheck
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Non-root user
RUN useradd -m -u 1001 appuser

WORKDIR /app
COPY --from=build /app/publish .
RUN chown -R appuser:appuser /app

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=40s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

USER appuser
ENTRYPOINT ["dotnet", "FIIT-folder.Api.dll"]
