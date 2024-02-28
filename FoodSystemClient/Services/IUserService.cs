using FoodSystemClient.Models;

namespace FoodSystemClient.Services;

public interface IUserService
{
    Task<UserMetrics> GetUserMetrics(int userId);
    Task<int> PostUserMetrics(UserMetrics userMetrics, int userId);
}