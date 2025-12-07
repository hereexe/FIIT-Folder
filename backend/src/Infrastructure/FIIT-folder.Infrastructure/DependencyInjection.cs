using FIIT_folder.Domain.Interfaces;
using FIIT_folder.Infrastructure.FileStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FIIT_folder.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // MongoDB
        services.AddSingleton<ISubjectRepository, SubjectMongoDB>();
        services.AddSingleton<IMaterialMongoDB>(sp =>
            new MaterialMongoDB(
                configuration["MongoDbSettings:ConnectionString"]!,
                configuration["MongoDbSettings:DatabaseName"]!
            ));

        // Yandex Cloud S3
        services.AddSingleton<IFileStorageRepository>(sp =>
            new FileStorageRepository(
                configuration["YandexCloud:AccessKey"]!,
                configuration["YandexCloud:SecretKey"]!,
                configuration["YandexCloud:BucketName"]!,
                configuration["YandexCloud:Region"] ?? "ru-central1"
            ));

        return services;
    }
}