namespace FPLMS.Api.Controllers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FPLMS.Api.Dto;
using FPLMS.Api.Filters;
using global::Api.Dto.Shared;
using global::Api.Dto.Temp;
using global::Api.Services.Students;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Repositories.Contracts;

[ApiController]
[ServiceFilter(typeof(ValidationFilterAttribute))]
[Route("api/management/students")]
public class StudentController : ControllerBase
{
	private readonly IStudentService _studentService;
	private readonly ILogger<StudentController> _logger;
    private IRepositoryWrapper _repositoryWrapper;
    private IMapper _mapper;

    public StudentController(IStudentService studentService, ILogger<StudentController> logger, IRepositoryWrapper repositoryWrapper, IMapper mapper)
	{
		_studentService = studentService;
		_logger = logger;
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
	}

	[HttpGet("{studentId}"), Authorize(Roles = "Student")]
	public Task<ResponseDto<StudentDto>> GetStudentById(int studentId)
	{
		return _studentService.GetStudentById(studentId);
	}

    [HttpGet("questions")]
    [Authorize]
    //[TypeFilter(typeof(AuthorizationFilterAttribute))]
    //[ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> GetStudentQuestions()
    {
        try
        {
            var userEmail = HttpContext.Items["userEmail"] as string;
            var userRole = HttpContext.Items["userRole"] as string;

            if (!userRole.Equals("Student"))
            {
                return Unauthorized("Only students can get questions.");
            }

            var student = await _repositoryWrapper.StudentRepository.GetStudentByEmailAsync(userEmail);
            var questions = await _repositoryWrapper.QuestionRepository.GetQuestionsByStudentId(student.Id);
            if (!student.Id.Equals(questions.First().StudentId))
            {
                return Forbid("Only the author of the questions can get their questions");
            }

            var result = _mapper.Map<List<GetQuestionDto>>(questions);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("answers")]
    [Authorize]
    //[TypeFilter(typeof(AuthorizationFilterAttribute))]
    //[ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> GetStudentAnswers()
    {
        try
        {
            var userEmail = HttpContext.Items["userEmail"] as string;
            var userRole = HttpContext.Items["userRole"] as string;

            if (!userRole.Equals("Student"))
            {
                return Forbid("Only students can get answers.");
            }
            var student = await _repositoryWrapper.StudentRepository.GetStudentByEmailAsync(userEmail);
            var answers = await _repositoryWrapper.AnswerRepository.GetAnswersByStudentId(student.Id);
            if (!student.Id.Equals(answers.First().StudentId))
            {
                return Forbid("Only the author of the answers can get their answers");
            }
            var result = _mapper.Map<List<GetAnswerDto>>(answers);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error");
        }
    }

}