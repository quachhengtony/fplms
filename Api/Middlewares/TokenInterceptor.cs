using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FPLMS.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

public class TokenInterceptor
{
    private readonly RequestDelegate _next;

    public TokenInterceptor(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IAuthService authService, IConfiguration config)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token != null) AttachUserToContext(context, authService, config, token);
        await _next(context);
    }

    private void AttachUserToContext(HttpContext context, IAuthService authService, IConfiguration config, string token)
    {
        try
        {
            var (userEmail, userRole) = ValidateToken(token, config);

            context.Items["userEmail"] = userEmail;
            context.Items["userRole"] = userRole;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    // private string ValidateToken(string token, IConfiguration config)
    // {
    //     var tokenHandler = new JwtSecurityTokenHandler();
    //     var key = Encoding.ASCII.GetBytes(config["Token:Secret"]);
    //     try
    //     {
    //         tokenHandler.ValidateToken(token, new TokenValidationParameters
    //         {
    //             ValidateIssuerSigningKey = true,
    //             IssuerSigningKey = new SymmetricSecurityKey(key),
    //             ValidateIssuer = false,
    //             ValidateAudience = false,
    //             ClockSkew = TimeSpan.Zero
    //         }, out SecurityToken validatedToken);

    //         var jwtToken = (JwtSecurityToken)validatedToken;
    //         var userEmail = jwtToken.Claims.First(x => x.Type == "email").Value;
    //         // var userRole = jwtToken.Claims.First(x => x.Type == "role").Value;

    //         return userEmail;
    //     }
    //     catch (Exception ex)
    //     {
    //         throw ex;
    //     }
    // }

    private (string, string) ValidateToken(string token, IConfiguration config)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(config["Token:Secret"]);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userEmail = jwtToken.Claims.First(x => x.Type == ClaimTypes.Email).Value;
            var userRole = jwtToken.Claims.First(x => x.Type == ClaimTypes.Role).Value;

            return (userEmail, userRole);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}