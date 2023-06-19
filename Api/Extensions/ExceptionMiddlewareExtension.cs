namespace FPLMS.Api.Extensions;

using FPLMS.Api.Middlewares;
using Microsoft.AspNetCore.Builder;

public static class ExceptionMiddlewareExtension
{
    public static void ConfigureExceptionMiddleware(this WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
    }
}