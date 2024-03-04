using System.Linq.Expressions;
using InternalAPI.DTOs;
using InternalAPI.Filters;
using InternalAPI.Models;
using InternalAPI.Services;
using InternalAPI.Wrappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace InternalAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RecipeController : ControllerBase
{
    private readonly IRecipeService _recipeService;

    public RecipeController(IRecipeService recipeService)
    {
        _recipeService = recipeService;
    }

    [OutputCache(VaryByQueryKeys = ["search"])]
    [HttpGet]
    public async Task<IActionResult> GetRecipes([FromQuery] string search)
    {
        var entities = !string.IsNullOrEmpty(search)
        ? await _recipeService.GetAll(x => x.Title.Contains(search))
        : await _recipeService.GetAll();

        if (entities == null)
        {
            return NotFound();
        }
        return Ok(new Response<IEnumerable<Recipe>>(entities));
    }

    [OutputCache(VaryByQueryKeys = ["search", "caloriesum", "numberofmeals", "days"])]
    [HttpGet("byFilter")]
    public async Task<IActionResult> GetRecipesByFilter([FromQuery] SearchFilter searchFilter)
    {
        IEnumerable<Recipe> entities;

        entities = await _recipeService.GetAll(searchFilter);

        if (entities == null)
        {
            return NotFound();
        }

        return Ok(new Response<IEnumerable<Recipe>>(entities));
    }

    [OutputCache(VaryByQueryKeys = ["ingredients"])]
    [HttpGet("byIngredients")]
    public async Task<IActionResult> GetByIngredients(string ingredients)
    {
        var recipes = await _recipeService.GetAllByIngredients(ingredients);
        return Ok(new Response<IEnumerable<Recipe>>(recipes));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRecipe(int id)
    {
        var recipe = await _recipeService.GetById(id);
        if(recipe == null)
        {
            return NotFound();
        }
        return Ok(new Response<Recipe>(recipe));
    }

    [HttpPost]
    public async Task<IActionResult> AddRecipe(IEnumerable<CreateRecipeDTO> recipes) //fix with add range later
    {
        var response = await _recipeService.AddMany(recipes);
        return Ok(new Response<IEnumerable<Recipe>>(response));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateRecipe(Recipe recipe)
    {
       
        return Ok(await _recipeService.UpdateAsync(recipe));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRecipe(UpdateRecipeDTO recipe, int id)
    {
        var response = await _recipeService.UpdateById(id, recipe);
        if(response == null)
        {
            return NotFound();
        }
        return Ok(new Response<Recipe>(response));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRecipe(int id)
    {
        var recipe = await _recipeService.Delete(id);
        if(recipe == null)
        {
            return NotFound();
        }

        return Ok(new Response<Recipe>(recipe));
    }
}