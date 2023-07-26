using Api.Dto.Shared;
using Api.Services.Meetings;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using FPLMS.Api.Dto;
using Microsoft.AspNetCore.Authorization;
using Repositories.Enum;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/management/meetings")]
    public class MeetingController : ControllerBase
    {
        private readonly IMeetingService _meetingService;

        public MeetingController(IMeetingService meetingService)
        {
            _meetingService = meetingService;
        }
        [HttpGet, Authorize(Roles = "Lecturer,Student")]
        public async Task<ActionResult<ResponseDto<HashSet<MeetingDto>>>> GetMeetingInGroup([FromQuery] int? classId, [FromQuery] int? groupId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var userEmail = (string)HttpContext.Items["userEmail"]!;
            var userRole = (string)HttpContext.Items["userRole"]!;
            if (userRole.Contains(RoleTypes.Lecturer))
            {
                return await _meetingService.GetMeetingInGroupByLecturerAsync(classId, groupId, new DateTimeOffset(startDate.Value), new DateTimeOffset(endDate.Value), userEmail);
            }
            if (userRole.Contains(RoleTypes.Student))
            {
                return await _meetingService.GetMeetingInGroupByStudentAsync(classId, groupId, new DateTimeOffset(startDate.Value), new DateTimeOffset(endDate.Value), userEmail);
            }
            return StatusCode(403, "Not have role access");
        }

        [HttpGet("{meetingId}"), Authorize(Roles = "Lecturer,Student")]
        public async Task<ActionResult<ResponseDto<MeetingDto>>> GetMeetingById(int meetingId)
        {
            var userEmail = (string)HttpContext.Items["userEmail"]!;
            var userRole = (string)HttpContext.Items["userRole"]!;
            if (userRole.Contains(RoleTypes.Lecturer))
            {
                return await _meetingService.GetMeetingDetailByLecturerAsync(userEmail, meetingId);
            }
            if (userRole.Contains(RoleTypes.Student))
            {
                return await _meetingService.GetMeetingDetailByStudentAsync(userEmail, meetingId);
            }
            return StatusCode(403, "Not have role access");
        }

        [HttpPost, Authorize(Roles = "Lecturer")]
        public async Task<ActionResult<ResponseDto<MeetingDto>>> ScheduleMeetingByLecturer(MeetingDto meetingDto)
        {
            var userEmail = (string)HttpContext.Items["userEmail"]!;
            var userRole = (string)HttpContext.Items["userRole"]!;
            return await _meetingService.ScheduleMeetingByLecturerAsync(meetingDto, userEmail);
        }

        [HttpPut, Authorize(Roles = "Lecturer")]
        public async Task<ActionResult<ResponseDto<object>>> UpdateMeetingByLecturer(MeetingDto meetingDto)
        {
            var userEmail = (string)HttpContext.Items["userEmail"]!;
            var userRole = (string)HttpContext.Items["userRole"]!;
            return await _meetingService.UpdateMeetingByLecturerAsync(meetingDto, userEmail);
        }

        [HttpDelete, Authorize(Roles = "Lecturer")]
        public async Task<ActionResult<ResponseDto<object>>> DeleteMeetingByLecturer([FromQuery] int meetingId)
        {
            var userEmail = (string)HttpContext.Items["userEmail"]!;
            var userRole = (string)HttpContext.Items["userRole"]!;
            return await _meetingService.DeleteMeetingByLecturerAsync(userEmail, meetingId);
        }
    }
}
