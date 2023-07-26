namespace FPLMS.Api.Controllers;

using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BusinessObjects.Models;
using FPLMS.Api.Dto;
using FPLMS.Api.Filters;
using FPLMS.Api.Models;
using FPLMS.Api.Services;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Repositories.Enum;
using Repositories.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    private readonly IStudentRepository _studentRepository;
    private readonly ILecturerRepository _lecturerRepository;
    private string adminEmail = "";

    public AuthController(IConfiguration config, IAuthService authService, ILogger<AuthController> logger, IStudentRepository studentRepository, ILecturerRepository lecturerRepository)
    {
        _config = config;
        _authService = authService;
        _logger = logger;
        _studentRepository = studentRepository;
        _lecturerRepository = lecturerRepository;
    }

    private string ExtractStudentCode(string input)
    {
        // Use a regular expression to find the pattern of two characters followed by numbers
        Regex regex = new Regex(@"[A-Za-z]{2}\d+");
        Match match = regex.Match(input);

        if (match.Success)
        {
            return match.Value;
        }

        // If no match is found, return an empty string or handle it as you see fit.
        return string.Empty;
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

            int userId;
            bool userExists;
            string role;
            // if (tokenPayload.Email.Contains(_config["EmailFormat:Student"]))
            //     role = RoleTypes.Student;
            // else
            //     role = RoleTypes.Lecturer;


            if (tokenPayload.Email.Contains(_config["EmailFormat:Student"]))
            {
                userExists = await _studentRepository.ExistsByEmail(tokenPayload.Email);
                if (!userExists)
                {
                    var student = new Student
                    {
                        Code = ExtractStudentCode(tokenPayload.Email),
                        Email = tokenPayload.Email,
                        ImageUrl = tokenPayload.Picture
                    };
                    _studentRepository.Create(student);
                    await _studentRepository.SaveChanges();
                }
                role = RoleTypes.Student;

            }
            else
            {
                userId = await _lecturerRepository.ExistsByEmailAsync(tokenPayload.Email);
                if (userId == null)
                {
                    var lecturer = new Lecturer
                    {
                        ImageUrl = tokenPayload.Picture,
                        Email = tokenPayload.Email,
                    };
                    _lecturerRepository.Create(lecturer);
                    await _lecturerRepository.SaveChanges();
                }
                role = RoleTypes.Lecturer;
            }

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