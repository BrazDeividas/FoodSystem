using AutoMapper;
using FoodSystemAPI.Entities;
using FoodSystemAPI.Repositories;
using FoodSystemAPI.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.FoodSystemAPITests;

internal class FoodSystemAPI_MealPlanServiceTests
{
    [SetUp]
    public void InitDb()
    {
        var db = GetMemoryContext();
        db.Database.EnsureDeleted();

        var rnd = new Random();
        var pass = new byte[32];
        rnd.NextBytes(pass);
        db.Users.Add(new User { FirstName = "John", LastName = "Jonathan", Username = "jonnyboi01", PasswordHash = pass});
        db.Ingredients.Add(new Ingredient {  CalciumMg = 100, CarbG = 100, EnergyKcal = 100, FatG = 100, FiberG = 100, IronMg = 100, MagnesiumMg = 100, PotassiumMg = 100, ProteinG = 100, SodiumMg = 100, SugarG = 100, ZincMg = 100, Description = "TestIngredient" });
        db.SaveChanges();
    }

    [Test]
    public async Task AddMealPlanAsync_UserDoesNotExist_NullReturned()
    {
        var db = GetMemoryContext();
        var mealPlanRepo = new Repository<MealPlan>(db);
        var userRepo = new Repository<User>(db);
        var httpClient = new HttpClient();
        var mealPlanService = new MealPlanService(mealPlanRepo, httpClient,);

        Assert.IsNull(await mealPlanService.AddMealPlanAsync(new PostMealPlanDto
        {
            Name = "TestMealPlan",
            Description = "TestDescription",
            MealPlanDays = new List<PostMealPlanDayDto>
            {
                new PostMealPlanDayDto
                {
                    Day = 1,
                    Meals = new List<PostMealPlanMealDto>
                    {
                        new PostMealPlanMealDto
                        {
                            Name = "TestMeal",
                            Description = "TestDescription",
                            MealPlanIngredients = new List<PostMealPlanIngredientDto>
                            {
                                new PostMealPlanIngredientDto
                                {
                                    IngredientId = 1,
                                    Amount = 100
                                }
                            }
                        }
                    }
                }
            }
        }, 37));
    }

    public FoodDbContext GetMemoryContext()
    {
        var options = new DbContextOptionsBuilder<FoodDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;

        return new FoodDbContext(options);
    }
}
