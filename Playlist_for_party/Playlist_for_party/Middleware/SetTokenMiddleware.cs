using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Playlist_for_party.Middleware
{
    public class SetTokenMiddleware : IMiddleware
    {
        private readonly ILogger<SetTokenMiddleware> _logger;
        public SetTokenMiddleware(ILogger<SetTokenMiddleware> logger)
        {
            _logger = logger;
        }
        
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var jwToken = context.Session.GetString("Authorization");
            if (!string.IsNullOrEmpty(jwToken))
            {
                context.Request.Headers.Add("Authorization", jwToken);
                _logger.LogInformation("Set token: {JwToken}", jwToken);
            }
            
            await next(context);
        }
    }
}