using System.Linq.Expressions;
using FoodSystemAPI.Entities;
using FoodSystemAPI.Filters;
using Microsoft.EntityFrameworkCore;

namespace FoodSystemAPI.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly FoodDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(FoodDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAll(PaginationFilter paginationFilter)
    {
        return await _dbSet.Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
            .Take(paginationFilter.PageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAll(PaginationFilter paginationFilter, Expression<Func<T, bool>> expression)
    {
        return await _dbSet.Where(expression)
            .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
            .Take(paginationFilter.PageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> expression)
    {
        return await _dbSet.Where(expression).ToListAsync();
    }

    public IQueryable<T> GetAllQueryable(Expression<Func<T, bool>> expression)
    {
        return _dbSet.Where(expression);
    }

    public async Task<IEnumerable<T>> GetAllInclude(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes)
    {
        var query = _dbSet.Where(expression);
        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        return await query.ToListAsync();
    }

    public IQueryable<T> IncludeMultipleQueryable(IQueryable<T> query, params Expression<Func<T, object>>[] includes)
    {
        if(includes != null)
        {
            query = includes.Aggregate(query, (current, include) => current.Include(include));
        }
        return query;
    }

    public async Task<T> GetById(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T> Add(T entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public T Update(T entity)
    {
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        return entity;
    }

    public async Task<T> Delete(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity == null)
        {
            return entity;
        }
        _dbSet.Remove(entity);
        return entity;
    }

    public async Task<int> CountAsync()
    {
        return await _dbSet.CountAsync();
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>> expression)
    {
        return await _dbSet.CountAsync(expression);
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}