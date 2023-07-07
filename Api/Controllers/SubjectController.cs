namespace FPLMS.Api.Controllers;

using System.Collections.Generic;
using System.Threading.Tasks;
using FPLMS.Api.Dto;
using FPLMS.Api.Filters;
using global::Api.Dto.Shared;
using global::Api.Services.Subjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

[ApiController]
[ServiceFilter(typeof(ValidationFilterAttribute))]
[Route("api/management/subjects")]
public class SubjectController : ControllerBase
{
    private readonly ISubjectService _subjectService;
    private readonly ILogger<ClassController> _logger;

    public SubjectController(ISubjectService subjectService, ILogger<ClassController> logger)
    {
        _subjectService = subjectService;
        _logger = logger;
    }
	/*
    [HttpPost, Authorize(Roles = "Lecturer")]
    public async Task<ActionResult<ResponseDto<int>>> CreateClassByLecturer([FromBody] ClassRequestResponseDto classDto)
    {
        string userEmail = (string)HttpContext.Items["userEmail"];
        var responseDto = new ResponseDto<int>
        {
            code = StatusCodes.Status200OK,
            data = 1,
            message = userEmail ?? "none"
        };
        return Ok(responseDto);
    }*/

	[HttpGet]
	public Task<ResponseDto<HashSet<SubjectDto>>> GetSubjects()
	{
		return _subjectService.GetSubjects();
	}

	
	[HttpPost]
	public Task<ResponseDto<object>> CreateSubject([FromBody] SubjectDto subjectDto)
	{
		return _subjectService.CreateSubject(subjectDto);
	}

	[HttpPut]
	public Task<ResponseDto<object>> UpdateSubject([FromBody] SubjectDto subjectDto)
	{
		return _subjectService.UpdateSubject(subjectDto);
	}
	
	[HttpDelete("{subjectId}")]
	public Task<ResponseDto<object>> DeleteSubject(int subjectId)
	{
		return _subjectService.DeleteSubject(subjectId);
	}

	[HttpGet("isStudied")]
	public IActionResult IsStudentStudySubject([FromQuery] string subjectName,
			[FromQuery] string userEmail) {
		int status = _subjectService.IsStudentStudySubject(userEmail, subjectName).Result;

		return StatusCode(status);
	}
}