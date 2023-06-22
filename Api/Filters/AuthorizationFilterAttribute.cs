namespace FPLMS.Api.Filters;

using System;
using System.Linq;
using FPLMS.Api.Models;
using FPLMS.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizationFilterAttribute : Attribute, IAuthorizationFilter
{
    private readonly IAuthService _authService;
    public AuthorizationFilterAttribute(IAuthService authService)
    {
        _authService = authService;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        string? userEmail = (string?)context.HttpContext.Items["UserEmail"];
        if (userEmail is null)
        {
            context.Result = new JsonResult(new ErrorBase
            {
                Message = "Request terminated. Unauthorized access to protected resource.",
                StatusCode = StatusCodes.Status401Unauthorized
            });
        }
    }
}