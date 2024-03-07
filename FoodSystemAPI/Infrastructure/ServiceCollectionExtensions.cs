using System.Text;
using FoodSystemAPI.Entities;
using FoodSystemAPI.Handlers;
using FoodSystemAPI.Helpers;
using FoodSystemAPI.Middleware;
using FoodSystemAPI.Repositories;
using FoodSystemAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace FoodSystemAPI.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static void AddDataAccessServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<FoodDbContext>(options =>
            options.UseSqlServer(connectionString));
    }

    public static void AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IIngredientService, IngredientService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRecipeService, RecipeService>();
        services.AddScoped<IMealPlanService, MealPlanService>();
        services.AddScoped<IUserPointService, UserPointService>();

        services.AddSingleton<IUriService>(o =>
        {
            var accessor = o.GetRequiredService<IHttpContextAccessor>();
            var request = accessor.HttpContext!.Request;
            var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
            return new UriService(uri);
        });

        services.AddMemoryCache();
        services.AddScoped<ICacheService, MemoryCacheService>();
    }

    public static void AddCustomJwtAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options => 
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true
            };
        });
    }

    public static void AddExceptionHandler(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
    }

    public static void AddCustomAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    }

    public static IHttpClientBuilder AddHttpAPIClient(this IServiceCollection services, string name, Action<HttpClient> configureClient)
    {
        return services.AddHttpClient<HttpClient>(name, (HttpClient) =>
        {
            configureClient(HttpClient);
        });
    }

    public static void AddHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpAPIClient("api-1", (httpClient) =>
        {
            httpClient.BaseAddress = new Uri(configuration["APIs:api-1:Url"]!);
            httpClient.AddRapidAPIHeaders(configuration["APIs:api-1:Host"]!, configuration["APIs:api-1:Key"]!);
        });

        services.AddHttpAPIClient("api-2", (httpClient) =>
        {
            httpClient.BaseAddress = new Uri(configuration["APIs:api-2:Url"]!);
            httpClient.AddRapidAPIHeaders(configuration["APIs:api-2:Host"]!, configuration["APIs:api-2:Key"]!);
        });

        services.AddHttpAPIClient("api-internal", (httpClient) =>
        {
            httpClient.BaseAddress = new Uri(configuration["APIs:api-internal:Url"]!);
        });
    }

}