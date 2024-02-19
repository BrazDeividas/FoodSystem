namespace InternalAPI.DbContext;

using System.Text.Json;
using System.Text.Json.Serialization;
using InternalAPI.Models;
using Microsoft.EntityFrameworkCore;

public class RecipeDbContext : DbContext
{
    public RecipeDbContext(DbContextOptions<RecipeDbContext> options) : base(options)
    {
    }

    public DbSet<Recipe> Recipes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Recipe>()
        .Property(r => r.Ingredients)
        .HasConversion(
            v => JsonSerializer.Serialize(v, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }),
            v => JsonSerializer.Deserialize<List<string>>(v, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
        );
    }
}