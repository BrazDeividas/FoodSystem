namespace FoodSystemAPI.DTOs;

public class SendClientMealPlanDto
{
    public int MealPlanId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TotalCalories { get; set; }
    public IEnumerable<SendClientMealPlanItemDto> MealPlanItems { get; set; } = new List<SendClientMealPlanItemDto>();
}