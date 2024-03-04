using AutoMapper;
using FoodSystemAPI.Entities;
using FoodSystemAPI.Profiles;
using FoodSystemAPI.Repositories;
using FoodSystemAPI.Services;
using FoodSystemAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Tests.FoodSystemAPITests;

internal class FoodSystemAPI_UserServiceTests
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
    public async Task AddUserMetricsAsync_UserDoesNotExist_NullReturned()
    {
        var db = GetMemoryContext();
        var userMetricsRepo = new Repository<UserMetrics>(db);
        var userRepo = new Repository<User>(db);
        var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<UserMetricsProfile>()));
        var userService = new UserService(userMetricsRepo, userRepo, mapper);

        Assert.IsNull(await userService.AddUserMetricsAsync(new PostUserMetricsDto
        {
            Weight = 100,
            Height = 190,
            Age = 25,
            Sex = "Male",
            ActivityLevel = "Sedentary"
        }, 37));
    }

    [Test]
    public async Task AddUserMetricsAsync_UserExists_UserMetricsReturned()
    {
        var db = GetMemoryContext();
        var userMetricsRepo = new Repository<UserMetrics>(db);
        var userRepo = new Repository<User>(db);
        var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<UserMetricsProfile>()));
        var userService = new UserService(userMetricsRepo, userRepo, mapper);

        Assert.IsNotNull(await userService.AddUserMetricsAsync(new PostUserMetricsDto
        {
            Weight = 100,
            Height = 190,
            Age = 25,
            Sex = "Female",
            ActivityLevel = "ExtraActive"
        }, 1));
    }

    [Test]
    public async Task AddIngredientsToUserAsync_UserAndIngredientsExist_IngredientsAddedToUser()
    {
        var db = GetMemoryContext();
        var userMetricsRepo = new Repository<UserMetrics>(db);
        var userRepo = new Repository<User>(db);
        var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<UserMetricsProfile>()));
        var userService = new UserService(userMetricsRepo, userRepo, mapper);

        var ingredientRepo = new Repository<Ingredient>(db);
        var ingredients = (await ingredientRepo.GetAll()).ToList();


        await userService.AddIngredientsToUserAsync(ingredients, 1);
        Assert.IsTrue((await userRepo.GetAllInclude(x => x.UserId == 1, x => x.Ingredients)).First().Ingredients.Count == 1);
    }

    [Test]
    public async Task GetUserByUsername_UserExists_UserMatchingUsernameIsReturned()
    {
        var db = GetMemoryContext();
        var userMetricsRepo = new Repository<UserMetrics>(db);
        var userRepo = new Repository<User>(db);    
        var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<UserMetricsProfile>()));
        var userService = new UserService(userMetricsRepo, userRepo, mapper);

        var user = await userService.GetUserByUsername("jonnyboi01");
        Assert.IsNotNull(user);
        Assert.IsInstanceOf<User>(user);
        Assert.That(user.Username, Is.EqualTo("jonnyboi01"));
    }

    public FoodDbContext GetMemoryContext()
    {
        var options = new DbContextOptionsBuilder<FoodDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;

        return new FoodDbContext(options);
    }
}
