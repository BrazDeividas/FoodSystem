using System.Linq.Expressions;
using InternalAPI.DTOs;
using InternalAPI.Models;
using InternalAPI.Services;
using InternalAPI.Wrappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace InternalAPI.Controllers;

//remove pagination
//move caching to service

[Route("api/[controller]")]
[ApiController]
public class RecipeController : ControllerBase
{
    private readonly IUriService _uriService;
    private readonly IRecipeService _recipeService;

    public RecipeController(IUriService uriService, IRecipeService recipeService)
    {
        _uriService = uriService;
        _recipeService = recipeService;
    }

    [OutputCache(VaryByQueryKeys = ["search"])]
    [HttpGet]
    public async Task<IActionResult> GetRecipes([FromQuery] string search = "")
    {
        IEnumerable<Recipe> entities;

        if (!string.IsNullOrEmpty(search))
        {
            Expression<Func<Recipe, bool>> filter = x => x.Title.Contains(search);
            entities = await _recipeService.GetAll(filter);
        }
        else
        {
            entities = await _recipeService.GetAll();
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