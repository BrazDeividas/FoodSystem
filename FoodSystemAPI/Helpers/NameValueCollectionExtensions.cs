using System.Collections.Specialized;
using Microsoft.Extensions.Primitives;

namespace FoodSystemAPI.Helpers;

public static class NameValueCollectionExtensions {
    public static IEnumerable<KeyValuePair<string, StringValues>> AsEnumerable(this NameValueCollection query)
    {
        return query?.Cast<string>().Select((s, ix) => new KeyValuePair<string, StringValues>(s, query[ix])) 
            ?? Enumerable.Empty<KeyValuePair<string, StringValues>>();
    }
}