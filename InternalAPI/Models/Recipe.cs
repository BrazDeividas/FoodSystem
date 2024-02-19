using System.ComponentModel.DataAnnotations;

namespace InternalAPI.Models;

public class Recipe
{
    [Key]
    public int RecipeId { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Instructions { get; set; }
    [Required]
    public virtual List<string> Ingredients { get; set; } = new List<string>();
    [Required]
    public int Calories { get; set; }
    [Required]
    public int Servings { get; set; }
    public string ImageUrl { get; set; }
}