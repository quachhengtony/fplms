namespace FPLMS.Api.Controllers;

using System.Threading.Tasks;
using FPLMS.Api.Dto;
using FPLMS.Api.Filters;
using FPLMS.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

[ApiController]
[ServiceFilter(typeof(ValidationFilterAttribute))]
[Route("api/management/classes")]
public class ClassController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly IClassService _classService;
    private readonly ILogger<ClassController> _logger;
    public ClassController(IConfiguration config, IClassService classService, ILogger<ClassController> logger)
    {
        _config = config;
        _classService = classService;
        _logger = logger;
    }

    [HttpPost, Authorize(Roles = "Lecturer")]
    public async Task<ActionResult<ResponseDto<int>>> CreateClassByLecturer([FromBody] ClassRequestResponseDto classDto)
    {
        string userEmail = (string)HttpContext.Items["userEmail"]!;
        string userRole = (string)HttpContext.Items["userRole"]!;

        _logger.LogInformation(userEmail);
        _logger.LogInformation(userRole);

        var responseDto = new ResponseDto<int>
        {
            code = StatusCodes.Status200OK,
            data = 1,
            message = userEmail ?? "none"
        };
        return Ok(responseDto);
    }
}