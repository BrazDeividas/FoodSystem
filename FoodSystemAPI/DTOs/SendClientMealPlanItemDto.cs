namespace FoodSystemAPI.DTOs;

public class SendClientMealPlanItemDto
{
    public int MealPlanItemId { get; set; }
    public int MealPlanId { get; set; }
    public int RecipeId { get; set; }
    public SendClientRecipeDto Recipe { get; set; } = null!;
}