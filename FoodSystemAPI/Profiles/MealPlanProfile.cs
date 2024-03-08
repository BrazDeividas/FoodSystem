using AutoMapper;
using FoodSystemAPI.DTOs;
using FoodSystemAPI.Entities;

namespace FoodSystemAPI.Profiles;

public class MealPlanProfile : Profile
{
    public MealPlanProfile()
    {
        CreateMap<MealPlan, SendClientMealPlanDto>();
    }
}