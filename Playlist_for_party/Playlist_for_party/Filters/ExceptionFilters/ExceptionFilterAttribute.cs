using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Playlist_for_party.Exceptions.AppExceptions;
using ExceptionContext = Microsoft.AspNetCore.Mvc.Filters.ExceptionContext;
using ViewResult = Microsoft.AspNetCore.Mvc.ViewResult;

namespace Playlist_for_party.Filters.ExceptionFilters
{
    public class ExceptionFilterAttribute : TypeFilterAttribute
    {
        public ExceptionFilterAttribute() : base(typeof(ExceptionFilterImplementation))
        {
        }

        private class ExceptionFilterImplementation : IAsyncExceptionFilter
        {
            private readonly ILogger<ExceptionFilterImplementation> _logger;

            private readonly List<Type> _exceptions = new List<Type>()
            {
                typeof(DeserializationOfSpotifyModelException),
                typeof(BadRequestToSpotifyApiException),
                typeof(UnauthorizedException),
                typeof(InvalidTokensProvided)
            };

            public ExceptionFilterImplementation(ILogger<ExceptionFilterImplementation> logger)
            {
                _logger = logger;
            }

            public Task OnExceptionAsync(ExceptionContext context)
            {
                var controllerName = context.RouteData.Values["controller"];
                var actionName = context.RouteData.Values["action"];
                var contextType = context.Exception.InnerException?.GetType();

                if (!_exceptions.Contains(contextType))
                {
                    _logger.LogError("Unknown exception\n");
                }

                _logger.LogError("\tController: {}\n" +
                                 "\tAction: {}\n" +
                                 "\tExceptionName: {}\n" +
                                 "\tMessage: {}",
                    controllerName, actionName, contextType?.Name, context.Exception.Message);

                var res = new ViewResult()
                {
                    StatusCode = 500,
                    ViewName = "Error",
                };
                context.Result = res;
                context.ExceptionHandled = true;
                return Task.CompletedTask;
            }
        }
    }
}