namespace FPLMS.Api.Services;

using System.Threading.Tasks;
using FPLMS.Api.Dto;
using Google.Apis.Auth;

public interface IAuthService
{
    Task<GoogleJsonWebSignature.Payload> ValidateGoogleToken(LoginRequestDto loginRequestDto);
    string CreateToken(GoogleJsonWebSignature.Payload payload, string roleClaim);
    string ValidateToken(string token);
}