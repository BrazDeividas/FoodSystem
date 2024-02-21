using System.Linq.Expressions;
using System.Web;
using AutoMapper;
using InternalAPI.DTOs;
using InternalAPI.Filters;
using InternalAPI.Helpers;
using InternalAPI.Models;
using InternalAPI.Services;
using InternalAPI.Wrappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Caching.Memory;

namespace InternalAPI.Controllers;

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

    [OutputCache(VaryByQueryKeys = ["pageNumber", "pageSize", "search"])]
    [HttpGet]
    public async Task<IActionResult> GetRecipes([FromQuery] PaginationFilter paginationFilter, [FromQuery] string search = "")
    {
        var route = Request.Path.Value;
        var validFilter = new PaginationFilter(paginationFilter.PageNumber, paginationFilter.PageSize);
        IEnumerable<Recipe> entities;
        int totalRecords;
        PagedResponse<IEnumerable<Recipe>> pagedResponse;

        var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
        parameters = PaginationHelper.TrimPaginationParameters(parameters);
        var parametersEnumerable = NameValueCollectionExtensions.AsEnumerable(parameters);

        if (!string.IsNullOrEmpty(search))
        {
            Expression<Func<Recipe, bool>> filter = x => x.Title.Contains(search);
            entities = await _recipeService.GetAll(validFilter, filter);
            totalRecords = await _recipeService.CountAsync(filter);
        }
        else
        {
            entities = await _recipeService.GetAll(validFilter);
            totalRecords = await _recipeService.CountAsync();
        }
        
        pagedResponse = PaginationHelper.CreatePagedResponse(entities, validFilter, totalRecords, _uriService, route, parametersEnumerable);
        return Ok(pagedResponse);
    }

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