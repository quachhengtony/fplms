namespace FPLMS.Api.Middlewares;

using System;
using System.Net;
using System.Threading.Tasks;
using FPLMS.Api.Models;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);

            switch (httpContext.Response.StatusCode)
            {
                case ((int)HttpStatusCode.Unauthorized):
                    httpContext.Response.ContentType = "application/json";
                    await httpContext.Response.WriteAsJsonAsync(new ErrorBase { Message = "Unauthorized: Access is denied due to invalid credentials or authentication failure.", StatusCode = (int)HttpStatusCode.Unauthorized });
                    break;
                case ((int)HttpStatusCode.Forbidden):
                    httpContext.Response.ContentType = "application/json";
                    await httpContext.Response.WriteAsJsonAsync(new ErrorBase { Message = "Forbidden: Access is denied due to insufficient privileges.", StatusCode = (int)HttpStatusCode.Forbidden });
                    break;
                default:
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong: {ex}");
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        string message = exception switch
        {
            SecurityTokenException => "Bad token.",
            _ => "Internal server error."
        };

        await httpContext.Response.WriteAsync(new ErrorBase
        {
            StatusCode = httpContext.Response.StatusCode,
            Message = message
        }.ToString());
    }
}