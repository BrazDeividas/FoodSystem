using System.Linq.Expressions;

namespace InternalAPI.Repositories;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAll();
    Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> expression);
    Task<T> GetById(int id);
    Task<T> Get(Expression<Func<T, bool>> expression);
    Task<T> Add(T entity);
    T Update(T entity);
    Task<T> Delete(int id);
    Task<int> CountAsync();
    Task<int> CountAsync(Expression<Func<T, bool>> expression);
}