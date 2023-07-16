using Api.Dto.Shared;
using Api.Services.Constant;
using Api.Services.Meetings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using FPLMS.Api.Dto;
using FPLMS.Api.Enum;

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

        [HttpGet]
        public async Task<ActionResult<ResponseDto<HashSet<MeetingDto>>>> GetMeetingInGroup([FromQuery] int? classId, [FromQuery] int? groupId, [FromQuery] string userRole, [FromQuery] string userEmail, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
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

        [HttpGet("{meetingId}")]
        public async Task<ActionResult<ResponseDto<MeetingDto>>> GetMeetingById(int meetingId, [FromQuery] string userRole, [FromQuery] string userEmail)
        {
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

        [HttpPost]
        public async Task<ActionResult<ResponseDto<MeetingDto>>> ScheduleMeetingByLecturer(MeetingDto meetingDto, [FromQuery] string userEmail)
        {
            return await _meetingService.ScheduleMeetingByLecturerAsync(meetingDto, userEmail);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDto<object>>> UpdateMeetingByLecturer(MeetingDto meetingDto, [FromQuery] string userEmail)
        {
            return await _meetingService.UpdateMeetingByLecturerAsync(meetingDto, userEmail);
        }

        [HttpDelete]
        public async Task<ActionResult<ResponseDto<object>>> DeleteMeetingByLecturer([FromQuery] int meetingId, [FromQuery] string userEmail)
        {
            return await _meetingService.DeleteMeetingByLecturerAsync(userEmail, meetingId);
        }
    }
}
