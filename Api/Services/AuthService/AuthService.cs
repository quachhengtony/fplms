namespace FPLMS.Api.Services;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using FPLMS.Api.Dto;
using FPLMS.Api.Models;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

sealed class AuthService : IAuthService
{
    private readonly IConfiguration _config;

    public AuthService(IConfiguration config)
    {
        _config = config;
    }

    public async Task<GoogleJsonWebSignature.Payload> ValidateGoogleToken(LoginRequestDto loginRequestDto)
    {
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string>() { _config["Google:ClientId"] }
            };

            return await GoogleJsonWebSignature.ValidateAsync(loginRequestDto.IdToken, settings);
        }
        catch (InvalidJwtException ex)
        {
            throw ex;
        }
    }

    public string CreateToken(GoogleJsonWebSignature.Payload payload, string roleClaim)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, payload.Email),
            new Claim(ClaimTypes.Role, roleClaim)
        };
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config["Token:Secret"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddMinutes(30), signingCredentials: credentials));
        return token;
    }
}