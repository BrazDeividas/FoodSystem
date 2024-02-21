using System.Linq.Expressions;
using AutoMapper;
using InternalAPI.DTOs;
using InternalAPI.Filters;
using InternalAPI.Models;

namespace InternalAPI.Services;

public class RecipeService : IRecipeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public RecipeService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Recipe>> AddMany(IEnumerable<CreateRecipeDTO> recipes)
    {
        List<Recipe> entities = new List<Recipe>();
        foreach (var recipe in recipes)
        {
            var entity = _mapper.Map<CreateRecipeDTO, Recipe>(recipe);
            Expression<Func<Recipe, bool>> expression = x => x.SourceAPI == entity.SourceAPI && x.SourceId == entity.SourceId;
            if ((await _unitOfWork.Recipes.Get(expression)) != null)
            {
                continue;
            }
            await _unitOfWork.Recipes.Add(entity);
            await _unitOfWork.CompleteAsync();
            entities.Add(entity);
        }
        
        return entities;
    }

    public Task<int> CountAsync(Expression<Func<Recipe, bool>> expression)
    {
        return _unitOfWork.Recipes.CountAsync(expression);
    }

    public Task<int> CountAsync()
    {
        return _unitOfWork.Recipes.CountAsync();
    }

    public async Task<Recipe> Delete(int id)
    {
        var recipe = await _unitOfWork.Recipes.Delete(id);
        if (recipe == null)
        {
            return null;
        }

        await _unitOfWork.CompleteAsync();
        return recipe;
    }

    public Task<IEnumerable<Recipe>> GetAll(PaginationFilter paginationFilter, Expression<Func<Recipe, bool>> expression)
    {
        return _unitOfWork.Recipes.GetAll(paginationFilter, expression);
    }

    public Task<IEnumerable<Recipe>> GetAll(PaginationFilter paginationFilter)
    {
        return _unitOfWork.Recipes.GetAll(paginationFilter);
    }

    public Task<IEnumerable<Recipe>> GetAll(Expression<Func<Recipe, bool>> expression)
    {
        return _unitOfWork.Recipes.GetAll(expression);
    }

    public async Task<IEnumerable<Recipe>> GetAllByIngredients(string ingredients)
    {
        var recipes = await _unitOfWork.Recipes.GetAll();
        string[] ingredientsArray = [.. ingredients.Split(',')];
        Expression<Func<Recipe, bool>> suggestions = x => x.Ingredients.Select(x => x.ToLower()).Intersect(ingredientsArray).Any();
        return recipes.Where(suggestions.Compile());
    }

    public Task<Recipe> GetById(int id)
    {
        return _unitOfWork.Recipes.GetById(id);
    }

    public async Task<Recipe> UpdateAsync(Recipe entity)
    {
        var updatedEntity = _unitOfWork.Recipes.Update(entity);
        await _unitOfWork.CompleteAsync();
        return updatedEntity;
    }

    public async Task<Recipe> UpdateById(int id, UpdateRecipeDTO recipe)
    {
        var updatedEntity = await _unitOfWork.Recipes.GetById(id);
        if (updatedEntity == null)
        {
            return null;
        }
        _mapper.Map(recipe, updatedEntity);
        _unitOfWork.Recipes.Update(updatedEntity);
        await _unitOfWork.CompleteAsync();
        return updatedEntity;
    }
}