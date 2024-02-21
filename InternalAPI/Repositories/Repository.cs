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
    public readonly ICacheService<object> _memoryCache;
    public readonly DbSet<T> _dbSet;

    public Repository(RecipeDbContext context, ICacheService<object> memoryCache)
    {
        _context = context;
        _memoryCache = memoryCache;
        _dbSet = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAll(PaginationFilter paginationFilter)
    {
        string key = $"{_dbSet.GetType().Name}-{paginationFilter.PageNumber}-{paginationFilter.PageSize}"; 
        var cacheEntry = await _memoryCache.Get(key);
        if(cacheEntry != null)
        {
            return (IEnumerable<T>)cacheEntry; //cia good practice vps?
        }

        var entries = await _dbSet.Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
            .Take(paginationFilter.PageSize)
            .ToListAsync();

        _memoryCache.Set(key, entries);
        return entries;
    }

    public async Task<IEnumerable<T>> GetAll(PaginationFilter paginationFilter, Expression<Func<T, bool>> expression)
    {
        string expBody = expression.Body.ToString();
        string key = $"{_dbSet.GetType().Name}-{expBody}-{paginationFilter.PageNumber}-{paginationFilter.PageSize}"; 
        var cacheEntry = await _memoryCache.Get(key);
        if(cacheEntry != null)
        {
            return (IEnumerable<T>)cacheEntry; //cia good practice vps?
        }
        
        var entries = await _dbSet.Where(expression)
            .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
            .Take(paginationFilter.PageSize)
            .ToListAsync();
        
        _memoryCache.Set(key, entries);
        return entries;
    }

    public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> expression)
    {
        string expBody = expression.Body.ToString();
        string key = $"{_dbSet.GetType().Name}-{expBody}";
        var cacheEntry = await _memoryCache.Get(key);
        if(cacheEntry != null)
        {
            return (IEnumerable<T>)cacheEntry; //cia good practice vps?
        }

        var entries = await _dbSet.Where(expression).ToListAsync();

        _memoryCache.Set(key, entries);
        return entries;
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        string key = $"{_dbSet.GetType().Name}";
        var cacheEntry = await _memoryCache.Get(key);
        if(cacheEntry != null)
        {
            return (IEnumerable<T>)cacheEntry; //cia good practice vps?
        }

        var entries = await _dbSet.ToListAsync();

        _memoryCache.Set(key, entries);
        return entries;
    }

    public async Task<T> GetById(int id)
    {
        string key = $"{_dbSet.GetType().Name}-{id}";
        var cacheEntry = await _memoryCache.Get(key);
        if(cacheEntry != null)
        {
            return (T)cacheEntry; //cia good practice vps?
        }
        
        var entry = await _dbSet.FindAsync(id);

        _memoryCache.Set(key, entry);
        return entry;
    }

    public async Task<T> Get(Expression<Func<T, bool>> expression)
    {
        string expBody = expression.Body.ToString();
        string key = $"One_{_dbSet.GetType().Name}-{expBody}";
        var cacheEntry = await _memoryCache.Get(key);
        if(cacheEntry != null)
        {
            return (T)cacheEntry; //cia good practice vps?
        }

        var entry = await _dbSet.FirstOrDefaultAsync(expression);

        _memoryCache.Set(key, entry);
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