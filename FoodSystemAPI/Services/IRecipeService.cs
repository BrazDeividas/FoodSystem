using System.Linq.Expressions;
using FoodSystemAPI.DTOs;
using FoodSystemAPI.Entities;
using FoodSystemAPI.Filters;

namespace FoodSystemAPI.Services;

public interface IRecipeService
{
    Task<IEnumerable<Recipe>> GetSavedRecipesAsync(Expression<Func<Recipe, bool>> expression);
    Task<IEnumerable<Recipe>> GetSavedRecipesIncludeAsync(Expression<Func<Recipe, bool>> expression, params Expression<Func<Recipe, object>>[] includeProperties);
    Task<IEnumerable<ReceiveServerRecipeDto>> GetRecipesAsync(string searchQuery, PaginationFilter paginationFilter);
    Task<IEnumerable<ReceiveServerRecipeDto>> GetRecipesByIngredientsAsync(string ingredients);
    Task<IEnumerable<Recipe>> AddRecipesForUserAsync(IEnumerable<ReceiveServerRecipeDto> recipe, int userId);
}