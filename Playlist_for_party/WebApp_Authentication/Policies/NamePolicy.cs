using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace WebApp_Authentication.Policies
{
    public static class NamePolicy
    {
        public static string Name = "ales-policy";

        public static AuthorizationPolicy Requirements => new(
            new[]
            {
                new ClaimsAuthorizationRequirement(ClaimTypes.Name, new[] { "ales" })
            },
            new[] { JwtBearerDefaults.AuthenticationScheme }
        );
    }
}