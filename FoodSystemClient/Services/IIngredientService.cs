using FoodSystemClient.Models;
using FoodSystemClient.Wrappers;

namespace FoodSystemClient.Services;

public interface IIngredientService
{
    Task<PagedResponse<IEnumerable<Ingredient>>> GetIngredientsByPage(int pageNumber, int pageSize);
    Task<PagedResponse<IEnumerable<Ingredient>>> GetIngredientsByPage(int pageNumber, int pageSize, string search);
    Task AddIngredientsToUser(IEnumerable<int> ingredientIds);
    Task RemoveIngredientsFromUser(IEnumerable<int> ingredientIds);
    Task<Response<IEnumerable<Ingredient>>> GetOwnedIngredients();
}