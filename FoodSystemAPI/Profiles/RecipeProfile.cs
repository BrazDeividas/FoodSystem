using FoodSystemAPI.DTOs;
using FoodSystemAPI.Entities;
using AutoMapper;
using FoodSystemAPI.DTOs.Tasty;

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

        CreateMap<Result, SendServerRecipeDto>()
            .ForMember(dest => dest.SourceId, opt => opt.MapFrom(src => src.id))
            .ForMember(dest => dest.SourceAPI, opt => opt.MapFrom(src => "tasty.p.rapidapi.com"))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.name))
            .ForMember(dest => dest.Instructions, opt => opt.MapFrom(src => string.Join("\n", src.instructions.Select(x => x.display_text).ToList())))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.thumbnail_url))
            .ForMember(dest => dest.Calories, opt => opt.MapFrom(src => src.nutrition.calories ?? 0))
            .ForMember(dest => dest.Servings, opt => opt.MapFrom(src => src.num_servings))
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.sections.SelectMany(x => x.components).Select(x => x.raw_text).ToList()));

        CreateMap<ReceiveServerRecipeDto, Recipe>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Instructions, opt => opt.MapFrom(src => src.Instructions))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
            .ForMember(dest => dest.Calories, opt => opt.MapFrom(src => src.Calories))
            .ForMember(dest => dest.Servings, opt => opt.MapFrom(src => src.Servings))
            .ForMember(dest => dest.Ingredients, opt => opt.Ignore())
            .ForMember(dest => dest.RecipeId, opt => opt.Ignore());

        CreateMap<Recipe, SendClientRecipeDto>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Instructions, opt => opt.MapFrom(src => src.Instructions))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
            .ForMember(dest => dest.Calories, opt => opt.MapFrom(src => src.Calories))
            .ForMember(dest => dest.Servings, opt => opt.MapFrom(src => src.Servings))
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients.Select(x => x.Description).ToList()))
            .ForMember(dest => dest.IngredientIds, opt => opt.MapFrom(src => src.Ingredients.Select(x => x.IngredientId).ToList()));
    }
}