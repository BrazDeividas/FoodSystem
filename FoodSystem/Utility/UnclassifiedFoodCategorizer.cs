using Configuration;

namespace Utility
{
    public class UnclassifiedFoodCategorizer 
    {
        public UnclassifiedFoodCategorizer() 
        { 
        }

        public int Categorize(string ingredient)
        {
            foreach (var category in CategoryConfiguration.keywordCategorization)
            {
                foreach (var keyword in category.Value)
                {
                    if (ingredient.Contains(keyword))
                    {
                        return category.Key;
                    }
                }
            }
            return CategoryConfiguration.OtherCategory;
        }
    }
}