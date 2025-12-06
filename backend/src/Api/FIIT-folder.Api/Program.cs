using FluentValidation;
using FIIT_folder.Api.Middlware;
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
// var name = "ewweweewew.txt";
// var folder = "twewewewe-folder";
// var repository = new FileStorageRepository(
//     "YCAJEJjZUAxs4F0iCpajJG4_L",
//     "YCNbbq1t3RGwiRuNrNAnCsODmPVgWFM1s6jT201L",  
//     "my-fiit",
//     "ru-central1"
// );
// var testContent = "еееее acwqdwqdwqеееееу";
// var type = "text/plain";
//
// //await StorageTester.TestSaveInRepository(repository, testContent, type, name, folder); //Запускать через консоль
//
// await StorageTester.TestDeleteFile(name, folder);
//Console.WriteLine("================");
//это мое


//await TesterMongoDB.CreateMaterial();
await TesterMongoDB.DeleteStudyMaterial();

app.Run();