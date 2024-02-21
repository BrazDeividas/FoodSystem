using FoodSystemAPI.Entities;
using FoodSystemAPI.Handlers;
using FoodSystemAPI.Helpers;
using FoodSystemAPI.Infrastructure;
using FoodSystemAPI.Repositories;
using FoodSystemAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDataAccessServices(builder.Configuration["ConnectionStrings:SqlServer"]!);
builder.Services.AddCustomServices();
builder.Services.AddCustomAutoMapper();
builder.Services.AddExceptionHandler();

builder.Services.AddHttpAPIClient("api-1", (httpClient) =>
{
    httpClient.BaseAddress = new(builder.Configuration["APIs:api-1:Url"]!);
    httpClient.AddRapidAPIHeaders(builder.Configuration["APIs:api-1:Host"]!, builder.Configuration["APIs:api-1:Key"]!);
});

builder.Services.AddHttpAPIClient("api-internal", (httpClient) =>
{
    httpClient.BaseAddress = new(builder.Configuration["APIs:api-internal:Url"]!);
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseExceptionHandler();

app.Run();
