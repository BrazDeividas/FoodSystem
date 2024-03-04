namespace FoodSystemClient.Models;

public class MealPlan
{
    public int MealPlanId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TotalCalories { get; set; }
    public IEnumerable<MealPlanItem> MealPlanItems { get; set; } = new List<MealPlanItem>();
}