using FoodSystemAPI.Entities;
using FoodSystemAPI.Handlers;
using FoodSystemAPI.Helpers;
using FoodSystemAPI.Repositories;
using FoodSystemAPI.Services;
using Microsoft.EntityFrameworkCore;

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
}