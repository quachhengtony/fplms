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
    private readonly ILogger<SubjectController> _logger;

    public SubjectController(ISubjectService subjectService, ILogger<SubjectController> logger)
    {
        _subjectService = subjectService;
        _logger = logger;
    }

	[HttpGet, Authorize(Roles = "Lecturer, Student")]
	public Task<ResponseDto<HashSet<SubjectDto>>> GetSubjects()
	{
		return _subjectService.GetSubjects();
	}

	
	[HttpPost, Authorize(Roles = "Lecturer")]
	public Task<ResponseDto<object>> CreateSubject([FromBody] SubjectDto subjectDto)
	{
		return _subjectService.CreateSubject(subjectDto);
	}

	[HttpPut, Authorize(Roles = "Lecturer")]
	public Task<ResponseDto<object>> UpdateSubject([FromBody] SubjectDto subjectDto)
	{
		return _subjectService.UpdateSubject(subjectDto);
	}
	
	[HttpDelete("{subjectId}"), Authorize(Roles = "Lecturer")]
	public Task<ResponseDto<object>> DeleteSubject(int subjectId)
	{
		return _subjectService.DeleteSubject(subjectId);
	}

	[HttpGet("isStudied"), Authorize(Roles = "Student")]
	public IActionResult IsStudentStudySubject([FromQuery] string subjectName) {
		string userEmail = (string)HttpContext.Items["userEmail"]!;
		int status = _subjectService.IsStudentStudySubject(userEmail, subjectName).Result;

		return StatusCode(status);
	}
}