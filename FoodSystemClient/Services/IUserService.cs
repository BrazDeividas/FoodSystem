using System.Security.Claims;
using FoodSystemClient.Models;

namespace FoodSystemClient.Services;

public interface IUserService
{
    Task<ClaimsPrincipal> LoginAsync(string username);
    Task<UserMetrics> GetUserMetrics(int userId);
    Task<int> PostUserMetrics(UserMetrics userMetrics, int userId);
}