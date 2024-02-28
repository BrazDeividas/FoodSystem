using System.Security.Claims;
using System.Text.Json;
using Microsoft.JSInterop;

namespace FoodSystemClient.Services;

public class StorageService
{
    private readonly IJSRuntime _jsRuntime;

    public StorageService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }
    public async Task StoreClaimsPrincipal(ClaimsPrincipal user)
    {
        var serializedUser = JsonSerializer.Serialize(user);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "user", serializedUser);
    }

    public async Task<ClaimsPrincipal> GetClaimsPrincipal()
    {
        var serializedUser = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "user");
        return JsonSerializer.Deserialize<ClaimsPrincipal>(serializedUser) ?? null!;
    }
}