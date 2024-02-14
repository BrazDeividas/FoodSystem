using Configuration;
using Mapper;
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

        int count = 0;

        for(int i = 1; i < categories.Count; i++)
        {
/*             string query = $"INSERT INTO dbo.category (description) VALUES ('{categories[i][2]}')";
            count += dbService.CreateCommand(query);*/
            
            categoryCodes.Add(int.TryParse(categories[i][1], out int code) ? code : 0, categories[i][2]);
        }
        Console.WriteLine($"{count} categories inserted.");

        string query = $"INSERT INTO dbo.category (description) VALUES ('Other Food Items')"; // last category insertion
        dbService.CreateCommand(query);

        categoryCodes = categoryCodes.OrderByDescending(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value);

        List<Category> categoryList = dbService.ReadCategories(); 

        CategoryConfiguration.OtherCategory = categoryList.Last().Id;


        Console.WriteLine($"Reading {file2}...");

        var ingredients = csvReader.Read(path2);

        Console.Write($"{file2} labels:");
        
        foreach (var label in ingredients[0])
        {
            Console.Write(" " + label);
        }
        Console.Write("\n");

        Console.WriteLine("Inserting ingredients into database...");

        MapperBase<Ingredient, string[]> ingredientMapper = new IngredientMapper();

        count = 0;            

        for(int i = 1; i < ingredients.Count; i++)
        {
            int categoryId = 0;

            if(ingredients[i][1].Contains("alcohol")) //classification for alcohol separately
            {
                categoryId = categoryList.FirstOrDefault(x => x.Description == "Alcoholic Beverages").Id;
            }
            else //classification based on NDB number
            {
                var ndbNumber = int.TryParse(ingredients[i][0], out int s) ? s : 1;
                var categoryDescription = categoryCodes.FirstOrDefault(x => ndbNumber / (x.Key * 10) == 1).Value;
                var category = categoryList.FirstOrDefault(x => x.Description == categoryDescription);
                categoryId = category != null ? category.Id : 0;
            }

            if(categoryId == 0) //classification for unclassified food items
            {
                UnclassifiedFoodCategorizer unclassifiedFoodCategorizer = new UnclassifiedFoodCategorizer();
                categoryId = unclassifiedFoodCategorizer.Categorize(ingredients[i][1]);
            }

            var ingredient = ingredientMapper.Map(ingredients[i]);
            ingredient.CategoryId = categoryId;
            count += dbService.InsertIngredient(ingredient);
        }

        Console.WriteLine($"{count} ingredients inserted.");
    }
}