using System.Linq.Expressions;
using FoodSystemAPI.Filters;

namespace FoodSystemAPI.Repositories;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAll();
    Task<IEnumerable<T>> GetAll(PaginationFilter paginationFilter);
    Task<IEnumerable<T>> GetAll(PaginationFilter paginationFilter, Expression<Func<T, bool>> expression);
    Task<T> GetById(int id);
    Task<T> Add(T entity);
    T Update(T entity);
    Task<T> Delete(int id);
    Task<int> CountAsync();
    Task<int> CountAsync(Expression<Func<T, bool>> expression);
    void Save();
}