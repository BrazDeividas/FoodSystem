using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FoodSystemClient.DTOs;

public class PostIngredientDto
{
    [Required]
    public string Description { get; set; } = null!;

    [Required]
    [DisplayName("Category")]
    public string Category { get; set; } = null!;

    [Required]
    [DisplayName("Energy (kcal)")]
    public int EnergyKcal { get; set; }

    [Required]
    [DisplayName("Protein (g)")]
    public double ProteinG { get; set; }

    [Required]
    [DisplayName("Saturated Fats (g)")]
    public double SaturatedFatsG { get; set; }

    [Required]
    [DisplayName("Fat (g)")]
    public double FatG { get; set; }

    [Required]
    [DisplayName("Carbohydrates (g)")]
    public double CarbG { get; set; }

    [Required]
    [DisplayName("Sugar (g)")]
    public double SugarG { get; set; }

    [Required]
    [DisplayName("Sodium (mg)")]
    public double SodiumMg { get; set; }

    [DisplayName("Fiber (g)")]
    public double FiberG { get; set; }

    [DisplayName("Calcium (mg)")]
    public double CalciumMg { get; set; }

    [DisplayName("Iron (mg)")]
    public double IronMg { get; set; }

    [DisplayName("Magnesium (mg)")]
    public double MagnesiumMg { get; set; }

    [DisplayName("Potassium (mg)")]
    public double PotassiumMg { get; set; }

    [DisplayName("Zinc (mg)")]
    public double ZincMg { get; set; }

    [DisplayName("Copper (mcg)")]
    public double CopperMcg { get; set; }

    [DisplayName("Manganese (mg)")]
    public double ManganeseMg { get; set; }

    [DisplayName("Selenium (mcg)")]
    public double SeleniumMcg { get; set; }

    [DisplayName("Vitamin C (mg)")]
    public double VitcMg { get; set; }

    [DisplayName("Thiamin (mg)")]
    public double ThiaminMg { get; set; }

    [DisplayName("Riboflavin (mg)")]
    public double RiboflavinMg { get; set; }

    [DisplayName("Niacin (mg)")]
    public double NiacinMg { get; set; }

    [DisplayName("Vitamin B6 (mg)")]
    public double Vitb6Mg { get; set; }

    [DisplayName("Folate (mcg)")]
    public double FolateMcg { get; set; }

    [DisplayName("Vitamin B12 (mcg)")]
    public double Vitb12Mcg { get; set; }

    [DisplayName("Vitamin A (mcg)")]
    public double VitaMcg { get; set; }

    [DisplayName("Vitamin E (mg)")]
    public double ViteMg { get; set; }

    [DisplayName("Vitamin D2 (mcg)")]
    public double Vitd2Mcg { get; set; }
}