using DotNetEnv;
using FileStorage.Application.Abstractions;
using FileStorage.Application.Services;
using FileStorage.Infrastructure;
using FileStorage.Infrastructure.Storage.Abstractions;
using FileStorage.Infrastructure.Storage.Handlers;
using Microsoft.Extensions.Options;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.Configure<ActivityServiceSettings>(builder.Configuration.GetSection(nameof(ActivityServiceSettings)));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IFileHandler>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var path = config["Storage:Path"];

    return new FileHandler(path);
});

builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddHttpClient("InteractionsClient", (sp, client) =>
{
    var settings = sp.GetRequiredService<IOptions<ActivityServiceSettings>>().Value;
    if (!string.IsNullOrEmpty(settings.BaseUrl))
    {
        client.BaseAddress = new Uri(settings.BaseUrl);
    }
});

builder.Services.AddScoped<IInteractionsService, InteractionsService>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<ActivityServiceSettings>>().Value;
    var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient("InteractionsClient");
    return new InteractionsService(httpClient, settings);
});

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();