using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using FPLMS.Api.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

public class CustomJwtBearerHandler : JwtBearerHandler
{
    private readonly IAuthService _authService;

    public CustomJwtBearerHandler(IAuthService authService, IOptionsMonitor<JwtBearerOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
        _authService = authService;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        try
        {
            if (!Context.Request.Headers.TryGetValue("Authorization", out var authorizationHeaderValues))
                return await Task.FromResult(AuthenticateResult.Fail("Authorization header not found."));
            var authorizationHeader = authorizationHeaderValues.FirstOrDefault();
            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("bearer "))
                return await Task.FromResult(AuthenticateResult.Fail("Bearer token not found in Authorization header."));

            var token = authorizationHeader.Split(" ").Last();
            string userEmail = _authService.ValidateToken(token);

            var principal = GetClaims(token);
            return await Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name)));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private ClaimsPrincipal GetClaims(string jwt)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadToken(jwt) as JwtSecurityToken;
        var claimsIdentity = new ClaimsIdentity(token.Claims, "token");
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        return claimsPrincipal;
    }
}