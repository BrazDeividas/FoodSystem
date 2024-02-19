using FoodSystemAPI.Entities;
using FoodSystemAPI.Filters;

namespace FoodSystemAPI.Services;

public interface IIngredientService
{
    public Task<IEnumerable<Ingredient>> GetAll(PaginationFilter paginationFilter, int categoryId = 0);
}