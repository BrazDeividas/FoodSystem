using FoodSystemAPI.DTOs;
using FoodSystemAPI.Entities;

namespace FoodSystemAPI.Services;

public interface IUserService
{
    Task<UserMetrics> AddUserMetricsAsync(PostUserMetricsDto userMetrics);
    Task<UserMetrics> UpdateUserMetricsAsync(PostUserMetricsDto userMetrics);
}