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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

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

	[HttpGet]
	public Task<ResponseDto<HashSet<GroupDetailResponseDto>>> GetGroupOfClass(int classId, [FromQuery] string userEmail, [FromQuery] string userRole)
	{
		if (userRole == "LECTURER")
			return _groupService.GetGroupOfClassByLecturer(classId, userEmail);
		if (userRole == "STUDENT")
			return _groupService.GetGroupOfClassByStudent(classId, userEmail);
		return Task.FromResult(new ResponseDto<HashSet<GroupDetailResponseDto>> { code = 403, message = "Not have role access" });
	}

	[HttpPost]
	public Task<ResponseDto<object>> CreateGroupByLecturer([FromBody] CreateGroupRequestDto createGroupRequest, int classId, string userEmail) {

		createGroupRequest.ClassId = classId;
        return _groupService.CreateGroupRequestByLecturer(createGroupRequest, userEmail);
    }

	[HttpPut]
	public Task<ResponseDto<object>> UpdateGroupByLecturer(int classId, [FromBody] GroupDto groupDTO, [FromQuery] string userEmail) {
        return _groupService.UpdateGroupByLecturer(classId, groupDTO, userEmail);
    }

	[HttpDelete("{groupId}")]
	public Task<ResponseDto<object>> DeleteGroupByLecturer(int groupId, int classId, [FromQuery] string userEmail) {
        return _groupService.DeleteGroupByLecturer(groupId, classId, userEmail);
    }
	
	[HttpPut("{groupId}/disable")]
	public Task<ResponseDto<object>> DisableGroupByLecturer(int groupId, int classId, [FromQuery] string userEmail) {
        return _groupService.DisableGroupByLecturer(groupId, classId, userEmail);
    }

	[HttpPut("{groupId}/enable")]
	public Task<ResponseDto<object>> EnableGroupByLecturer(int groupId, int classId, [FromQuery] string userEmail) {
        return _groupService.EnableGroupByLecturer(groupId, classId, userEmail);
    }

	[HttpPost("{groupId}/join")]
	public Task<ResponseDto<object>> AddStudentToGroup([FromQuery] string userEmail, int classId, int groupId) {
		int studentId = _studentService.GetStudentIdByEmail(userEmail).Result;
        return _groupService.AddStudentToGroup(classId, groupId, studentId);
    }

	[HttpDelete("leave")]
	public Task<ResponseDto<object>> RemoveStudentFromGroup(string userEmail, int classId) {
		int studentId = _studentService.GetStudentIdByEmail(userEmail).Result;
        return _groupService.RemoveStudentFromGroup(classId, studentId);
    }

	[HttpDelete("remove/{removeStudentId}")]
	public Task<ResponseDto<object>> removeStudentFromGroupByLeader([FromQuery] string userEmail, int classId, int removeStudentId) {

		int leaderId = _studentService.GetStudentIdByEmail(userEmail).Result;
        return _groupService.RemoveStudentFromGroupByLeader(classId, removeStudentId, leaderId);
    }

	[HttpGet("details")]
	public Task<ResponseDto<GroupDetailResponseDto>> getGroupByClassIdAndGroupId([FromQuery] string userEmail, int classId) {

		int studentId = _studentService.GetStudentIdByEmail(userEmail).Result;
		return _groupService.GetGroupByClassId(classId, studentId);
    }

	[HttpPut("changeLeader/{newLeaderId}")]
	public Task<ResponseDto<object>> changeGroupLeader([FromQuery] string userEmail, int classId, int newLeaderId) {
		int leaderId = _studentService.GetStudentIdByEmail(userEmail).Result;
		return _groupService.ChangeGroupLeader(classId, leaderId, newLeaderId);
    }

}