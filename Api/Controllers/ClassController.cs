namespace FPLMS.Api.Controllers;

using System.Collections.Generic;
using System.Threading.Tasks;
using FPLMS.Api.Dto;
using FPLMS.Api.Filters;
using global::Api.Dto.Response;
using global::Api.Dto.Shared;
using global::Api.Services.Classes;
using global::Api.Services.Students;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repositories.Enum;

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
    public Task<ResponseDto<int>> CreateClassByLecturer([FromBody] ClassDto classDTO)
    {
        string userEmail = (string)HttpContext.Items["userEmail"]!;
        string userRole = (string)HttpContext.Items["userRole"]!;
        return _classService.CreateClassByLecturer(classDTO, userEmail);
    }

    [HttpPut, Authorize(Roles = "Lecturer")]
    public Task<ResponseDto<object>> UpdateClassByLecturer([FromBody] ClassDto classDTO)
    {
        string userEmail = (string)HttpContext.Items["userEmail"]!;
        return _classService.UpdateClassByLecturer(classDTO, userEmail);
    }

    [HttpDelete("{classId}"), Authorize(Roles = "Lecturer")]
    public Task<ResponseDto<object>> DeleteClassByLecturer(int classId)
    {
        string userEmail = (string)HttpContext.Items["userEmail"]!;
        return _classService.DeleteClassByLecturer(classId, userEmail);
    }

    [HttpGet, Authorize(Roles = "Lecturer")]
    public Task<ResponseDto<HashSet<ClassDto>>> GetClassOfLecturer()
    {
        string userEmail = (string)HttpContext.Items["userEmail"]!;
        return _classService.GetClassOfLecture(userEmail);
    }

    [HttpGet("{classId}"), Authorize(Roles = "Lecturer,Student")]
    public Task<ResponseDto<ClassDto>> GetClassDetail(int classId)
    {
        string userEmail = (string)HttpContext.Items["userEmail"]!;
        string userRole = (string)HttpContext.Items["userRole"]!;
        if (userRole == RoleTypes.Lecturer)
        {
            return _classService.GetClassDetailByLecture(userEmail, classId);
        }
        if (userRole == RoleTypes.Student)
        {
            return _classService.GetClassDetailByStudent(userEmail, classId);
        }
        return Task.FromResult(new ResponseDto<ClassDto> { code = 403, message = "Not have role access" });
    }

    [HttpGet("{id}/students"), Authorize(Roles = "Lecturer")]
    public Task<ResponseDto<HashSet<StudentInClassResponseDto>>> GetStudentInClassByLecturer(int id)
    {
        string userEmail = (string)HttpContext.Items["userEmail"]!;
        return _classService.GetStudentInClassByLecturer(id, userEmail);
    }

    [HttpDelete("{classId}/students/{studentId}"), Authorize(Roles = "Lecturer")]
    public Task<ResponseDto<object>> RemoveStudentInClassByLecturer(int classId, int studentId)
    {
        string userEmail = (string)HttpContext.Items["userEmail"]!;
        return _classService.RemoveStudentInClassByLecturer(studentId, classId, userEmail);
    }

    [HttpPut("{classId}/students/{studentId}/groups/{groupNumber}"), Authorize(Roles = "Lecturer")]
    public Task<ResponseDto<object>> ChangeStudentGroupByLecturer(int classId, int studentId, int groupNumber)
    {
        string userEmail = (string)HttpContext.Items["userEmail"]!;
        return _classService.ChangeStudentGroupByLecturer(studentId, classId, groupNumber, userEmail);
    }

    [HttpPost("{classId}/enroll"), Authorize(Roles = "Student")]
    public Task<ResponseDto<object>> EnrollStudentToClass(int classId, [FromBody] string enrollKey)
    {
        string userEmail = (string)HttpContext.Items["userEmail"]!;
        int studentId = _studentService.GetStudentIdByEmail(userEmail).Result;
        return _classService.EnrollStudentToClass(classId, studentId, enrollKey);
    }

    [HttpGet("student"), Authorize(Roles = "Student")]
    public Task<ResponseDto<HashSet<ClassByStudentResponseDto>>> GetClassesBySearchStrByStudent([FromQuery] string? search)
    {
        string userEmail = (string)HttpContext.Items["userEmail"]!;
        int studentId = _studentService.GetStudentIdByEmail(userEmail).Result;
        return _classService.GetClassesBySearchStrByStudent(search, studentId);
    }

    [HttpDelete("{classId}/unenroll"), Authorize(Roles = "Student")]
    public Task<ResponseDto<object>> UnenrollStudentFromClass(int classId)
    {
        string userEmail = (string)HttpContext.Items["userEmail"]!;
        int studentId = _studentService.GetStudentIdByEmail(userEmail).Result;
        return _classService.UnenrollStudentInClass(studentId, classId);
    }

}