using FoodSystemAPI.Entities;

namespace FoodSystemAPI.Services;

public interface IUserPointService
{
    Task<UserPoints?> GetUserPoints(int userId);
    Task<UserPoints?> AddUserPoints(int userId, int points);
    Task<UserPoints?> SubtractUserPoints(int userId, int points);
}