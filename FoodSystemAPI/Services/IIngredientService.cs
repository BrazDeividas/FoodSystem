using System.Linq.Expressions;
using FoodSystemAPI.DTOs;
using FoodSystemAPI.Entities;
using FoodSystemAPI.Filters;

namespace FoodSystemAPI.Services;

public interface IIngredientService
{
    public Task<IEnumerable<Ingredient>> GetAll(PaginationFilter paginationFilter, Expression<Func<Ingredient, bool>>? expression = null);
    public Task<IEnumerable<Ingredient>> GetAll(Expression<Func<Ingredient, bool>> expression);
    public Task<IEnumerable<Ingredient>> MatchIngredients(IEnumerable<string> ingredients);
    public Task<Ingredient> GetById(int id);
    public Task<Ingredient> Add(PostIngredientDto ingredientDto);
    public Ingredient Update(Ingredient ingredient);
    public Task<Ingredient> Delete(int id);
    public Task<int> CountAsync(Expression<Func<Ingredient, bool>>? expression = null);
}