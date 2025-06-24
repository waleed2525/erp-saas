using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public class JwtAuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _storage;
    private readonly IHttpClientFactory _httpFactory;
    public JwtAuthStateProvider(ILocalStorageService storage, IHttpClientFactory httpFactory)
    {
        _storage = storage; _httpFactory = httpFactory;
    }
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _storage.GetItemAsStringAsync("token");
        if (string.IsNullOrWhiteSpace(token))
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        if (jwt.ValidTo < DateTime.UtcNow)
        {
            // جرّب refresh
            var refresh = await _storage.GetItemAsStringAsync("refreshToken");
            if (!await TryRefresh(refresh)) // لم ينجح التحديث
            {
                await _storage.RemoveItemsAsync(new[] { "token", "refreshToken" });
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
            token = await _storage.GetItemAsStringAsync("token");
            jwt = handler.ReadJwtToken(token);
        }

        var identity = new ClaimsIdentity(jwt.Claims, "jwt");
        var user = new ClaimsPrincipal(identity);
        return new AuthenticationState(user);
    }

    private async Task<bool> TryRefresh(string refresh)
    {
        if (string.IsNullOrWhiteSpace(refresh)) return false;
        var client = _httpFactory.CreateClient("ERPApi");
        var res = await client.PostAsJsonAsync("/auth/refresh", new { token = refresh });
        if (!res.IsSuccessStatusCode) return false;
        var json = await res.Content.ReadFromJsonAsync<AuthResponse>();
        await _storage.SetItemAsStringAsync("token", json!.token);
        await _storage.SetItemAsStringAsync("refreshToken", json.refreshToken);
        return true;
    }

    private record AuthResponse(string token, string refreshToken);
}
