using System.ComponentModel.DataAnnotations;

namespace InternalAPI.Filters;

public class SearchFilter
{
    public string Search { get; set; }
    [Required]
    public int CalorieSum { get; set; }
    public int NumberOfMeals { get; set; }

    public SearchFilter()
    {
        Search = "";
        CalorieSum = 0;
        NumberOfMeals = 3;
    }

    public SearchFilter(string search, int calorieSum, int numberOfMeals)
    {
        Search = search;
        CalorieSum = calorieSum < 0 ? 0 : calorieSum;
        NumberOfMeals = numberOfMeals < 1 ? 1 : numberOfMeals;
    }
}