using System.Text.Json.Serialization;

namespace FoodSystemClient.Models;

public class Recipe
{
    public int RecipeId { get; set; }

    public string Title { get; set; } = null!;

    public string Instructions { get; set; } = null!;

    public IEnumerable<string> Ingredients { get; set; } = new List<string>();
    
    public IEnumerable<int> IngredientIds { get; set; } = new List<int>();

    public int Calories { get; set; }

    public int Servings { get; set; }

    public Uri ImageUrl { get; set; } = null!;
}
