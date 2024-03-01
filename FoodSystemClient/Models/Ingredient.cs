namespace FoodSystemClient.Models;

public partial class Ingredient
{
    public int IngredientId { get; set; }

    public int CategoryId { get; set; }

    public string Description { get; set; } = null!;

    public int EnergyKcal { get; set; }

    public double ProteinG { get; set; }

    public double SaturatedFatsG { get; set; }

    public double FatG { get; set; }

    public double CarbG { get; set; }

    public double FiberG { get; set; }

    public double SugarG { get; set; }

    public double CalciumMg { get; set; }

    public double IronMg { get; set; }

    public double MagnesiumMg { get; set; }

    public double PotassiumMg { get; set; }

    public double SodiumMg { get; set; }

    public double ZincMg { get; set; }

    public double CopperMcg { get; set; }

    public double ManganeseMg { get; set; }

    public double SeleniumMcg { get; set; }

    public double VitcMg { get; set; }

    public double ThiaminMg { get; set; }

    public double RiboflavinMg { get; set; }

    public double NiacinMg { get; set; }

    public double Vitb6Mg { get; set; }

    public double FolateMcg { get; set; }

    public double Vitb12Mcg { get; set; }

    public double VitaMcg { get; set; }

    public double ViteMg { get; set; }

    public double Vitd2Mcg { get; set; }

    public string Category { get; set; } = null!;
    
    public bool Selected { get; set; } = false;
    
    public bool Owned { get; set; } = false;
}
