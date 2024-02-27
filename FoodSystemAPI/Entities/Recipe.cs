using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FoodSystemAPI.Entities;

public partial class Recipe
{
    public int RecipeId { get; set; }

    public int UserId { get; set; }

    public string Title { get; set; } = null!;

    public string Instructions { get; set; } = null!;

    public virtual IEnumerable<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

    public int Calories { get; set; }

    public int Servings { get; set; }

    public Uri ImageUrl { get; set; } = null!;

    [JsonIgnore]
    public virtual User User { get; set; } = null!;

    [JsonIgnore]
    public virtual MealPlanItem MealPlanItem { get; set; } = null!;
}
