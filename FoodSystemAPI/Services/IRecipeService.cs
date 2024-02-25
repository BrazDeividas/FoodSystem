using FoodSystemAPI.DTOs;
using FoodSystemAPI.Entities;
using FoodSystemAPI.Filters;

namespace FoodSystemAPI.Services;

public interface IRecipeService
{
    Task<IEnumerable<ReceiveServerRecipeDto>> GetRecipesAsync(string searchQuery, PaginationFilter paginationFilter);

    Task<IEnumerable<ReceiveServerRecipeDto>> GetRecipesByIngredientsAsync(string ingredients);
}