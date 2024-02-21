namespace FoodSystemAPI.DTOs;

public class RapidAPIDto_1
{
    public int s { get; set; }
    public IEnumerable<RapidAPIRecipeDto_1> d { get; set; } = new List<RapidAPIRecipeDto_1>();
    public int t { get; set; }
    public RapidAPIPaginationDto_1 p { get; set; }
}

public class RapidAPIPaginationDto_1
{
    public int limitstart { get; set; }
    public int limit { get; set; }
    public int total { get; set; }
    public int pagesStart { get; set; }
    public int pagesStop { get; set; }
    public int pagesCurrent { get; set; }
    public int pagesTotal { get; set; }
}

public class RapidAPIRecipeDto_1
{
    public string id { get; set; }
    public string Title { get; set; }
    public Dictionary<string, string> Ingredients { get; set; } = new Dictionary<string, string>();
    public string Instructions { get; set; }
    public Uri Image { get; set; }
}