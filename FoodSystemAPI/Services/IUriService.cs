using FoodSystemAPI.Filters;
using Microsoft.Extensions.Primitives;

namespace FoodSystemAPI.Services;

public interface IUriService
{
    public Uri GetPageUri(PaginationFilter filter, string route);
    public Uri GetPageUri(PaginationFilter filter, string route, IEnumerable<KeyValuePair<string, StringValues>> otherParameters);
}