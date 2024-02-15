using System;
using System.Collections.Generic;

namespace FoodSystemAPI.Entities;

public partial class RecipeIngredient
{
    public int RecipeId { get; set; }

    public int IngredientId { get; set; }

    public int Amount { get; set; }

    public string AmountType { get; set; } = null!;

    public virtual Ingredient Ingredient { get; set; } = null!;

    public virtual Recipe Recipe { get; set; } = null!;
}
