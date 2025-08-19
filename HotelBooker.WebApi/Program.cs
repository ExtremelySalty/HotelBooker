using System.Net.Mime;
using HotelBooker.Persistence;
using HotelBooker.WebApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(config => config.AddServerHeader = false);
builder.Services.AddRouting(config => config.LowercaseUrls = true);
builder.Services.AddControllers(config =>
{
    config.Filters.Add(new ConsumesAttribute(MediaTypeNames.Application.Json));
    config.Filters.Add(new ProducesAttribute(MediaTypeNames.Application.Json));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(action =>
{
    action.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = builder.Configuration["ApplicationInfo:Name"],
        Version = builder.Configuration["ApplicationInfo:Version"],
        Description = "The api for booking and managing rooms and hotels."
    });
});

builder
    .Services
    .AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>();

builder
    .Services
    .AddProblemDetails();

builder
    .Services
    .ConfigureProjects(builder.Configuration)
    .ConfigureOptions(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHealthChecks("/health");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
