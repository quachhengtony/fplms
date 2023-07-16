using Api.Dto.Shared;
using Api.Services.Constant;
using Api.Services.Meetings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using FPLMS.Api.Dto;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/management/meetings")]
    public class MeetingController : ControllerBase
    {
        private readonly IMeetingService _meetingService;
        private readonly IAuthenticationService _authenticationService;

        public MeetingController(IMeetingService meetingService, IAuthenticationService authenticationService)
        {
            _meetingService = meetingService;
            _authenticationService = authenticationService;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto<HashSet<MeetingDto>>>> GetMeetingInGroup([FromQuery] int? classId, [FromQuery] int? groupId, [FromQuery] string userRole, [FromQuery] string userEmail, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            //if (userRole.Contains(GatewayConstant.ROLE_LECTURER))
            //{
                return await _meetingService.GetMeetingInGroupByLecturerAsync(classId, groupId, new DateTimeOffset(startDate.Value), new DateTimeOffset(endDate.Value), userEmail);
            //}
            //if (userRole.Contains(GatewayConstant.ROLE_STUDENT))
            //{
                return await _meetingService.GetMeetingInGroupByStudentAsync(classId, groupId, new DateTimeOffset(startDate.Value), new DateTimeOffset(endDate.Value), userEmail);
            //}
            return StatusCode(403, "Not have role access");
        }

        [HttpGet("{meetingId}")]
        public async Task<ActionResult<ResponseDto<MeetingDto>>> GetMeetingById(int meetingId, [FromQuery] string userRole, [FromQuery] string userEmail)
        {
            //if (userRole.Contains(GatewayConstant.ROLE_LECTURER))
            //{
                return await _meetingService.GetMeetingDetailByLecturerAsync(userEmail, meetingId);
            //}
            //if (userRole.Contains(GatewayConstant.ROLE_STUDENT))
            //{
                return await _meetingService.GetMeetingDetailByStudentAsync(userEmail, meetingId);
            //}
            return StatusCode(403, "Not have role access");
        }

        //[HttpPost]
        //public async Task<ActionResult<ResponseDto<MeetingDto>>> ScheduleMeetingByLecturer(MeetingDto meetingDto, [FromQuery] string userEmail)
        //{
        //    //meetingDto.LecturerId = _authenticationService.GetLectureIdByEmail(userEmail);
        //    //return await _meetingService.ScheduleMeetingByLecturerAsync(meetingDto);
        //}

        //[HttpPut]
        //public async Task<ActionResult> UpdateMeetingByLecturer(MeetingDto meetingDto, [FromQuery] string userEmail)
        //{
        //    //meetingDto.LecturerId = _authenticationService.GetLectureIdByEmail(userEmail);
        //    //return await _meetingService.UpdateMeetingByLecturerAsync(meetingDto);
        //}

        //[HttpDelete]
        //public async Task<ActionResult<ResponseDto<object>>> DeleteMeetingByLecturer([FromQuery] int meetingId, [FromQuery] string userEmail)
        //{
        //    //return await _meetingService.DeleteMeetingByLecturerAsync(_authenticationService.GetLectureIdByEmail(userEmail), meetingId);
        //}
    }
}
