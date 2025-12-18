using FluentValidation;
using FIIT_folder.Api.Middlware;
using FIIT_folder.Application;
using FIIT_folder.Infrastructure;
using DotNetEnv;

//потом удалить
using FIIT_folder.Infrastructure.FileStorage;
using FIIT_folder.Infrastructure;
using FIIT_folder.Infrastructure.Test;

//до сюда

Env.Load();

var builder = WebApplication.CreateBuilder(args);

//add env 
builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Регистрация Application и Infrastructure слоёв
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//TODO: Переделать нормально. Разрешить не всем.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// Инициализация данных при запуске
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    await seeder.SeedAsync();
}

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }))
   .AllowAnonymous();

app.MapControllers();

app.Run();

//потом удалить
var name = "Экзамен.txt";
var folder = "Матан";
var repository = new FileStorageRepository(
    "YCAJEJjZUAxs4F0iCpajJG4_L",
    "YCNbbq1t3RGwiRuNrNAnCsODmPVgWFM1s6jT201L",  
    "my-fiit",
    "ru-central1"
);
var testContent = "бебебе";
var type = "text/plain";
 
//await StorageTester.TestSaveInRepository(repository, testContent, type, name, folder); //Запускать через консоль

//await StorageTester.TestDeleteFile(repository, name, folder);
//await StorageTester.TestGetFile(repository, name, folder);
//Console.WriteLine("================");
//это мое


await TesterMongoDB.CreateMaterial();
//await TesterMongoDB.DeleteStudyMaterial();
// await TesterMongoDB.GetByIdMaterial(new Guid(
//     "5cf635fe-7f4e-45a1-89e6-71e30c702dab"));
//await TesterMongoDB.GetByNameMaterial("Матан");


//до сюда