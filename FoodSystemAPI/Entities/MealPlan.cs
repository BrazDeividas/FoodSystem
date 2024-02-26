using System.Text.Json.Serialization;

namespace FoodSystemAPI.Entities;

public class MealPlan 
{
    public int MealPlanId { get; set; }

    public int UserId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int TotalCalories { get; set; } 

    [JsonIgnore]
    public virtual User User { get; set; } = null!;

    public virtual ICollection<MealPlanItem> MealPlanItems { get; set; } = new List<MealPlanItem>();
}