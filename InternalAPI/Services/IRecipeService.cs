using System.Linq.Expressions;
using InternalAPI.DTOs;
using InternalAPI.Filters;
using InternalAPI.Models;

namespace InternalAPI.Services;

public interface IRecipeService
{
    Task<IEnumerable<Recipe>> GetAll(Expression<Func<Recipe, bool>> expression);
    Task<IEnumerable<Recipe>> GetAll(SearchFilter searchFilter);
    Task<IEnumerable<Recipe>> GetAll();
    Task<int> CountAsync(Expression <Func<Recipe, bool>> expression);
    Task<int> CountAsync();
    Task<Recipe> GetById(int id);
    Task<IEnumerable<Recipe>> AddMany(IEnumerable<CreateRecipeDTO> entities);
    Task<Recipe> UpdateAsync(Recipe entity);
    Task<Recipe> UpdateById(int id, UpdateRecipeDTO entity);
    Task<Recipe> Delete(int id);
    Task<IEnumerable<Recipe>> GetAllByIngredients(string ingredients);
}