using AutoMapper;
using FoodSystemAPI.DTOs;
using FoodSystemAPI.Entities;

namespace FoodSystemAPI.Profiles;

public class UserMetricsProfile : Profile
{
    public UserMetricsProfile()
    {
        CreateMap<PostUserMetricsDto, UserMetrics>();
    }
}