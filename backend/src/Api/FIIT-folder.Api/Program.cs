using FluentValidation;
using FIIT_folder.Api.Middlware;
using FIIT_folder.Domain.Entities;
using FIIT_folder.Domain.Value_Object;
using FIIT_folder.Infrastructure.Test;
using FIIT_folder.Infrastructure.FileStorage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll"); //для теста, потом уберу 

app.UseAuthorization();

app.MapControllers();

//это мое
 // var name = "Экзаме111н.txt";
 // var folder = "Матан";
 // var repositoryFileStorage = new FileStorageRepository(
 //     "YCAJEJjZUAxs4F0iCpajJG4_L",
 //     "YCNbbq1t3RGwiRuNrNAnCsODmPVgWFM1s6jT201L",  
 //     "my-fiit",
 //     "ru-central1"
 // );
 // var testContent = "беб11111ебе";
 // var type = "text/plain";
 
//await StorageTester.TestSaveInRepository(repository, testContent, type, name, folder); //Запускать через консоль

//await StorageTester.TestDeleteFile(repository, name, folder);
//await StorageTester.TestGetFile(repository, name, folder);
//Console.WriteLine("================");
//это мое

var connectionString = "mongodb://localhost:27017";
var databaseName = "newbd";
        
var repositoryMongoDB = new MaterialMongoDB(connectionString, databaseName);

 var material = new StudyMaterial(
     new MaterialName("Матан"), 
     new SubjectId(Guid.NewGuid()), 
     new UserId(Guid.NewGuid()), 
     new StudyYear(2023), 
     new MaterialSize(2000), 
     MaterialType.Colloquium, 
     new ResourceLocation("/путь/к/файлу.pdf")
 );

//await TesterMongoDB.CreateMaterial(repositoryMongoDB, material);
//await TesterMongoDB.DeleteStudyMaterial(repositoryMongoDB, 
//    new Guid("e73a1768-ebb3-4edc-8766-2a3067be1698"));
await TesterMongoDB.GetByIdMaterial(repositoryMongoDB, new Guid(
     "791cb503-2daf-4a36-8b56-05b201e7bf15"));
//await TesterMongoDB.GetByNameMaterial(repositoryMongoDB, "Матан");

app.Run();