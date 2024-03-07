using FoodSystemAPI.Filters;
using FoodSystemAPI.Services;
using Microsoft.Extensions.Primitives;

namespace Tests.FoodSystemAPITests;

internal class FoodSystemAPI_UriServiceTests
{
    private UriService _uriService = null!;

    [SetUp]
    public void Init()
    {
        _uriService = new UriService("http://localhost/");
    }

    [Test]
    public void GetPageUri_ValidPaginationAndRoute_ReturnsConstructedUri()
    {
        var route = "api/Ingredients";
        var paginationFilter = new PaginationFilter(1, 10);
        var result = _uriService.GetPageUri(paginationFilter, route);
        Assert.That(result.ToString(), Is.EqualTo("http://localhost/api/Ingredients?pageNumber=1&pageSize=10"));
    }

    [Test]
    public void GetPageUri_ValidPaginationRouteAndOtherParameters_ReturnsConstructedUri()
    {
        var route = "api/Ingredients";
        var paginationFilter = new PaginationFilter(1, 10);
        var otherParameters = new List<KeyValuePair<string, StringValues>>
        {
            new KeyValuePair<string, StringValues>("test", "test"),
            new KeyValuePair<string, StringValues>("test2", "test2")
        };
        var result = _uriService.GetPageUri(paginationFilter, route, otherParameters);
        Assert.That(result.ToString(), Is.EqualTo("http://localhost/api/Ingredients?pageNumber=1&pageSize=10&test=test&test2=test2"));
    }
}
