using System.Linq.Expressions;
using AutoMapper;
using InternalAPI.DTOs;
using InternalAPI.Filters;
using InternalAPI.Models;
using Microsoft.Extensions.Caching.Memory;

namespace InternalAPI.Services;

public class RecipeService : IRecipeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;

    public RecipeService(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _cacheService = cacheService;
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
            entity = await _unitOfWork.Recipes.Add(entity);
            await _unitOfWork.CompleteAsync();
            entities.Add(entity);
            
            _cacheService.Set($"{typeof(Recipe)}-{entity.RecipeId}", entity, new MemoryCacheEntryOptions {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30),
            });
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
        _cacheService.Remove($"{typeof(Recipe)}-{id}");
        return recipe;
    }
    
    public async Task<IEnumerable<Recipe>> GetAll(Expression<Func<Recipe, bool>> expression)
    {
        var entities = await _unitOfWork.Recipes.GetAll(expression);
        return entities;
    }

    public async Task<IEnumerable<Recipe>> GetAll(SearchFilter searchFilter)
    {
        Expression<Func<Recipe, bool>> filter = !string.IsNullOrEmpty(searchFilter.Search)
        ? searchFilter.CalorieSum != 0
        ? x => x.Title.Contains(searchFilter.Search) && Math.Abs(x.Calories - (searchFilter.CalorieSum / 3)) <= 150
        : x => x.Title.Contains(searchFilter.Search)
        : x => int.Abs(x.Calories - (searchFilter.CalorieSum / searchFilter.NumberOfMeals)) <= 150;


        
        Random rng = new Random();
        var entities = await GetAll(filter);
        var shuffledEntities = entities.OrderBy(x => rng.Next()).ToList();
        return shuffledEntities.Take(searchFilter.NumberOfMeals * searchFilter.Days);
    }

    public async Task<IEnumerable<Recipe>> GetAll()
    {
        var cachedEntities = await _cacheService.Get<IEnumerable<Recipe>>($"{typeof(Recipe)}-all");

        if(cachedEntities != null)
        {
            return cachedEntities;
        }

        var entities = await _unitOfWork.Recipes.GetAll();
        _cacheService.Set($"{typeof(Recipe)}-all", entities, new MemoryCacheEntryOptions {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30),
        });

        return entities;
    }

    public async Task<IEnumerable<Recipe>> GetAllByIngredients(string ingredients)
    {
        var cachedEntities = await _cacheService.Get<IEnumerable<Recipe>>($"{typeof(Recipe)}-all-by-ingredients-{ingredients}");
        
        if(cachedEntities != null)
        {
            return cachedEntities;
        }
        
        string[] ingredientsArray = [.. ingredients.Split(',')];
        var recipes = await _unitOfWork.Recipes.GetAll();
        
        var entities = (from recipe in recipes
            from ingredient in recipe.Ingredients
            where ingredientsArray.Any(ingredient.Contains)
            select recipe).Distinct().ToList();

        _cacheService.Set($"{typeof(Recipe)}-all-by-ingredients-{ingredients}", entities, new MemoryCacheEntryOptions {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30),
        });

        return entities;
    }

    public async Task<Recipe> GetById(int id)
    {
        var cachedEntity = await _cacheService.Get<Recipe>($"{typeof(Recipe)}-{id}");

        if(cachedEntity != null)
        {
            return cachedEntity;
        }

        var entity = await _unitOfWork.Recipes.GetById(id);

        _cacheService.Set($"{typeof(Recipe)}-{id}", entity, new MemoryCacheEntryOptions {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30),
        });

        return entity;
    }

    public async Task<Recipe> UpdateAsync(Recipe entity)
    {
        var updatedEntity = _unitOfWork.Recipes.Update(entity);
        await _unitOfWork.CompleteAsync();

        _cacheService.Set($"{typeof(Recipe)}-{entity.RecipeId}", updatedEntity, new MemoryCacheEntryOptions {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30),
        });

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
        
        _cacheService.Set($"{typeof(Recipe)}-{id}", updatedEntity, new MemoryCacheEntryOptions {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30),
        });

        return updatedEntity;
    }
}