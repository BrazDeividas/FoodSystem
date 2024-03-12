using FoodSystemAPI.DTOs;
using FoodSystemAPI.Entities;

namespace FoodSystemAPI.Services;

public interface IUserService
{
    Task<UserMetrics> AddUserMetricsAsync(PostUserMetricsDto userMetrics, int userId);
    Task<UserMetrics> GetUserMetricsByUserIdAsync(int userId);
    Task<UserMetrics> UpdateUserMetricsAsync(PostUserMetricsDto userMetrics, int userId);
    Task<User> GetUserByUsername(string username);
    Task AddIngredientsToUserAsync(IEnumerable<Ingredient> ingredients, int userId);
    Task RemoveIngredientsFromUserAsync(IEnumerable<Ingredient> ingredients, int userId);
}