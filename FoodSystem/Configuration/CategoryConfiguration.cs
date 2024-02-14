namespace Configuration
{
    public static class CategoryConfiguration
    {
        public static Dictionary<int, List<string>> keywordCategorization = new Dictionary<int, List<string>>
        {
            {1, new List<string> {"egg", "milk", "dairy"}}, // Dairy & Eggs
            {2, new List<string> {"spice", "herb", }}, // Spices & Herbs
            {3, new List<string> {"baby", "formula"}}, // Baby Foods
            {4, new List<string> {"oil"}}, // Fats & Oils
            {5, new List<string> {"chicken", "poultry", "turkey"}}, // Poultry
            {6, new List<string> {"soup", "sauce", "gravy"}}, // Soups, Sauces & Gravies
            {7, new List<string> {"sausage"}}, // Sausages & Luncheon Meats
            {8, new List<string> {"breakfast"}}, // Breakfast Cereals
            {9, new List<string> {"juice", "coconut", "fruit", "mango", "pumpkin", "apple", "apricot", "banana", "berry", "cherry", "dates", "grapes", "fig", "lime", "orange", "pulp", "raisin", "melon"}}, // Fruits & Fruit Juices
            {10, new List<string> {"pork"}}, // Pork
            {11, new List<string> {"vegetable", "cabbage", "cucumber", "cauliflower", "tomato", "carrot", "potato", "ginger"}}, // Vegetables
            {12, new List<string> {"nut", "seed", "pistachio"}}, // Nut & Seed Products
            {13, new List<string> {"beef", "calf"}}, // Beef
            {14, new List<string> {"drink"}}, // Beverages
            {15, new List<string> {"fish"}}, // Finfish & Shellfish Products
            {16, new List<string> {"legume"}}, // Legumes & Legume Products
            {17, new List<string> {"goat", "sheep"}}, // Lamb, Veal, & Game Products
            {18, new List<string> {"baked"}}, // Baked Products
            {19, new List<string> {"sweet"}}, // Sweets
            {20, new List<string> {"cereal", "pasta"}}, // Cereal Grains & Pasta
            {21, new List<string> {"fast food"}}, // Fast Foods
            {22, new List<string> {"meal", "entree", "side"}}, // Meals, Entrees, & Side Dishes
            {23, new List<string> {"snack"}}, // Snacks
            {24, new List<string> {}}, // American Indian/Alaska Native Foods
            {25, new List<string> {"restaurant"}}, // Restaurant Foods
            {26, new List<string> {"brand"}}, // Branded Food Products
            {27, new List<string> {}}, // Quality Control Materials
            {28, new List<string> {"alcohol", "wine", "beer", "whiskey"}} // Alcoholic Beverages
        };

        public static int OtherCategory = 29; //changes based on the number of categories after insertion
    }
}