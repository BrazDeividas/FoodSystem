using System.ComponentModel.DataAnnotations;

namespace InternalAPI.Filters;

public class SearchFilter
{
    public string Search { get; set; }
    [Required]
    public int CalorieSum { get; set; }
    public int NumberOfMeals { get; set; }
    public int Days { get; set; }

    public SearchFilter()
    {
        Search = "";
        CalorieSum = 0;
        NumberOfMeals = 3;
        Days = 1;
    }

    public SearchFilter(string search, int calorieSum, int numberOfMeals, int days)
    {
        Search = search;
        CalorieSum = calorieSum < 0 ? 0 : calorieSum;
        NumberOfMeals = numberOfMeals < 1 ? 1 : numberOfMeals;
        Days = days < 1 ? 1 : days;
    }
}