using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Playlist_for_party.Models;

namespace Playlist_for_party.Filters.ActionFilters
{
    public class AuthorizationFilterAttribute : TypeFilterAttribute
    {
        private static List<string> Roles { get; set; }

        public AuthorizationFilterAttribute(string roles) : base(typeof(AuthorizationFilterImplementation))
        {
            Roles = roles.Split(",").ToList();
        }

        private class AuthorizationFilterImplementation : Attribute, IAsyncActionFilter
        {
            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (!context.HttpContext.Items.TryGetValue("user-auth", out var authUser))
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                }

                var user = (User)authUser;
                if (user != null && user.Roles.Any(r => Roles.Contains(r)))
                {
                    await next();
                }

                context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
        }
    }
}