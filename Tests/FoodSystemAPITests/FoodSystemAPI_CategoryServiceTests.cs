
using AutoFixture;
using AutoMapper;
using FoodSystemAPI.DTOs;
using FoodSystemAPI.Entities;
using FoodSystemAPI.Profiles;
using FoodSystemAPI.Repositories;
using FoodSystemAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace Tests.FoodSystemAPITests;

internal class FoodSystemAPI_CategoryServiceTests
{
    private CategoryService _categoryService = null!;

    [SetUp]
    public void InitDb()
    {
        var db = GetMemoryContext();
        db.Database.EnsureDeleted();

        db.Categories.Add(new Category { Description = "TestCategory" });
        db.Categories.Add(new Category { Description = "TestCategory2" });
        db.Categories.Add(new Category { Description = "TestCategory3" });
        db.Categories.Add(new Category { Description = "TestCategory4" });
        db.SaveChanges();
    }

    [SetUp]
    public void InitServices()
    {
        var db = GetMemoryContext();

        var fixture = new Fixture();
        var categoryRepo = new Repository<Category>(db);
        var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<CategoryProfile>()));

        _categoryService = new CategoryService(categoryRepo, mapper);
    }

    [Test]
    public async Task GetAll_TestCategoriesExist_ReturnsTestCategories()
    {
        var result = await _categoryService.GetAll();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(4));
        Assert.That(result.All(c => c.Description.Contains("TestCategory")), Is.True);
    }

    [Test]
    public async Task GetById_IngredientExists_ReturnsIngredient()
    {
        var result = await _categoryService.GetById(1);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Description, Is.EqualTo("TestCategory"));
    }

    [Test]
    public async Task Add_ValidCategory_CategoryIsAdded()
    {
        var newCategory = new PostCategoryDto
        {
            Description = "TestCategory5"
        };

        var result = await _categoryService.Add(newCategory);

        var categories = await _categoryService.GetAll();

        Assert.That(result, Is.Not.Null);
        Assert.That(categories.Count, Is.EqualTo(5));
        Assert.That(categories.Any(c => c.Description == "TestCategory5"), Is.True);
    }

    [Test]
    public async Task Update_ValidCategory_CategoryIsUpdated()
    {
        var updatedCategory = new Category
        {
            CategoryId = 1,
            Description = "TestCategoryUpdated"
        };

        _categoryService.Update(updatedCategory);

        var category = await _categoryService.GetById(1);

        Assert.That(category, Is.Not.Null);
        Assert.That(category.Description, Is.EqualTo("TestCategoryUpdated"));
    }

    [Test]
    public async Task Delete_IngredientIdIsValid_IngredientIsDeleted()
    {
        var result = await _categoryService.Delete(1);

        var categories = await _categoryService.GetAll();

        Assert.That(result, Is.Not.Null);
        Assert.That(categories.Count, Is.EqualTo(3));
        Assert.That(categories.Any(c => c.CategoryId == 1), Is.False);
    }

    public FoodDbContext GetMemoryContext()
    {
        var options = new DbContextOptionsBuilder<FoodDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;

        return new FoodDbContext(options);
    }
}
