using CoreWCF;
using CoreWCF.Configuration;
using CoreWCF.Description;
using FileStorage.Application.Abstractions;
using FileStorage.Application.Services;
using FileStorage.Infrastructure.Database;
using FileStorage.Infrastructure.Database.Abstractions;
using FileStorage.Infrastructure.Database.Repositories;
using FileStorage.Infrastructure.Storage.Abstractions;
using FileStorage.Infrastructure.Storage.Handlers;
using FileStorage.SOAP.Abstractions;
using FileStorage.SOAP.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string? connectionString = builder.Configuration.GetConnectionString(nameof(ApplicationDbContext));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IFileMetadataRepository, FileMetadataRepository>();
builder.Services.AddScoped<IFileHandler, FileHandler>();
builder.Services.AddScoped<IFileMetadataService, FileMetadataService>();
builder.Services.AddScoped<IFileSoapService, FileSoapService>();

builder.Services.AddServiceModelServices();
builder.Services.AddServiceModelMetadata();
builder.Services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

var app = builder.Build();

app.UseServiceModel(builder =>
{
    builder.AddService<FileSoapService>();

    builder.AddServiceEndpoint<FileSoapService, IFileSoapService>(
        new BasicHttpBinding(),
        "/FileService.svc"
    );

    var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
    serviceMetadataBehavior.HttpGetEnabled = true;
});


app.Run();
