using System.Threading.Tasks;
using FPLMS.Api.Dto;
using Google.Apis.Auth;

namespace FPLMS.Api.Services;

public interface IAuthService
{
    Task<GoogleJsonWebSignature.Payload> ValidateGoogleToken(LoginRequestDto loginRequestDto);
    string CreateToken(GoogleJsonWebSignature.Payload payload, string roleClaim);
}