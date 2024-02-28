using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace FoodSystemClient.Services;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly StorageService StorageService;

    public CustomAuthenticationStateProvider(StorageService storageService)
    {
        StorageService = storageService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var user = await StorageService.GetClaimsPrincipal();
        return new AuthenticationState(user);
    }

    public async void MarkUserAsAuthenticated(ClaimsPrincipal user)
    {
        await StorageService.StoreClaimsPrincipal(user);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public async void MarkUsedAsLoggedOut()
    {
        await StorageService.StoreClaimsPrincipal(null);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal())));
    }    
}