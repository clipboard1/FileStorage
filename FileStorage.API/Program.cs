using FileStorage.Application.Abstractions;
using FileStorage.Application.Services;
using FileStorage.Infrastructure.Storage.Abstractions;
using FileStorage.Infrastructure.Storage.Handlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IFileHandler, FileHandler>();
builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
