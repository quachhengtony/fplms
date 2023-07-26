namespace FPLMS.Api.Controllers;

using System.Linq;
using System.Threading.Tasks;
using FPLMS.Api.Dto;
using FPLMS.Api.Enum;
using FPLMS.Api.Filters;
using FPLMS.Api.Models;
using FPLMS.Api.Services;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;
    private string adminEmail = "huongntc2.fe@gmail.com";

    public AuthController(IConfiguration config, IAuthService authService, ILogger<AuthController> logger)
    {
        _config = config;
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("login")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        try
        {
            if (!ModelState.IsValid) return UnprocessableEntity(ModelState);
            var tokenPayload = await _authService.ValidateGoogleToken(loginRequestDto);
            if (!tokenPayload.Email.Contains(_config["EmailFormat:Student"]) && !tokenPayload.Email.Contains(_config["EmailFormat:Lecturer"]))
            {
                return BadRequest("Invalid credentials.");
            }

            string role;
            if (tokenPayload.Email.Contains(_config["EmailFormat:Student"]))
                role = RoleTypes.Student;
            else
                role = RoleTypes.Lecturer;
            if (tokenPayload.Email.Equals(adminEmail))
                role = RoleTypes.Admin;
            var token = _authService.CreateToken(tokenPayload, role);

            return Ok(new LoginResponseDto
            {
                ErrorMessage = "",
                IsAuthSuccessful = true,
                Token = token
            });
        }
        catch (InvalidJwtException)
        {
            return BadRequest(new ErrorBase
            {
                Message = "Bad credentials.",
                StatusCode = StatusCodes.Status400BadRequest
            });
        }
    }

    [HttpGet]
    public IActionResult CheckAdminRole()
    {
        if (HttpContext.Request.Headers.TryGetValue("userEmail", out var userEmail)
            && userEmail.Equals(adminEmail))
        {
            return Ok(true);
        }
        else
        {
            return Ok(false);
        }
    }
}