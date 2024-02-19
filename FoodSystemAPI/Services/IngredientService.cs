using System.Linq.Expressions;
using AutoMapper;
using FoodSystemAPI.DTOs;
using FoodSystemAPI.Entities;
using FoodSystemAPI.Filters;
using FoodSystemAPI.Repositories;

namespace FoodSystemAPI.Services;

public class IngredientService : IIngredientService
{
    private readonly IRepository<Ingredient> _ingredientRepository;
    private readonly IMapper _mapper;

    public IngredientService(IRepository<Ingredient> ingredientRepository, IMapper mapper)
    {
        _ingredientRepository = ingredientRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Ingredient>> GetAll(PaginationFilter paginationFilter, Expression<Func<Ingredient, bool>>? expression = null)
    {
        if (expression != null)
            return await _ingredientRepository.GetAll(paginationFilter, expression);
        return await _ingredientRepository.GetAll(paginationFilter);
    }

    public async Task<Ingredient> GetById(int id)
    {
        return await _ingredientRepository.GetById(id);
    }

    public async Task<Ingredient> Add(PostIngredientDto ingredientDto)
    {
        var ingredient = _mapper.Map<PostIngredientDto, Ingredient>(ingredientDto);
        await _ingredientRepository.Add(ingredient);
        _ingredientRepository.Save();
        return ingredient;
    }

    public Ingredient Update(Ingredient ingredient)
    {
        _ingredientRepository.Update(ingredient);
        _ingredientRepository.Save();
        return ingredient;
    }

    public async Task<Ingredient> Delete(int id)
    {
        var ingredient = await _ingredientRepository.GetById(id);
        if (ingredient == null)
            return null;
        await _ingredientRepository.Delete(id);
        _ingredientRepository.Save();
        return ingredient;
    }

    public async Task<int> CountAsync(Expression<Func<Ingredient, bool>>? expression = null)
    {
        if (expression != null)
            return await _ingredientRepository.CountAsync(expression);
        return await _ingredientRepository.CountAsync();
    }
}