using FoodSystemAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FoodSystemAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MealPlanController : ControllerBase
{
    private readonly IMealPlanService _mealPlanService;

    public MealPlanController(IMealPlanService mealPlanService)
    {
        _mealPlanService = mealPlanService;
    }
}