using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Playlist_for_party.Exceptions;
using ExceptionContext = Microsoft.AspNetCore.Mvc.Filters.ExceptionContext;
using ViewResult = Microsoft.AspNetCore.Mvc.ViewResult;

namespace Playlist_for_party.Attributes
{
    public class ExceptionFilterAttribute : TypeFilterAttribute
    {
        public ExceptionFilterAttribute() : base(typeof(ExceptionFilterImplementation))
        {
        }

        private class ExceptionFilterImplementation : IAsyncExceptionFilter
        {
            private readonly ILogger<ExceptionFilterImplementation> _logger;

            public ExceptionFilterImplementation(ILogger<ExceptionFilterImplementation> logger)
            {
                _logger = logger;
            }

            public Task OnExceptionAsync(ExceptionContext context)
            {
                var controllerName = context.RouteData.Values["controller"];
                var actionName = context.RouteData.Values["action"];
                if (context.Exception.InnerException is DeserializationException or BadRequestToSpotifyApiException)
                {
                    _logger.LogError("Controller: {}\n" +
                                     "\tAction: {}\n" +
                                     "\tMessage: {}", 
                        controllerName, actionName, context.Exception.Message);
                }
                else
                {
                    _logger.LogError("Unknown exception");
                }
                var res = new ViewResult()
                {
                    StatusCode = 500,
                    ViewName = "Error",
                };
                res.ViewData["dede"] = "efed";
                context.Result = res;
                context.ExceptionHandled = true;
                return Task.CompletedTask;
            }
        }
    }

}