namespace FoodSystemAPI.Entities;

public class UserPoints
{
    public int UserPointsId { get; set; }
    public int UserId { get; set; }
    public int Points { get; set; } = 100;
    public virtual User User { get; set; } = null!;
}