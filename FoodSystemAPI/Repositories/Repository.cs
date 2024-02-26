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