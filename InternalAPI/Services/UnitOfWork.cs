using InternalAPI.DbContext;
using InternalAPI.Models;
using InternalAPI.Repositories;

namespace InternalAPI.Services;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly RecipeDbContext _context;
    public IRepository<Recipe> Recipes { get; }

    public UnitOfWork(RecipeDbContext context, ICacheService<object> cacheService)
    {
        _context = context;
        Recipes = new Repository<Recipe>(_context, cacheService);
    }

    public async Task CompleteAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}