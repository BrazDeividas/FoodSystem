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

namespace InternalAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RecipeController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUriService _uriService;

    public RecipeController(IUnitOfWork unitOfWork, IMapper mapper, IUriService uriService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _uriService = uriService;
    }


    [HttpGet]
    public async Task<IActionResult> GetRecipes([FromQuery] PaginationFilter paginationFilter, [FromQuery] string search = "")
    {
        var route = Request.Path.Value;
        var validFilter = new PaginationFilter(paginationFilter.PageNumber, paginationFilter.PageSize);
        IEnumerable<Recipe> entities;
        int totalRecords;
        PagedResponse<IEnumerable<Recipe>> pagedResponse;

        Expression<Func<Recipe, bool>> filter = x => x.Title.Contains(search);
        if (!string.IsNullOrEmpty(search))
        {
            entities = await _unitOfWork.Recipes.GetAll(validFilter, filter);
            totalRecords = await _unitOfWork.Recipes.CountAsync(filter);
        }
        else
        {
            entities = await _unitOfWork.Recipes.GetAll(validFilter);
            totalRecords = await _unitOfWork.Recipes.CountAsync();
        }

        var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
        parameters = PaginationHelper.TrimPaginationParameters(parameters);
        var parametersEnumerable = NameValueCollectionExtensions.AsEnumerable(parameters);
        pagedResponse = PaginationHelper.CreatePagedResponse(entities, validFilter, totalRecords, _uriService, route, parametersEnumerable);
        return Ok(pagedResponse);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRecipe(int id)
    {
        var recipe = await _unitOfWork.Recipes.GetById(id);
        if(recipe == null)
        {
            return NotFound();
        }
        return Ok(new Response<Recipe>(recipe));
    }

    [HttpPost]
    public async Task<IActionResult> AddRecipe(CreateRecipeDTO recipe)
    {
        var entity = _mapper.Map<CreateRecipeDTO, Recipe>(recipe);
        await _unitOfWork.Recipes.Add(entity);
        await _unitOfWork.CompleteAsync();
        return Ok(new Response<Recipe>(entity));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateRecipe(Recipe recipe)
    {
        _unitOfWork.Recipes.Update(recipe);
        await _unitOfWork.CompleteAsync();
        return Ok(recipe);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRecipe(UpdateRecipeDTO recipe, int id)
    {
        var entity = await _unitOfWork.Recipes.GetById(id);
        if(entity == null)
        {
            return NotFound();
        }
        _mapper.Map(recipe, entity);
        _unitOfWork.Recipes.Update(entity);
        await _unitOfWork.CompleteAsync();
        return Ok(new Response<Recipe>(entity));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRecipe(int id)
    {
        var recipe = await _unitOfWork.Recipes.Delete(id);
        if(recipe == null)
        {
            return NotFound();
        }
        await _unitOfWork.CompleteAsync();
        return Ok(new Response<Recipe>(recipe));
    }
}