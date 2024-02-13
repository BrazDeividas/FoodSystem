using Model;
using Utility;

internal class Program
{
    private static void Main()
    {
        string file1 = "food_category.csv";
        string file2 = "cleaned_ingredients.csv";

        string connectionString = "Server=54499C3;Database=test;User Id=sa;Trusted_Connection=True";

        string path1 = Path.Combine(Environment.CurrentDirectory, @"../../../import_data", file1);
        string path2 = Path.Combine(Environment.CurrentDirectory, @"../../../import_data", file2);

        CSVReader csvReader = new CSVReader();
        DBService dbService = new DBService(connectionString);

        dbService.TestConnection();


        Console.WriteLine($"Reading {file1}...");

        var categories = csvReader.Read(path1);
        Dictionary<int, string> categoryCodes = new Dictionary<int, string>();

        Console.Write($"{file1} labels:");

        foreach (var label in categories[0])
        {
            Console.Write(" " + label);
        }
        Console.Write("\n");

        Console.WriteLine("Inserting categories into database...");

        for(int i = 1; i < categories.Count; i++)
        {
/*             string query = $"INSERT INTO dbo.category (description) VALUES ('{categories[i][2]}')";
            dbService.CreateCommand(query); */
            
            categoryCodes.Add(int.TryParse(categories[i][1], out int code) ? code : 0, categories[i][2]);
        }
        categoryCodes = categoryCodes.OrderByDescending(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value);

        List<Category> categoryList = dbService.ReadCategories(); 

        Console.WriteLine($"Reading {file2}...");

        var ingredients = csvReader.Read(path2);

        Console.Write($"{file2} labels:");
        
        foreach (var label in ingredients[0])
        {
            Console.Write(" " + label);
        }
        Console.Write("\n");

        Console.WriteLine("Inserting ingredients into database...");

        for(int i = 1; i < ingredients.Count; i++)
        {
            int categoryId = 0;
            if(ingredients[i][1].Contains("alcohol"))
            {
                categoryId = categoryList.FirstOrDefault(x => x.Description == "Alcoholic Beverages").Id;
            }
            else
            {
                var ndbNumber = int.TryParse(ingredients[i][0], out int s) ? s : 1;
                var categoryDescription = categoryCodes.FirstOrDefault(x => ndbNumber / (x.Key * 10) == 1).Value;
                var category = categoryList.FirstOrDefault(x => x.Description == categoryDescription);
                categoryId = category != null ? category.Id : 0;
            }

            Console.WriteLine(categoryId);

            if(categoryId != 0) 
            {
                dbService.InsertIngredient(new Ingredient
                {
                    Description = ingredients[i][1],
                    CategoryId = categoryId,
                    Energy_kcal = int.TryParse(ingredients[i][2], out int v1) ? v1 : 0,
                    Protein_g = float.TryParse(ingredients[i][3], out float v2) ? v2 : 0,
                    Saturated_fats_g = float.TryParse(ingredients[i][4], out float v3) ? v3: 0,
                    Fat_g = float.TryParse(ingredients[i][5], out float v4) ? v4 : 0,
                    Carb_g = float.TryParse(ingredients[i][6], out float v5) ? v5 : 0,
                    Fiber_g = float.TryParse(ingredients[i][7], out float v6) ? v6 : 0,
                    Sugar_g = float.TryParse(ingredients[i][8], out float v7) ? v7 : 0,
                    Calcium_mg = float.TryParse(ingredients[i][9], out float v8) ? v8 : 0,
                    Iron_mg = float.TryParse(ingredients[i][10], out float v9) ? v9 : 0,
                    Magnesium_mg = float.TryParse(ingredients[i][11], out float v10) ? v10 : 0,
                    Potassium_mg = float.TryParse(ingredients[i][12], out float v11) ? v11 : 0,
                    Sodium_mg = float.TryParse(ingredients[i][13], out float v12) ? v12 : 0,
                    Zinc_mg = float.TryParse(ingredients[i][14], out float v13) ? v13 : 0,
                    Copper_mcg = float.TryParse(ingredients[i][15], out float v14) ? v14 : 0,
                    Manganese_mg = float.TryParse(ingredients[i][16], out float v15) ? v15 : 0,
                    Selenium_mcg = float.TryParse(ingredients[i][17], out float v16) ? v16 : 0,
                    Vitc_mg = float.TryParse(ingredients[i][18], out float v17) ? v17 : 0,
                    Thiamin_mg = float.TryParse(ingredients[i][19], out float v18) ? v18 : 0,
                    Riboflavin_mg = float.TryParse(ingredients[i][20], out float v19) ? v19 : 0,
                    Niacin_mg = float.TryParse(ingredients[i][21], out float v20) ? v20 : 0,
                    Vitb6_mg = float.TryParse(ingredients[i][22], out float v21) ? v21 : 0,
                    Folate_mcg = float.TryParse(ingredients[i][23], out float v22) ? v22 : 0,
                    Vitb12_mcg = float.TryParse(ingredients[i][24], out float v23) ? v23 : 0,
                    Vita_mcg = float.TryParse(ingredients[i][25], out float v24) ? v24 : 0,
                    Vite_mg = float.TryParse(ingredients[i][26], out float v25) ? v25 : 0,
                    Vitd2_mcg = float.TryParse(ingredients[i][27], out float v26) ? v26 : 0
                });
            }
        }
    }
}