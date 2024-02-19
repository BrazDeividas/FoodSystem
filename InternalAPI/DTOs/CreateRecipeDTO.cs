namespace InternalAPI.DTOs;

public class CreateRecipeDTO
{
    public string Title { get; set; }
    public string Instructions { get; set; }
    public List<string> Ingredients { get; set; }
    public int Calories { get; set; }
    public int Servings { get; set; }
    public string ImageUrl { get; set; }
}