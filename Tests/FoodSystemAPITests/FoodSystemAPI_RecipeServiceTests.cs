
using AutoFixture;
using AutoMapper;
using FoodSystemAPI.DTOs;
using FoodSystemAPI.Entities;
using FoodSystemAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using RichardSzalay.MockHttp;
using System.Net.Http.Json;
using System.Net;
using FoodSystemAPI.Repositories;
using FoodSystemAPI.Profiles;
using FoodSystemAPI.Wrappers;
using FoodSystemAPI.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.FoodSystemAPITests;

internal class FoodSystemAPI_RecipeServiceTests
{
    private RecipeService _recipeService = null!;

    [SetUp]
    public void InitDb()
    {
        var db = GetMemoryContext();
        db.Database.EnsureDeleted();

        db.Users.Add(new User
        {
            FirstName = "John",
            LastName = "Jonathan",
            Username = "jonnyboi01",
            PasswordHash = new byte[32]
        });

        db.Recipes.Add(new Recipe
        {
            RecipeId = 1,
            Title = "Chicken Pasta",
            Instructions = "Order online lmao",
            ImageUrl = new Uri("http://localhost/"),
            Calories = 100,
            Servings = 1
        });
        db.Recipes.Add(new Recipe
        {
            RecipeId = 2,
            Title = "TestRecipe",
            Instructions = "TestInstructions",
            ImageUrl = new Uri("http://localhost/"),
            Calories = 100,
            Servings = 1
        });

        db.Ingredients.Add(new Ingredient
        {
            IngredientId = 1,
            Description = "Marshmallow",
        });
        db.Ingredients.Add(new Ingredient
        {
            IngredientId = 2,
            Description = "Bread"
        });

        db.SaveChanges();
    }

    [SetUp]
    public void InitServices()
    {
        var db = GetMemoryContext();

        var fixture = new Fixture();

        var handler = new MockHttpMessageHandler();

        handler.When(HttpMethod.Get, "http://localhost/api/Recipe?search=Test")
            .Respond(HttpStatusCode.OK, JsonContent.Create(fixture.Build<Response<List<ReceiveServerRecipeDto>>>()
            .With(p => p.Data,
            [
                new ReceiveServerRecipeDto
                {
                    Title = "TestRecipe",
                    Instructions = "TestInstructions",
                    ImageUrl = new Uri("http://localhost/"),
                    Calories = 100,
                    Servings = 1
                }
            ])
            .Create()));

        handler.When(HttpMethod.Get, "http://localhost/api/Recipe?search=pageTwoTest")
            .Respond(HttpStatusCode.OK, JsonContent.Create(fixture.Build<Response<List<ReceiveServerRecipeDto>>>()
            .With(p => p.Data,
            fixture.Build<ReceiveServerRecipeDto>()
            .CreateMany(11).ToList())
            .Create()));

        handler.When(HttpMethod.Post, "http://localhost/api/Recipe")
            .Respond(HttpStatusCode.OK, JsonContent.Create(fixture.Build<Response<List<ReceiveServerRecipeDto>>>()
            .With(p => p.Data,
            [
                new ReceiveServerRecipeDto
                {
                    Title = "ChickenPasta",
                    Instructions = "Order online lmao",
                    ImageUrl = new Uri("http://localhost/"),
                    Calories = 100,
                    Servings = 1
                },
                new ReceiveServerRecipeDto
                {
                    Title = "TestRecipe2",
                    Instructions = "TestInstructions2",
                    ImageUrl = new Uri("http://localhost/"),
                    Calories = 100,
                    Servings = 1
                }
            ])
            .Create()));

        handler.When(HttpMethod.Get, "http://localhost/api/Recipe?search=NonExistent")
            .Respond(HttpStatusCode.OK, JsonContent.Create(fixture.Build<Response<List<ReceiveServerRecipeDto>>>()
                       .With(p => p.Data,
                                  [])
                       .Create()));

        handler.When(HttpMethod.Get, "http://localhost/?from=0&size=20&q=NonExistent")
            .Respond(HttpStatusCode.OK, JsonContent.Create(fixture.Build<FoodSystemAPI.DTOs.Tasty.Root>()
            .With(r => r.results, [])
            .Create()));


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
        _recipeService = new RecipeService(mockHttpClientFactory.Object, recipeMapper, memoryCacheService, ingredientService, recipeRepo, recipeIngredientRepo);
    }

    [Test]
    public async Task GetSavedRecipesAsync_RecipeExists_AllRecipesRetrieved()
    {
        var recipes = await _recipeService.GetSavedRecipesAsync(x => true);

        Assert.That(recipes, Is.Not.Null);
        Assert.That(recipes.Count, Is.GreaterThan(0));
    }

    [Test]
    public async Task GetRecipesAsync_RecipeExistsInAPIBasedOnQuery_RecipeIsRetrieved()
    {
        var paginationFilter = new PaginationFilter(1, 10);
        var recipes = await _recipeService.GetRecipesAsync("Test", paginationFilter);

        Assert.That(recipes, Is.Not.Null);
        Assert.That(recipes.Count, Is.EqualTo(1));
        Assert.That(recipes.First().Title, Is.EqualTo("TestRecipe"));
    }

    [Test]
    public async Task GetRecipesAsync_RecipeDoesNotExistInAPIBasedOnQuery_NoRecipeIsRetrieved()
    {
        var paginationFilter = new PaginationFilter(1, 10);
        var recipes = await _recipeService.GetRecipesAsync("NonExistent", paginationFilter);

        Assert.That(recipes, Is.Null);
    }

    [Test]
    public async Task GetRecipesAsync_TwoPagesOfRecipesExists_SecondPageIsretrieved()
    {
        var paginationFilter = new PaginationFilter(2, 10);
        var recipes = await _recipeService.GetRecipesAsync("pageTwoTest", paginationFilter);

        Assert.That(recipes, Is.Not.Null);
        Assert.That(recipes.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task AddRecipesForUserAsync_UserExistsRecipeIsValid_RecipeIsAddedWithIngredients()
    {
        var recipes = new List<ReceiveServerRecipeDto>
        {
            new ReceiveServerRecipeDto
            {
                Title = "Marshmallow Toast",
                Instructions = "Order online lmao",
                Ingredients = new string[] { "Marshmallow", "Bread" },
                IngredientIds = new int[] { 1, 2 },
                ImageUrl = new Uri("http://localhost/"),
                Calories = 100,
                Servings = 1
            },
            new ReceiveServerRecipeDto
            {
                Title = "TestRecipe2",
                Instructions = "TestInstructions2",
                Ingredients = new string[] {"TestIngredient1", "TestIngredient2" },
                ImageUrl = new Uri("http://localhost/"),
                Calories = 100,
                Servings = 1
            }
        };

        var result = await _recipeService.AddRecipesForUserAsync(recipes, 1);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.First(x => x.Title.Equals("Marshmallow Toast")).Ingredients, Is.Not.Null);
        Assert.That(result.First(x => x.Title.Equals("Marshmallow Toast")).Ingredients.Count, Is.EqualTo(2));
    }

    public FoodDbContext GetMemoryContext()
    {
        var options = new DbContextOptionsBuilder<FoodDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;

        return new FoodDbContext(options);
    }
}
