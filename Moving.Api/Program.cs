using Microsoft.OpenApi.Models;
using Moving.Ai;
using Moving.Core.Repositories.Abstractions;
using Moving.Core.Services;
using Moving.Infra.Repositories;
using Moving.Infra.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddAgents();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Moving API",
        Version = "v1",
        Description = "API de caixas de armazenamento e itens (persistência em memória)."
    });

    var xml = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xml);
    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});

builder.Services.AddSingleton<IStorageBoxRepository, StorageBoxRepository>();
builder.Services.AddScoped<IStorageBoxService, StorageBoxService>();
builder.Services.AddScoped<IStoredItemService, StoredItemService>();
builder.Services.AddScoped<IItemLocatorService, ItemLocatorService>();
builder.Services.AddScoped<IItemLocatorRepository, ItemLocatorRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Moving API v1");
        options.RoutePrefix = "swagger";
    });
}

app.MapControllers();

app.Run();
