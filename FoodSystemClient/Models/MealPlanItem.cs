namespace FoodSystemClient.Models;

public class MealPlanItem
{
    public int MealPlanItemId { get; set; }
    public int MealPlanId { get; set; }
    public int RecipeId { get; set; }
    public Recipe Recipe { get; set; } = null!;
}