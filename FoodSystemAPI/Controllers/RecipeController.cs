using FoodSystemAPI.DTOs;
using FoodSystemAPI.Filters;
using FoodSystemAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodSystemAPI.Controllers;

[Authorize]
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
    public async Task<ActionResult<IEnumerable<ReceiveServerRecipeDto>>> GetRecipesAsync([FromQuery] string searchQuery, [FromQuery] PaginationFilter paginationFilter) // form paged thing-y & fix paged thing-y after change xd
    {
        var recipes = await _recipeService.GetRecipesAsync(searchQuery, paginationFilter);
        if (recipes != null)
        {
            return Ok(recipes);
        }
        else
        {
            return NotFound();
        }
    }

    [HttpGet("byIngredients")]
    public async Task<ActionResult<IEnumerable<ReceiveServerRecipeDto>>> GetRecipesByIngredientsAsync([FromQuery] string ingredients)
    {
        var recipes = await _recipeService.GetRecipesByIngredientsAsync(ingredients);
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