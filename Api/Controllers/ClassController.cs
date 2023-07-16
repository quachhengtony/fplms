namespace FPLMS.Api.Controllers;

using System.Collections.Generic;
using System.Threading.Tasks;
using FPLMS.Api.Dto;
using FPLMS.Api.Filters;
using FPLMS.Api.Services;
using global::Api.Dto.Response;
using global::Api.Dto.Shared;
using global::Api.Services.Classes;
using global::Api.Services.Students;
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
    private readonly IClassService _classService;
    private readonly IStudentService _studentService;

    private readonly ILogger<ClassController> _logger;
    public ClassController(IClassService classService, IStudentService studentService, ILogger<ClassController> logger)
    {
        _classService = classService;
        _studentService = studentService;
        _logger = logger;
    }

    [HttpPost, Authorize(Roles = "Lecturer")]
    public Task<ResponseDto<int>> CreateClassByLecturer([FromBody] ClassDto classDTO, [FromQuery] string userEmail) {
        string userEmailStr = (string)HttpContext.Items["userEmail"]!;
        string userRole = (string)HttpContext.Items["userRole"]!;
        return _classService.CreateClassByLecturer(classDTO, userEmailStr);
    }

    [HttpPut]
    public Task<ResponseDto<object>> UpdateClassByLecturer([FromBody] ClassDto classDTO, [FromQuery] string userEmail) {

        return _classService.UpdateClassByLecturer(classDTO, userEmail);
    }

    [HttpDelete("{classId}")]
    public Task<ResponseDto<object>> DeleteClassByLecturer(int classId, [FromQuery] string userEmail) {
        return _classService.DeleteClassByLecturer(classId, userEmail);
    }

    [HttpGet]
    public Task<ResponseDto<HashSet<ClassDto>>> GetClassOfLecturer([FromQuery] string userEmail) {
        return _classService.GetClassOfLecture(userEmail);
    }

    [HttpGet("{classId}")]
    public Task<ResponseDto<ClassDto>> GetClassDetail(int classId, [FromQuery] string userEmail, [FromQuery] string userRole) {
    	if (userRole == "LECTURER") {
			return _classService.GetClassDetailByLecture(userEmail, classId);
		}
		if (userRole == "STUDENT") {
			return _classService.GetClassDetailByStudent(userEmail, classId);
		}
        return Task.FromResult(new ResponseDto<ClassDto> { code = 403, message = "Not have role access" });
    }

    [HttpGet("{id}/students")]
    public Task<ResponseDto<HashSet<StudentInClassResponseDto>>> GetStudentInClassByLecturer(int id, [FromQuery] string userEmail) {
        return _classService.GetStudentInClassByLecturer(id, userEmail);
    }

    [HttpDelete("{classId}/students/{studentId}")]
    public Task<ResponseDto<object>> RemoveStudentInClassByLecturer(int classId, int studentId, [FromQuery] string userEmail) {
        return _classService.RemoveStudentInClassByLecturer(studentId, classId, userEmail);
    }

    [HttpPut("{classId}/students/{studentId}/groups/{groupNumber}")]
    public Task<ResponseDto<object>> ChangeStudentGroupByLecturer(int classId, int studentId, int groupNumber, [FromQuery] string userEmail) {
        return _classService.ChangeStudentGroupByLecturer(studentId, classId, groupNumber, userEmail);
    }

    [HttpPost("{classId}/enroll")]
    public Task<ResponseDto<object>> EnrollStudentToClass([FromQuery] string userEmail, int classId, [FromQuery] string enrollKey) {
        int studentId = _studentService.GetStudentIdByEmail(userEmail).Result;
        return _classService.EnrollStudentToClass(classId, studentId, enrollKey);
    }

    [HttpGet("student")]
    public Task<ResponseDto<HashSet<ClassByStudentResponseDto>>> GetClassesBySearchStrByStudent([FromQuery] string userEmail, [FromQuery] string? search) {
        int studentId = _studentService.GetStudentIdByEmail(userEmail).Result;
        return _classService.GetClassesBySearchStrByStudent(search, studentId);
    }

    [HttpDelete("{classId}/unenroll")]
    public Task<ResponseDto<object>> UnenrollStudentFromClass([FromQuery] string userEmail, int classId) {
        int studentId = _studentService.GetStudentIdByEmail(userEmail).Result;
        return _classService.UnenrollStudentInClass(studentId, classId);
    }

}