﻿using System.Security.Claims;
using System.Text.Json;

namespace FoodSystemClient.Authentication;

public static class JwtParser
{
    public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();
        var payload = jwt.Split('.')[1];

        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        claims.AddRange(keyValuePairs.Select(keyValuePair => new Claim(keyValuePair.Key, keyValuePair.Value.ToString())));

        return claims;
    }

    public static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch(base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }
}
