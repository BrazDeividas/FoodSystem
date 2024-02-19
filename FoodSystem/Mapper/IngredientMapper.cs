using Model;

namespace Mapper
{
    public sealed class IngredientMapper : MapperBase<Ingredient, string[]>
    {
        public override Ingredient Map(string[] ingredient)
        {
            return new Ingredient
                {
                    Description = ingredient[1],
                    Energy_kcal = float.TryParse(ingredient[2], out float v1) ? (int)v1 : 0,
                    Protein_g = float.TryParse(ingredient[3], out float v2) ? v2 : 0,
                    Saturated_fats_g = float.TryParse(ingredient[4], out float v3) ? v3: 0,
                    Fat_g = float.TryParse(ingredient[5], out float v4) ? v4 : 0,
                    Carb_g = float.TryParse(ingredient[6], out float v5) ? v5 : 0,
                    Fiber_g = float.TryParse(ingredient[7], out float v6) ? v6 : 0,
                    Sugar_g = float.TryParse(ingredient[8], out float v7) ? v7 : 0,
                    Calcium_mg = float.TryParse(ingredient[9], out float v8) ? v8 : 0,
                    Iron_mg = float.TryParse(ingredient[10], out float v9) ? v9 : 0,
                    Magnesium_mg = float.TryParse(ingredient[11], out float v10) ? v10 : 0,
                    Potassium_mg = float.TryParse(ingredient[12], out float v11) ? v11 : 0,
                    Sodium_mg = float.TryParse(ingredient[13], out float v12) ? v12 : 0,
                    Zinc_mg = float.TryParse(ingredient[14], out float v13) ? v13 : 0,
                    Copper_mcg = float.TryParse(ingredient[15], out float v14) ? v14 : 0,
                    Manganese_mg = float.TryParse(ingredient[16], out float v15) ? v15 : 0,
                    Selenium_mcg = float.TryParse(ingredient[17], out float v16) ? v16 : 0,
                    Vitc_mg = float.TryParse(ingredient[18], out float v17) ? v17 : 0,
                    Thiamin_mg = float.TryParse(ingredient[19], out float v18) ? v18 : 0,
                    Riboflavin_mg = float.TryParse(ingredient[20], out float v19) ? v19 : 0,
                    Niacin_mg = float.TryParse(ingredient[21], out float v20) ? v20 : 0,
                    Vitb6_mg = float.TryParse(ingredient[22], out float v21) ? v21 : 0,
                    Folate_mcg = float.TryParse(ingredient[23], out float v22) ? v22 : 0,
                    Vitb12_mcg = float.TryParse(ingredient[24], out float v23) ? v23 : 0,
                    Vita_mcg = float.TryParse(ingredient[25], out float v24) ? v24 : 0,
                    Vite_mg = float.TryParse(ingredient[26], out float v25) ? v25 : 0,
                    Vitd2_mcg = float.TryParse(ingredient[27], out float v26) ? v26 : 0
                };
        }

        public override string[] Map(Ingredient ingredient)
        {
            throw new NotImplementedException();
        }
    }
}