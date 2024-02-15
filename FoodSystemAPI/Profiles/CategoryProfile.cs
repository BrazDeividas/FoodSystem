namespace FoodSystemAPI.Profiles;

using AutoMapper;
using FoodSystemAPI.DTOs;
using FoodSystemAPI.Entities;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<PostCategoryDto, Category>();
    }
}