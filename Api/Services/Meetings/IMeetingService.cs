using Api.Dto.Shared;
using Api.Services.Constant;
using BusinessObjects.Models;
using FPLMS.Api.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Api.Services.Meetings
{
    public interface IMeetingService
    {
        public Task<ResponseDto<MeetingDto>> GetMeetingDetailByLecturerAsync(string userEmail, int meetingId);
        public Task<ResponseDto<MeetingDto>> GetMeetingDetailByStudentAsync(string userEmail, int meetingId);
        public Task<ResponseDto<HashSet<MeetingDto>>> GetMeetingInGroupByStudentAsync(int? classId, int? groupId, DateTimeOffset? startDate, DateTimeOffset? endDate, string userEmail);
        public Task<ResponseDto<HashSet<MeetingDto>>> GetMeetingInGroupByLecturerAsync(int? classId, int? groupId, DateTimeOffset? startDate, DateTimeOffset? endDate, string userEmail);
        public Task<ResponseDto<MeetingDto>> ScheduleMeetingByLecturerAsync(MeetingDto meetingDto);
        public Task<ResponseDto<object>> UpdateMeetingByLecturerAsync(MeetingDto meetingDto);
        public Task<ResponseDto<object>> DeleteMeetingByLecturerAsync(int lectureId, int meetingId);

    }
}
