using Microsoft.AspNetCore.Diagnostics;
using Q3.Shared.Exceptions;

namespace Q3.API.Middleware
{
    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static void UseCustomExceptionHandler(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var exceptionFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var exception = exceptionFeature?.Error;

                    var statusCode = exception switch
                    {
                        UsernameAlreadyExistsException => StatusCodes.Status409Conflict,
                        KeyNotFoundException => StatusCodes.Status404NotFound,
                        ArgumentException => StatusCodes.Status400BadRequest,
                        UnauthorizedAccessException => StatusCodes.Status403Forbidden,
                        _ => StatusCodes.Status500InternalServerError
                    };

                    var response = new
                    {
                        error = exception?.Message ?? "An unexpected error occurred.",
                        details = env.IsDevelopment() ? exception?.StackTrace : null
                    };

                    context.Response.StatusCode = statusCode;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(response);
                });
            });
        }
    }
}
