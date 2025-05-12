using System;
using BiodiversityCloudApp.Common;
using BiodiversityCloudApp.Controllers;
using BiodiversityCloudApp.DTOs.ObservationRecords;
using BiodiversityCloudApp.DTOs.Observations;
using BiodiversityCloudApp.DTOs;
using BiodiversityCloudApp.Repositories;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;

QuestPDF.Settings.License = LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);

// Configure database context
var connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING")
                      ?? "Host=localhost;Port=5432;Database=biodiversity_db;Username=biodiversity_db;Password=Bio20Diversity25;";

if (!Directory.Exists(AppPaths.PhotoUploadFolder))
    Directory.CreateDirectory(AppPaths.PhotoUploadFolder);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Register Repositories
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IObservationRepository, ObservationRepository>();
builder.Services.AddScoped<IPhotoRepository, PhotoRepository>();
builder.Services.AddScoped<IObservationRecordRepository, ObservationRecordRepository>();
builder.Services.AddTransient<MicroObservationReportDocument>();
builder.Services.AddTransient<List<ObservationRecordDto>>();
builder.Services.AddTransient<Dictionary<Guid, AnimalDto>>();
builder.Services.AddTransient<Dictionary<Guid, ObservationDto>>();
// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Biodiversity API",
        Version = "v1"
    });
});

var app = builder.Build();

app.UseDeveloperExceptionPage();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Biodiversity API V1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
