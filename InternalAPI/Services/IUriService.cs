using InternalAPI.Filters;
using Microsoft.Extensions.Primitives;

namespace InternalAPI.Services;

public interface IUriService
{
    public Uri GetPageUri(PaginationFilter filter, string route);
    public Uri GetPageUri(PaginationFilter filter, string route, IEnumerable<KeyValuePair<string, StringValues>> otherParameters);
}