using AutoMapper;
using InternalAPI.DTOs;
using InternalAPI.Models;
using InternalAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace InternalAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RecipeController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RecipeController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    [HttpGet]
    public async Task<IActionResult> GetRecipes()
    {
        var recipes = await _unitOfWork.Recipes.GetAll();
        return Ok(recipes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRecipe(int id)
    {
        var recipe = await _unitOfWork.Recipes.GetById(id);
        if(recipe == null)
        {
            return NotFound();
        }
        return Ok(recipe);
    }

    [HttpPost]
    public async Task<IActionResult> AddRecipe(CreateRecipeDTO recipe)
    {
        var entity = _mapper.Map<CreateRecipeDTO, Recipe>(recipe);
        await _unitOfWork.Recipes.Add(entity);
        await _unitOfWork.CompleteAsync();
        return Ok(recipe);
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
        return Ok(recipe);
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
        return Ok(recipe);
    }
}