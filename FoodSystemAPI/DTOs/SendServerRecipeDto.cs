namespace FoodSystemAPI.DTOs;

public class SendServerRecipeDto
{
    public string Title { get; set; }
    public string Instructions { get; set; }
    public IEnumerable<string> Ingredients { get; set; } = new List<string>();
    public IEnumerable<int> IngredientIds { get; set; } = new List<int>();
    public int Calories { get; set; }
    public int Servings { get; set; }
    public string ImageUrl { get; set; }
    public string SourceAPI { get; set; }
    public int SourceId { get; set; }
}