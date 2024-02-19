using AutoMapper;
using InternalAPI.DTOs;
using InternalAPI.Models;

namespace InternalAPI.Profiles
{
    public class RecipeProfile : Profile
    {
        public RecipeProfile()
        {
            CreateMap<CreateRecipeDTO, Recipe>();
            CreateMap<UpdateRecipeDTO, Recipe>();
        }
    }
}