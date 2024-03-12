using FoodSystemClient.Models;
using FoodSystemClient.Wrappers;

namespace FoodSystemClient.Services;

public interface ICategoryService
{
    Task<Response<IEnumerable<Category>>> GetCategories();
}