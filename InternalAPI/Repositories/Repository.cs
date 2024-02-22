using System.Linq.Expressions;
using InternalAPI.DbContext;
using InternalAPI.Filters;
using InternalAPI.Repositories;
using InternalAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace InternalAPI.Models;

public class Repository<T> : IRepository<T> where T : class
{
    public readonly RecipeDbContext _context;
    public readonly DbSet<T> _dbSet;

    public Repository(RecipeDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> expression)
    {
        var entries = await _dbSet.Where(expression).ToListAsync();

        return entries;
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        var entries = await _dbSet.ToListAsync();

        return entries;
    }

    public async Task<T> GetById(int id)
    {  
        var entry = await _dbSet.FindAsync(id);

        return entry;
    }

    public async Task<T> Get(Expression<Func<T, bool>> expression)
    {
        var entry = await _dbSet.FirstOrDefaultAsync(expression);

        return entry;
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
        var entity = await GetById(id);
        if(entity == null)
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
}