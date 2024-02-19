namespace InternalAPI.DbContext;

using InternalAPI.Models;
using Microsoft.EntityFrameworkCore;

public class RecipeDbContext : DbContext
{
    public RecipeDbContext(DbContextOptions<RecipeDbContext> options) : base(options)
    {
    }

    public DbSet<Recipe> Recipes { get; set; }
}