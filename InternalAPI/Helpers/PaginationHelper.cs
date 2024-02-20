using System.Collections.Specialized;
using InternalAPI.Filters;
using InternalAPI.Services;
using InternalAPI.Wrappers;
using Microsoft.Extensions.Primitives;

namespace InternalAPI.Helpers;

public static class PaginationHelper 
{
    public static PagedResponse<IEnumerable<T>> CreatePagedResponse<T>(IEnumerable<T> pagedData, PaginationFilter validFilter, int totalRecords, IUriService uriService, string route, IEnumerable<KeyValuePair<string, StringValues>> otherParameters)
    {
        var response = new PagedResponse<IEnumerable<T>>(pagedData, validFilter.PageNumber, validFilter.PageSize);
        var totalPages = ((double)totalRecords) / ((double)validFilter.PageSize);
        int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));

        response.NextPage =
            validFilter.PageNumber >= 1 && validFilter.PageNumber < roundedTotalPages
            ? uriService.GetPageUri(new PaginationFilter(validFilter.PageNumber + 1, validFilter.PageSize), route, otherParameters)
            : null;
        response.PreviousPage =
            validFilter.PageNumber - 1 >= 1 && validFilter.PageNumber <= roundedTotalPages
            ? uriService.GetPageUri(new PaginationFilter(validFilter.PageNumber - 1, validFilter.PageSize), route, otherParameters)
            : null;
        response.FirstPage = uriService.GetPageUri(new PaginationFilter(1, validFilter.PageSize), route, otherParameters);
        response.LastPage = uriService.GetPageUri(new PaginationFilter(roundedTotalPages, validFilter.PageSize), route, otherParameters);
        response.TotalPages = roundedTotalPages;
        response.TotalRecords = totalRecords;

        return response;
    }

    public static NameValueCollection TrimPaginationParameters(NameValueCollection query)
    {
        query.Remove("pageNumber");
        query.Remove("pageSize");
        return query;
    }
}