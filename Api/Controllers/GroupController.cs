namespace FPLMS.Api.Controllers;

using System.Collections.Generic;
using System.Threading.Tasks;
using FPLMS.Api.Dto;
using FPLMS.Api.Filters;
using global::Api.Dto.Request;
using global::Api.Dto.Response;
using global::Api.Dto.Shared;
using global::Api.Services.Groups;
using global::Api.Services.Students;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repositories.Enum;

[ApiController]
[ServiceFilter(typeof(ValidationFilterAttribute))]
[Route("api/management/classes/{classId}/groups")]
public class GroupController : ControllerBase
{
    private readonly IStudentService _studentService;
    private readonly IGroupService _groupService;
    private readonly ILogger<GroupController> _logger;

    public GroupController(IStudentService studentService, IGroupService groupService, ILogger<GroupController> logger)
    {
        _studentService = studentService;
        _groupService = groupService;
        _logger = logger;
    }

    [HttpGet, Authorize(Roles = "Lecturer,Student")]
    public Task<ResponseDto<HashSet<GroupDetailResponseDto>>> GetGroupOfClass(int classId)
    {
        string userEmail = (string)HttpContext.Items["userEmail"]!;
        string userRole = (string)HttpContext.Items["userRole"]!;
        if (userRole == RoleTypes.Lecturer)
            return _groupService.GetGroupOfClassByLecturer(classId, userEmail);
        if (userRole == RoleTypes.Student)
            return _groupService.GetGroupOfClassByStudent(classId, userEmail);
        return Task.FromResult(new ResponseDto<HashSet<GroupDetailResponseDto>> { code = 403, message = "Not have role access" });
    }

    [HttpPost, Authorize(Roles = "Lecturer")]
    public Task<ResponseDto<object>> CreateGroupByLecturer([FromBody] CreateGroupRequestDto createGroupRequest, int classId)
    {

        string userEmail = (string)HttpContext.Items["userEmail"]!;
        createGroupRequest.ClassId = classId;
        return _groupService.CreateGroupRequestByLecturer(createGroupRequest, userEmail);
    }

    [HttpPut, Authorize(Roles = "Lecturer")]
    public Task<ResponseDto<object>> UpdateGroupByLecturer(int classId, [FromBody] GroupDto groupDTO)
    {
        string userEmail = (string)HttpContext.Items["userEmail"]!;
        return _groupService.UpdateGroupByLecturer(classId, groupDTO, userEmail);
    }

    [HttpDelete("{groupId}"), Authorize(Roles = "Lecturer")]
    public Task<ResponseDto<object>> DeleteGroupByLecturer(int groupId, int classId)
    {
        string userEmail = (string)HttpContext.Items["userEmail"]!;
        return _groupService.DeleteGroupByLecturer(groupId, classId, userEmail);
    }

    [HttpPut("{groupId}/disable"), Authorize(Roles = "Lecturer")]
    public Task<ResponseDto<object>> DisableGroupByLecturer(int groupId, int classId)
    {
        string userEmail = (string)HttpContext.Items["userEmail"]!;
        return _groupService.DisableGroupByLecturer(groupId, classId, userEmail);
    }

    [HttpPut("{groupId}/enable"), Authorize(Roles = "Lecturer")]
    public Task<ResponseDto<object>> EnableGroupByLecturer(int groupId, int classId)
    {
        string userEmail = (string)HttpContext.Items["userEmail"]!;
        return _groupService.EnableGroupByLecturer(groupId, classId, userEmail);
    }

    [HttpPost("{groupId}/join"), Authorize(Roles = "Student")]
    public Task<ResponseDto<object>> AddStudentToGroup(int classId, int groupId)
    {
        string userEmail = (string)HttpContext.Items["userEmail"]!;
        int studentId = _studentService.GetStudentIdByEmail(userEmail).Result;
        return _groupService.AddStudentToGroup(classId, groupId, studentId);
    }

    [HttpDelete("leave"), Authorize(Roles = "Student")]
    public Task<ResponseDto<object>> RemoveStudentFromGroup(int classId)
    {
        string userEmail = (string)HttpContext.Items["userEmail"]!;
        int studentId = _studentService.GetStudentIdByEmail(userEmail).Result;
        return _groupService.RemoveStudentFromGroup(classId, studentId);
    }

    [HttpDelete("remove/{removeStudentId}"), Authorize(Roles = "Student")]
    public Task<ResponseDto<object>> removeStudentFromGroupByLeader(int classId, int removeStudentId)
    {
        string userEmail = (string)HttpContext.Items["userEmail"]!;
        int leaderId = _studentService.GetStudentIdByEmail(userEmail).Result;
        return _groupService.RemoveStudentFromGroupByLeader(classId, removeStudentId, leaderId);
    }

    [HttpGet("details"), Authorize(Roles = "Student")]
    public Task<ResponseDto<GroupDetailResponseDto>> getGroupByClassIdAndGroupId(int classId)
    {
        string userEmail = (string)HttpContext.Items["userEmail"]!;
        int studentId = _studentService.GetStudentIdByEmail(userEmail).Result;
        return _groupService.GetGroupByClassId(classId, studentId);
    }

    [HttpPut("changeLeader/{newLeaderId}"), Authorize(Roles = "Student")]
    public Task<ResponseDto<object>> changeGroupLeader(int classId, int newLeaderId)
    {
        string userEmail = (string)HttpContext.Items["userEmail"]!;
        int leaderId = _studentService.GetStudentIdByEmail(userEmail).Result;
        return _groupService.ChangeGroupLeader(classId, leaderId, newLeaderId);
    }

}