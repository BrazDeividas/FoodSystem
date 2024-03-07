using AutoFixture;
using AutoMapper;
using FoodSystemAPI.DTOs;
using FoodSystemAPI.Entities;
using FoodSystemAPI.Profiles;
using FoodSystemAPI.Repositories;
using FoodSystemAPI.Services;
using FoodSystemAPI.Wrappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using RichardSzalay.MockHttp;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Tests.FoodSystemAPITests;

internal class FoodSystemAPI_MealPlanServiceTests
{
    private MealPlanService _mealPlanService = null!;

    [SetUp]
    public void InitDb()
    {
        var db = GetMemoryContext();
        db.Database.EnsureDeleted();

        var rnd = new Random();
        var pass = new byte[32];
        rnd.NextBytes(pass);
        db.Users.Add(new User { FirstName = "John", LastName = "Jonathan", Username = "jonnyboi01", PasswordHash = pass});
        db.Ingredients.Add(new Ingredient {  CalciumMg = 100, CarbG = 100, EnergyKcal = 1900, FatG = 100, FiberG = 100, IronMg = 100, MagnesiumMg = 100, PotassiumMg = 100, ProteinG = 100, SodiumMg = 100, SugarG = 100, ZincMg = 100, Description = "TestIngredient" });
        db.SaveChanges();
    }

    [SetUp]
    public void InitServices()
    {
        var db = GetMemoryContext();

        var fixture = new Fixture();
        var testUri = new Uri("http://localhost/api/Recipe/byFilter");
        var expectedRecipe = fixture.Build<ReceiveServerRecipeDto>()
            .CreateMany(1)
            .ToList();
        var expectedResult = fixture.Build<Response<List<ReceiveServerRecipeDto>>>()
            .With(p => p.Data, expectedRecipe)
            .Create();


        var handler = new MockHttpMessageHandler();
        handler.When(HttpMethod.Get, testUri.ToString())
            .Respond(HttpStatusCode.OK, JsonContent.Create(expectedResult));

        var http = handler.ToHttpClient();
        http.BaseAddress = new Uri("http://localhost/");

        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        mockHttpClientFactory.Setup(p => p.CreateClient(It.IsAny<string>())).Returns(http);

        var services = new ServiceCollection();
        services.AddMemoryCache();
        var serviceProvider = services.BuildServiceProvider();

        var memoryCache = serviceProvider.GetService<IMemoryCache>();
        var memoryCacheService = new MemoryCacheService(memoryCache);

        var ingredientRepo = new Repository<Ingredient>(db);
        var recipeRepo = new Repository<Recipe>(db);
        var recipeIngredientRepo = new Repository<RecipeIngredient>(db);
        var mealPlanRepo = new Repository<MealPlan>(db);


        var ingredientMapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<IngredientProfile>()));
        var recipeMapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<RecipeProfile>()));

        var ingredientService = new IngredientService(ingredientRepo, ingredientMapper);
        var recipeService = new RecipeService(mockHttpClientFactory.Object, recipeMapper, memoryCacheService, ingredientService, recipeRepo, recipeIngredientRepo);
        _mealPlanService = new MealPlanService(mealPlanRepo, mockHttpClientFactory.Object, recipeService);
    }

    [Test]
    public async Task GetCurrentMealPlanAsync_NoMealPlanExists_NullReturned()
    {
        var mealPlan = await _mealPlanService.GetCurrentMealPlanAsync(1);

        Assert.IsNull(mealPlan);
    }

    [Test]
    public async Task GetCurrentMealPlanAsync_MealPlanExists_MealPlanReturned()
    {
        var db = GetMemoryContext();
        db.Recipes.Add(new Recipe 
        { 
            RecipeId = 1, 
            Title = "TestRecipe", 
            Instructions = "TestInstructions", 
            ImageUrl = new Uri("http://localhost/"),
            Calories = 100,
            Servings = 1
        });
        var expectedResult = new MealPlan
        {
            MealPlanId = 1,
            UserId = 1,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now + TimeSpan.FromDays(1),
            TotalCalories = 2000,
            MealPlanItems = new List<MealPlanItem> { new MealPlanItem { MealPlanItemId = 1, RecipeId = 1, MealPlanId = 1} }
        };  
        db.MealPlans.Add(expectedResult);

        db.SaveChanges();

        var mealPlan = await _mealPlanService.GetCurrentMealPlanAsync(1);

        var serializedMealPlan = JsonSerializer.Serialize(mealPlan);
        var serializedExpectedResult = JsonSerializer.Serialize(expectedResult);

        Assert.That(serializedMealPlan, Is.EqualTo(serializedExpectedResult));
    }

    [Test]
    public async Task PlanMealAsync_ArgumentsArePassed_CorrectMealPlanIsCreated()
    {
        var userMetrics = new UserMetrics
        {
            UserMetricsId = 1,
            UserId = 1,
            Sex = UserMetrics.SexType.Male,
            Age = 25,
            Height = 180,
            Weight = 80,
            ActivityLevel = UserMetrics.ActivityLevelType.ModeratelyActive
        };

        var mealPlan = await _mealPlanService.PlanMealAsync(userMetrics, 1, DateTime.Now, DateTime.Now + TimeSpan.FromDays(1));

        Assert.IsNotNull(mealPlan);
        Assert.That(mealPlan.MealPlanItems.Count(), Is.EqualTo(1));
        Assert.That(mealPlan.UserId, Is.EqualTo(1));
        Assert.That(mealPlan.TotalCalories, Is.GreaterThanOrEqualTo(_mealPlanService.CalculateCaloricNeeds(userMetrics)));
    }

    public FoodDbContext GetMemoryContext()
    {
        var options = new DbContextOptionsBuilder<FoodDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;

        return new FoodDbContext(options);
    }
}
