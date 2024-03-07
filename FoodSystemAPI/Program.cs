using FoodSystemAPI.Helpers;
using FoodSystemAPI.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDataAccessServices(builder.Configuration["ConnectionStrings:SqlServer"]!);
builder.Services.AddCustomServices();
builder.Services.AddCustomAutoMapper();
builder.Services.AddExceptionHandler();

builder.Services.AddAuthentication(options => 
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
}).AddCookie("Cookies");

builder.Services.AddHttpAPIClient("api-1", (httpClient) =>
{
    httpClient.BaseAddress = new(builder.Configuration["APIs:api-1:Url"]!);
    httpClient.AddRapidAPIHeaders(builder.Configuration["APIs:api-1:Host"]!, builder.Configuration["APIs:api-1:Key"]!);
});

builder.Services.AddHttpAPIClient("api-2", (httpClient) =>
{
    httpClient.BaseAddress = new(builder.Configuration["APIs:api-2:Url"]!);
    httpClient.AddRapidAPIHeaders(builder.Configuration["APIs:api-2:Host"]!, builder.Configuration["APIs:api-2:Key"]!);
});

builder.Services.AddHttpAPIClient("api-internal", (httpClient) =>
{
    httpClient.BaseAddress = new(builder.Configuration["APIs:api-internal:Url"]!);
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
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

app.UseAuthorization();

app.MapControllers();

app.UseExceptionHandler();

app.Run();
