using System.ComponentModel.DataAnnotations;

namespace InternalAPI.DTOs;

public class UpdateRecipeDTO
{
    [Required]
    [MaxLength(50)]
    public string Title { get; set; }
    [Required]
    public string Instructions { get; set; }
    [Required]
    public List<string> Ingredients { get; set; }
    [Required]
    [Range(0, int.MaxValue)]
    public int Calories { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int Servings { get; set; }
    [Url]
    public string ImageUrl { get; set; }
}