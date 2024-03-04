global using NUnit.Framework;
using FoodSystemAPI.Entities;
using Microsoft.EntityFrameworkCore;

public FoodDbContext GetMemoryContext()
{
    var options = new DbContextOptionsBuilder<FoodDbContext>()
        .UseInMemoryDatabase("TestDb")
        .Options;

    return new FoodDbContext(options);
}