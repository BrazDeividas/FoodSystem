using AutoFixture;
using AutoMapper;
using FoodSystemAPI.DTOs;
using FoodSystemAPI.Entities;
using FoodSystemAPI.Filters;
using FoodSystemAPI.Profiles;
using FoodSystemAPI.Repositories;
using FoodSystemAPI.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Tests.FoodSystemAPITests;

internal class FoodSystemAPI_IngredientServiceTests
{
    private IngredientService _ingredientService = null!;

    [SetUp]
    public void InitDb()
    {
        var db = GetMemoryContext();
        db.Database.EnsureDeleted();

        db.Ingredients.Add(new Ingredient { CalciumMg = 100, CarbG = 100, EnergyKcal = 1900, FatG = 100, FiberG = 100, IronMg = 100, MagnesiumMg = 100, PotassiumMg = 100, ProteinG = 100, SodiumMg = 100, SugarG = 100, ZincMg = 100, Description = "TestIngredient" });
        db.Ingredients.Add(new Ingredient { CalciumMg = 100, CarbG = 100, EnergyKcal = 1700, FatG = 100, FiberG = 100, IronMg = 100, MagnesiumMg = 100, PotassiumMg = 100, ProteinG = 100, SodiumMg = 100, SugarG = 100, ZincMg = 100, Description = "TestIngredient2" });
        db.Ingredients.Add(new Ingredient { CalciumMg = 100, CarbG = 100, EnergyKcal = 1500, FatG = 100, FiberG = 100, IronMg = 100, MagnesiumMg = 100, PotassiumMg = 100, ProteinG = 100, SodiumMg = 100, SugarG = 100, ZincMg = 100, Description = "TestIngredient3" });
        db.Ingredients.Add(new Ingredient { CalciumMg = 100, CarbG = 100, EnergyKcal = 1900, FatG = 100, FiberG = 100, IronMg = 100, MagnesiumMg = 100, PotassiumMg = 100, ProteinG = 100, SodiumMg = 100, SugarG = 100, ZincMg = 100, Description = "RegularIngredient" });
        db.SaveChanges();
    }

    [SetUp]
    public void InitServices()
    {
        var db = GetMemoryContext();

        var fixture = new Fixture();
        var ingredientRepo = new Repository<Ingredient>(db);
        var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<IngredientProfile>()));

        _ingredientService = new IngredientService(ingredientRepo, mapper);
    }

    [Test]
    public async Task GetAll_TestIngredientsExistByExpression_ReturnsTestIngredients()
    {
        var result = await _ingredientService.GetAll(i => i.Description.Contains("Test"));

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(3));
        Assert.That(result.All(i => i.Description.Contains("Test")), Is.True);
    }

    [Test]
    public async Task GetAll_PaginationIngredientsExist_ThirdPageIsReturned()
    {
        var pageSize = 1;
        var pageNumber = 3;

        var pagination = new PaginationFilter(pageNumber, pageSize);
        var result = await _ingredientService.GetAll(pagination);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(pageSize));
    }

    [Test]
    public async Task MatchIngredients_IngredientNamesAreSimilar_IngredientsAreMatched()
    {
        string[] ingredients = ["test", "regular"];

        var result = await _ingredientService.MatchIngredients(ingredients);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result.ElementAt(0).Description, Is.EqualTo("TestIngredient"));
        Assert.That(result.ElementAt(1).Description, Is.EqualTo("RegularIngredient"));
    }

    [Test]
    public async Task GetById_IngredientExists_IngredientReturned()
    {
        var ingredient = await _ingredientService.GetById(1);

        Assert.That(ingredient, Is.Not.Null);   
        Assert.That(ingredient.IngredientId, Is.EqualTo(1));
    }

    [Test]
    public async Task GetById_IngredientDoesNotExist_NullReturned()
    {
        var ingredient = await _ingredientService.GetById(100);

        Assert.That(ingredient, Is.Null);
    }

    [Test]
    public async Task Add_IngredientDtoIsValid_IngredientIsAdded()
    {
        var ingredientDto = new PostIngredientDto
        {
            CalciumMg = 100,
            CarbG = 100,
            EnergyKcal = 1900,
            FatG = 100,
            FiberG = 100,
            IronMg = 100,
            MagnesiumMg = 100,
            PotassiumMg = 100,
            ProteinG = 100,
            SodiumMg = 100,
            SugarG = 100,
            ZincMg = 100,
            Description = "TestIngredient4"
        };

        var result = await _ingredientService.Add(ingredientDto);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Description, Is.EqualTo("TestIngredient4"));
        Assert.That(result.IngredientId, Is.Positive);
    }

    [Test]
    public async Task Update_IngredientDtoIsValid_IngredientIsUpdated()
    {
        var updateIngredient = new Ingredient
        {
            IngredientId = 4,
            CalciumMg = 100,
            CarbG = 100,
            EnergyKcal = 1900,
            FatG = 100,
            FiberG = 100,
            IronMg = 100,
            MagnesiumMg = 100,
            PotassiumMg = 100,
            ProteinG = 100,
            SodiumMg = 100,
            SugarG = 100,
            ZincMg = 100,
            Description = "TestIngredient4"
        };

        var result = _ingredientService.Update(updateIngredient);

        var allIngredients = await _ingredientService.GetAll(x => true);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Description, Is.EqualTo("TestIngredient4"));
        Assert.That(result.IngredientId, Is.EqualTo(4));
        Assert.That(allIngredients.FirstOrDefault(x => x.Description == "RegularIngredient"), Is.Null);
    }

    [Test]
    public async Task Delete_IngredientExists_IngredientIsDeleted()
    {
        var result = await _ingredientService.Delete(4);

        var allIngredients = await _ingredientService.GetAll(x => true);

        Assert.That(result.IngredientId, Is.EqualTo(4));
        Assert.That(allIngredients.FirstOrDefault(x => x.IngredientId == 4), Is.Null);
    }

    [Test]
    public async Task Delete_IngredientDoesNotExist_NullReturned()
    {
        var result = await _ingredientService.Delete(100);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task CountAsync_FourIngredientsExist_FourIsReturned()
    {
        var numberOfIngredients = await _ingredientService.CountAsync();

        Assert.That(numberOfIngredients, Is.EqualTo(4));
    }



    public FoodDbContext GetMemoryContext()
    {
        var options = new DbContextOptionsBuilder<FoodDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;

        return new FoodDbContext(options);
    }
}
