using System.Security.Claims;
using System.Text;
using System.Text.Json;
using FoodSystemAPI.DTOs;
using FoodSystemAPI.Entities;
using FoodSystemAPI.Services;
using FoodSystemAPI.Wrappers;

namespace FoodSystemAPI.Middleware;

public class CheckUserPointsMiddleware
{
    private readonly RequestDelegate _next;

    public CheckUserPointsMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context, IUserService userService, IUserPointService userPointService)
    {
        if (context.Request.Path.HasValue && string.Equals(context.Request.Path.Value, "/api/MealPlan", StringComparison.CurrentCultureIgnoreCase) && context.Request.Method == "POST")
        {
            var user = context.User;
            var username = user.FindFirst(ClaimTypes.Name);
            
            if (username != null)
            {
                var userEntity = await userService.GetUserByUsername(username.Value);
                var userPoints = await userPointService.GetUserPoints(userEntity.UserId);
                context.Request.EnableBuffering();
                
                using (StreamReader stream = new StreamReader(context.Request.Body,
                                                            encoding: Encoding.UTF8,
                                                            detectEncodingFromByteOrderMarks: false,
                                                            leaveOpen: true))
                {
                    var body = await stream.ReadToEndAsync();
                    context.Request.Body.Seek(0, SeekOrigin.Begin);
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var result = JsonSerializer.Deserialize<MealPlanOptions>(body, options);
                    var days = (int)double.Ceiling((result.EndDate - result.StartDate).TotalDays);
                    if (result == null || userPoints == null || userPoints.Points < days)
                    {
                        context.Response.StatusCode = 403;
                        await context.Response.WriteAsJsonAsync(new Response<MealPlan> { Succeeded = false, Message = "Not enough points to form desired meal plan" });
                        return;
                    }
                    await _next.Invoke(context);
                }
            }
        }
        await _next.Invoke(context);
    }
}

public static class CheckUserPointsMiddlewareExtensions
{
    public static IApplicationBuilder UseCheckUserPointsMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CheckUserPointsMiddleware>();
    }
}