using FoodSystemAPI.Filters;

namespace FoodSystemAPI.Repositories;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAll();
    Task<IEnumerable<T>> GetAll(PaginationFilter paginationFilter);
    Task<T> GetById(int id);
    Task<T> Add(T entity);
    T Update(T entity);
    Task<T> Delete(int id);
    Task<int> CountAsync();
    void Save();
}