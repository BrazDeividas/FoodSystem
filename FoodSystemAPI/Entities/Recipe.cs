using System;
using System.Collections.Generic;

namespace FoodSystemAPI.Entities;

public partial class Recipe
{
    public int RecipeId { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int PreparationTime { get; set; }

    public int CookingTime { get; set; }

    public int Servings { get; set; }

    public virtual User User { get; set; } = null!;
}
