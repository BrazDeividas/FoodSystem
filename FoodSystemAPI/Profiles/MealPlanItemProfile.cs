using AutoMapper;
using FoodSystemAPI.DTOs;
using FoodSystemAPI.Entities;

namespace FoodSystemAPI.Profiles;

public class MealPlanItemProfile : Profile
{
    public MealPlanItemProfile()
    {
        CreateMap<MealPlanItem, SendClientMealPlanItemDto>();
    }
}