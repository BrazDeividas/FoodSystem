using FoodSystemAPI.Entities;
using FoodSystemAPI.Services;
using FoodSystemAPI.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace FoodSystemAPI.Controllers;

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
    public async Task<IActionResult> PlanMealAsync([FromQuery] int userId, [FromQuery] int numberOfMeals)
    {
        var userMetrics = await _userService.GetUserMetricsByUserIdAsync(userId);  

        if(userMetrics == null)
        {
            return NotFound("User metrics are not submitted");
        }

        var mealPlan = await _mealPlanService.PlanMealAsync(userMetrics, numberOfMeals);
        if (mealPlan == null)
        {
            return NotFound("Valid recipes not found for the meal plan");
        }

        return Ok(new Response<MealPlan>(mealPlan));
    }
}