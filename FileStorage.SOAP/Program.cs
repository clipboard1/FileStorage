using CoreWCF;
using CoreWCF.Configuration;
using CoreWCF.Description;
using FileStorage.Application.Abstractions;
using FileStorage.Application.Services;
using FileStorage.Infrastructure.Storage.Abstractions;
using FileStorage.Infrastructure.Storage.Handlers;
using FileStorage.SOAP.Abstractions;
using FileStorage.SOAP.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IFileHandler, FileHandler>();
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
