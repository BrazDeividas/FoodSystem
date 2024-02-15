namespace FoodSystemAPI.Profiles;

using AutoMapper;
using FoodSystemAPI.DTOs;
using FoodSystemAPI.Entities;

public class IngredientProfile : Profile
{
    public IngredientProfile()
    {
        CreateMap<PostIngredientDto, Ingredient>();
    }
}