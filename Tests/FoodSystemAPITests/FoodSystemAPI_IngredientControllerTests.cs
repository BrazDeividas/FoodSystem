using FoodSystemAPI.Entities;
using FoodSystemAPI.Repositories;
using FoodSystemAPI.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Tests.FoodSystemAPITests;

internal class FoodSystemAPI_IngredientControllerTests
{
    [SetUp]
    public void InitDb()
    {
        var db = GetMemoryContext();
        db.Database.EnsureDeleted();
    }

    [SetUp]
    public void InitServices()
    {
        var db = GetMemoryContext();
        
        var mockIngredientService = new Mock<IIngredientService>();
    }

    public FoodDbContext GetMemoryContext()
    {
        var options = new DbContextOptionsBuilder<FoodDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;

        return new FoodDbContext(options);
    }
}
