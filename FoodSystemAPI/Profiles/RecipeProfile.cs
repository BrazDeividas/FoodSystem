using FoodSystemAPI.DTOs;
using FoodSystemAPI.Entities;
using AutoMapper;

namespace FoodSystemAPI.Profiles;

public class RecipeProfile : Profile
{
    public RecipeProfile()
    {
        CreateMap<RapidAPIRecipeDto_1, SendServerRecipeDto>()
            .ForMember(dest => dest.SourceId, opt => opt.MapFrom(src => src.id))
            .ForMember(dest => dest.SourceAPI, opt => opt.MapFrom(src => "food-recipes-with-images.p.rapidapi.com"))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Instructions, opt => opt.MapFrom(src => src.Instructions))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Image))
            .ForMember(dest => dest.Calories, opt => opt.MapFrom(src => 0))
            .ForMember(dest => dest.Servings, opt => opt.MapFrom(src => 0))
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients.Select(i => i.Value).ToList()));
    }
}