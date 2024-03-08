using System.Security.Claims;
using FoodSystemAPI.DTOs;
using FoodSystemAPI.Entities;
using FoodSystemAPI.Services;
using FoodSystemAPI.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodSystemAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MealPlanController : ControllerBase
{
    private readonly IMealPlanService _mealPlanService;

    private readonly IUserService _userService;

    public MealPlanController(IMealPlanService mealPlanService, IUserService userService)
    {
        _mealPlanService = mealPlanService;
        _userService = userService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetCurrentMealPlanAsync()
    {
        ClaimsPrincipal claimsPrincipal = HttpContext.User;
        var username = claimsPrincipal.FindFirst(ClaimTypes.Name);
        
        if(username == null)
        {
            return NotFound();
        }

        var user = await _userService.GetUserByUsername(username.Value);

        var mealPlan = await _mealPlanService.GetCurrentMealPlanAsync(user.UserId);
        if (mealPlan == null)
        {
            return Ok(new Response<MealPlan>{ Succeeded = false, Message = "No active meal plan found" });
        }

        return Ok(new Response<SendClientMealPlanDto>(mealPlan));
    }

    [HttpPost]
    public async Task<IActionResult> PlanMealAsync([FromBody] MealPlanOptions mealPlanOptions)
    {
        ClaimsPrincipal claimsPrincipal = HttpContext.User;
        var username = claimsPrincipal.FindFirst(ClaimTypes.Name);
        
        if(username == null)
        {
            return NotFound();
        }

        var user = await _userService.GetUserByUsername(username.Value);

        
        var userMetrics = await _userService.GetUserMetricsByUserIdAsync(user.UserId);  

        if(userMetrics == null)
        {
            return Ok(new Response<MealPlan>{ Succeeded = false, Message = "User metrics not found" });
        }

        var mealPlan = await _mealPlanService.PlanMealAsync(userMetrics, mealPlanOptions.NumberOfMeals, mealPlanOptions.StartDate, mealPlanOptions.EndDate);
        if (mealPlan == null)
        {
            return Ok(new Response<MealPlan>{ Succeeded = false, Message = "Meal plan cannot be formed" });
        }

        return Ok(new Response<MealPlan>(mealPlan));
    }
}