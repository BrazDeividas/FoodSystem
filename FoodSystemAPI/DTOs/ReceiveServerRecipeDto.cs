using System.ComponentModel.DataAnnotations.Schema;

namespace FoodSystemAPI.DTOs;

public class ReceiveServerRecipeDto
{
    public int RecipeId { get; set; }
    public string Title { get; set; }
    public string Instructions { get; set; }
    public IEnumerable<string> Ingredients { get; set; } = new List<string>();
    public IEnumerable<int> IngredientIds { get; set; } = new List<int>();
    public int Calories { get; set; }
    public int Servings { get; set; }
    public Uri ImageUrl { get; set; }
    public string SourceAPI { get; set; }
    public int SourceId { get; set; }
}