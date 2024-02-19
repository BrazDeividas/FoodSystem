using FoodSystemAPI.DTOs;
using FoodSystemAPI.Entities;

namespace FoodSystemAPI.Services;

public interface ICategoryService
{
    public Task<IEnumerable<Category>> GetAll();
    public Task<Category> GetById(int id);
    public Task<Category> Add(PostCategoryDto categoryDto);
    public Category Update(Category category);
    public Task<Category> Delete(int id);
}