using System.Linq.Expressions;
using InternalAPI.Filters;

namespace InternalAPI.Repositories;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> expression);
    Task<IEnumerable<T>> GetAll();
    Task<T> GetById(int id);
    Task<T> Get(Expression<Func<T, bool>> expression);
    Task<T> Add(T entity);
    T Update(T entity);
    Task<T> Delete(int id);
    Task<int> CountAsync();
    Task<int> CountAsync(Expression<Func<T, bool>> expression);
}