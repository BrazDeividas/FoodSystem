using System.Security.Claims;
using FoodSystemClient.Models;

namespace FoodSystemClient.Services;

public interface IUserService
{
    Task<ClaimsPrincipal> LoginAsync(string username);
    Task<UserMetrics> GetUserMetrics();
    Task<bool> PostUserMetrics(UserMetrics userMetrics);
}