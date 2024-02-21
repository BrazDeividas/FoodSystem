using Azure;
using FoodSystemAPI.DTOs;
using FoodSystemAPI.Entities;
using FoodSystemAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FoodSystemAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RecipeController : ControllerBase
{
    private readonly IRecipeService _recipeService;
    public RecipeController(IRecipeService recipeService)
    {
        _recipeService = recipeService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReceiveServerRecipeDto>>> GetRecipesAsync(string searchQuery)
    {
        var recipes = await _recipeService.GetRecipesAsync(searchQuery);
        if (recipes != null)
        {
            return Ok(recipes);
        }
        else
        {
            return NotFound();
        }
    }
}