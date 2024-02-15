using FoodSystemAPI.Filters;

namespace FoodSystemAPI.Services;

public interface IUriService
{
    public Uri GetPageUri(PaginationFilter filter, string route);
}