namespace FPLMS.Api.Controllers;

using System.Collections.Generic;
using System.Threading.Tasks;
using FPLMS.Api.Dto;
using FPLMS.Api.Filters;
using global::Api.Dto.Shared;
using global::Api.Services.Students;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

[ApiController]
[ServiceFilter(typeof(ValidationFilterAttribute))]
[Route("api/management/students")]
public class StudentController : ControllerBase
{
	private readonly IStudentService _studentService;
	private readonly ILogger<StudentController> _logger;

	public StudentController(IStudentService studentService, ILogger<StudentController> logger)
	{
		_studentService = studentService;
		_logger = logger;
	}

	[HttpGet("{studentId}")]
	public Task<ResponseDto<StudentDto>> GetStudentById(int studentId)
	{
		return _studentService.GetStudentById(studentId);
	}

}