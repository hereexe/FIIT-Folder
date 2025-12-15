     using FIIT_folder.Application.Interfaces;
     using FIIT_folder.Domain.Interfaces;
     using FIIT_folder.Infrastructure.Authentication;
     using FIIT_folder.Infrastructure.FileStorage;
     using FIIT_folder.Infrastructure.Persistence;
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
                     Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING")!,
                     Environment.GetEnvironmentVariable("MONGODB_DATABASE_NAME")!
                 ));

             // User Repository
             services.AddSingleton<IUserRepository>(sp =>
                 new UserMongoDB(
                     Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING")!,
                     Environment.GetEnvironmentVariable("MONGODB_DATABASE_NAME")!
                 ));

             // Yandex Cloud S3
             services.AddSingleton<IFileStorageRepository>(sp =>
                 new FileStorageRepository(
                     Environment.GetEnvironmentVariable("YANDEX_ACCESS_KEY")!,
                     Environment.GetEnvironmentVariable("YANDEX_SECRET_KEY")!,
                     Environment.GetEnvironmentVariable("YANDEX_BUCKET_NAME")!,
                     Environment.GetEnvironmentVariable("YANDEX_REGION") ?? "ru-central1"
                 ));

             // Auth
             services.AddScoped<IPasswordHasher, PasswordHasher>();
             services.AddScoped<IJwtProvider, JwtProvider>();

             // Data Seeder
             services.AddScoped<DataSeeder>();

             return services;
         }
     }