using AutoMapper;
using FoodSystemAPI.DTOs;
using FoodSystemAPI.Entities;
using FoodSystemAPI.Repositories;

namespace FoodSystemAPI.Services;

public class CategoryService : ICategoryService
{
    private readonly IRepository<Category> _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryService(IRepository<Category> categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Category>> GetAll()
    {
        return await _categoryRepository.GetAll();
    }

    public async Task<Category> GetById(int id)
    {
        return await _categoryRepository.GetById(id);
    }

    public async Task<Category> Add(PostCategoryDto categoryDto)
    {
        var category = _mapper.Map<PostCategoryDto, Category>(categoryDto);
        await _categoryRepository.Add(category);
        _categoryRepository.Save();
        return category;
    }

    public Category Update(Category category)
    {
        _categoryRepository.Update(category);
        _categoryRepository.Save();
        return category;
    }

    public async Task<Category> Delete(int id)
    {
        var category = await _categoryRepository.GetById(id);
        if (category == null)
            return null;
        await _categoryRepository.Delete(id);
        _categoryRepository.Save();
        return category;
    }
}