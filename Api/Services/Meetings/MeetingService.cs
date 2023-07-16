using Api.Dto.Shared;
using Api.Services.Constant;
using BusinessObjects.Models;
using FPLMS.Api.Dto;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using Repositories;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Services.Meetings
{
    public class MeetingService : IMeetingService
    {
        private readonly IMeetingRepository _meetingRepository;
        private readonly IClassRepository _classRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ILecturerRepository _lecturerRepository;
        private readonly IStudentGroupRepository _studentGroupRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly ILogger<MeetingService> _logger;

        private static readonly DateTimeOffset TIMESTAMP_MAX_VALUE = new DateTimeOffset(9999, 12, 31, 23, 59, 59, TimeSpan.Zero);
        private static readonly DateTimeOffset TIMESTAMP_MIN_VALUE = new DateTimeOffset(0000, 01, 01, 00, 00, 00, TimeSpan.Zero);

        private const string NOT_IN_GROUP = "Student is not exist in group.";
        private const string LECTURER_NOT_MANAGE = "Lecturer not manage this meeting.";
        private const string GET_MEETING = "Get meeting: ";
        private const string SCHEDULING_MEETING_MESSAGE = "Schedule meeting: ";
        private const string UPDATE_MEETING_MESSAGE = "Update meeting: ";
        private const string DELETE_MEETING_MESSAGE = "Update meeting: ";

        public MeetingService(ILogger<MeetingService> logger)
        {
            _meetingRepository = MeetingRepository.Instance;
            _classRepository = ClassRepository.Instance;
            _studentRepository = StudentRepository.Instance;
            _lecturerRepository = LecturerRepository.Instance;
            _studentGroupRepository = StudentGroupRepository.Instance;
            _groupRepository = GroupRepository.Instance;
            _logger = logger;
        }

        public async Task<ResponseDto<MeetingDto>> GetMeetingDetailByLecturerAsync(string userEmail, int meetingId)
        {
            _logger.LogInformation("GetMeetingDetailByLecturerAsync(meetingId: {0}, userEmail: {1})", meetingId, userEmail);
            var lecturerId = await _lecturerRepository.FindLecturerIdByEmailAsync(userEmail);
            if (lecturerId <= 0 || meetingId <= 0)
            {
                _logger.LogWarning("{0}{1}", GET_MEETING, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return new ResponseDto<MeetingDto>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE };
            }
            if (!await _meetingRepository.ExistsById(meetingId))
            {
                _logger.LogWarning("{0}{1}", GET_MEETING, ServiceMessage.ID_NOT_EXIST_MESSAGE);
                return new ResponseDto<MeetingDto>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.ID_NOT_EXIST_MESSAGE };
            }
            var meeting = await _meetingRepository.FindOneByIdAsync(meetingId);
            if (!lecturerId.Equals(meeting.Group.Class.Lecturer.Id))
            {
                _logger.LogWarning("{0}{1}", GET_MEETING, LECTURER_NOT_MANAGE);
                return new ResponseDto<MeetingDto>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = LECTURER_NOT_MANAGE };
            }
            var meetingDto = MapMeetingToDto(meeting);
            _logger.LogInformation("Get meeting detail success");
            return new ResponseDto<MeetingDto>{ code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE, data = meetingDto };
        }

        public async Task<ResponseDto<MeetingDto>> GetMeetingDetailByStudentAsync(string userEmail, int meetingId)
        {
            _logger.LogInformation("GetMeetingDetailByStudentAsync(meetingId: {0}, userEmail: {1})", meetingId, userEmail);
            var studentId = await _studentRepository.FindStudentIdByEmail(userEmail);
            if (studentId <= 0 || meetingId <= 0)
            {
                _logger.LogWarning("{0}{1}", GET_MEETING, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return new ResponseDto<MeetingDto>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE };
            }
            if (!await _meetingRepository.ExistsById(meetingId))
            {
                _logger.LogWarning("{0}{1}", GET_MEETING, ServiceMessage.ID_NOT_EXIST_MESSAGE);
                return new ResponseDto<MeetingDto>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.ID_NOT_EXIST_MESSAGE };
            }
            var meeting = await _meetingRepository.FindOneByIdAsync(meetingId);
            if (await _studentGroupRepository.IsStudentExistInGroup(meeting.Group.Id, studentId) <= 0)
            {
                _logger.LogWarning("{0}{1}", GET_MEETING, NOT_IN_GROUP);
                return new ResponseDto<MeetingDto>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = NOT_IN_GROUP };
            }
            var meetingDto = MapMeetingToDto(meeting);
            _logger.LogInformation("Get meeting detail success");
            return new ResponseDto<MeetingDto>{ code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE, data = meetingDto };
        }

        public async Task<ResponseDto<HashSet<MeetingDto>>> GetMeetingInGroupByStudentAsync(int? classId, int? groupId, DateTimeOffset? startDate, DateTimeOffset? endDate, string userEmail)
        {
            _logger.LogInformation("GetMeetingInGroupByStudentAsync(classId: {0}, groupId: {1}, startDate: {2}, endDate: {3}, userEmail: {4})", classId, groupId, startDate, endDate, userEmail);
            if (startDate == null) startDate = TIMESTAMP_MIN_VALUE;
            if (endDate == null) endDate = TIMESTAMP_MAX_VALUE;

            var studentId = await _studentRepository.FindStudentIdByEmail(userEmail);
            if (studentId <= 0)
            {
                _logger.LogWarning("{0}{1}", GET_MEETING, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return new ResponseDto<HashSet<MeetingDto>>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE };
            }

            HashSet<Meeting> meetingSet;
            if (classId == null && groupId == null)
            {
                // Find by student
                meetingSet = (await _meetingRepository.FindByStudentIdAsync(studentId, startDate.Value.UtcDateTime, endDate.Value.UtcDateTime)).ToHashSet();
            }
            else if (classId != null)
            {
                if (groupId == null)
                {
                    // Invalid case: classId != 0 && groupId == 0
                    _logger.LogWarning("{0}{1}", GET_MEETING, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                    return new ResponseDto<HashSet<MeetingDto>>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE };
                }
                else
                {
                    // Find by group
                    if (!await _groupRepository.ExistsById(groupId.Value) || !await _classRepository.ExistsByIdAsync(classId))
                    {
                        _logger.LogWarning("{0}{1}", GET_MEETING, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                        return new ResponseDto<HashSet<MeetingDto>>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE };
                    }
                    if (await _groupRepository.IsGroupExistsInClassAsync(groupId.Value, classId.Value) == null)
                    {
                        _logger.LogWarning("{0}{1}", GET_MEETING, "Group is not exist in class.");
                        return new ResponseDto<HashSet<MeetingDto>>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.ID_NOT_EXIST_MESSAGE };
                    }
                    if (await _studentGroupRepository.IsStudentExistInGroup(groupId.Value, studentId) == 0)
                    {
                        _logger.LogWarning("{0}{1}", GET_MEETING, NOT_IN_GROUP);
                        return new ResponseDto<HashSet<MeetingDto>>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = NOT_IN_GROUP };
                    }
                    meetingSet = (await _meetingRepository.FindByGroupIdAsync(groupId.Value, startDate.Value.UtcDateTime, endDate.Value.UtcDateTime)).ToHashSet();
                }
            }
            else
            {
                // Invalid case: classId == 0 && groupId != 0
                _logger.LogWarning("{0}{1}", GET_MEETING, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return new ResponseDto<HashSet<MeetingDto>>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE };
            }

            var meetingDtoSet = meetingSet.Select(m => MapMeetingToDto(m)).ToHashSet();
            _logger.LogInformation("Get meeting in group success.");
            return new ResponseDto<HashSet<MeetingDto>>{ code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE, data = meetingDtoSet };
        }

        public async Task<ResponseDto<HashSet<MeetingDto>>> GetMeetingInGroupByLecturerAsync(int? classId, int? groupId, DateTimeOffset? startDate, DateTimeOffset? endDate, string userEmail)
        {
            _logger.LogInformation("GetMeetingInGroupByLecturerAsync(classId: {0}, groupId: {1}, startDate: {2}, endDate: {3}, userEmail: {4})", classId, groupId, startDate, endDate, userEmail);
            if (startDate == null) startDate = TIMESTAMP_MIN_VALUE;
            if (endDate == null) endDate = TIMESTAMP_MAX_VALUE;

            var lecturerId = await _lecturerRepository.FindLecturerIdByEmailAsync(userEmail);
            if (lecturerId <= 0)
            {
                _logger.LogWarning("{0}{1}", GET_MEETING, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return new ResponseDto<HashSet<MeetingDto>>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE };
            }

            HashSet<Meeting> meetingSet;
            if (classId == null && groupId == null)
            {
                // Find by lecturer
                meetingSet = (await _meetingRepository.FindByLecturerIdAsync(lecturerId, startDate.Value.UtcDateTime, endDate.Value.UtcDateTime)).ToHashSet();
            }
            else if (classId != null)
            {
                if (!await _classRepository.ExistsByIdAsync(classId))
                {
                    _logger.LogWarning("{0}{1}", GET_MEETING, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                    return new ResponseDto<HashSet<MeetingDto>>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE };
                }
                if (!lecturerId.Equals((await _classRepository.FindOneByIdAsync(classId.Value)).Lecturer.Id))
                {
                    _logger.LogWarning("{0}{1}", GET_MEETING, LECTURER_NOT_MANAGE);
                    return new ResponseDto<HashSet<MeetingDto>>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = LECTURER_NOT_MANAGE };
                }
                if (groupId == null)
                {
                    // Find by class
                    meetingSet = (await _meetingRepository.FindByClassIdAsync(classId.Value, startDate.Value.UtcDateTime, endDate.Value.UtcDateTime)).ToHashSet();
                }
                else
                {
                    // Find by group
                    if (!await _groupRepository.ExistsById(groupId.Value))
                    {
                        _logger.LogWarning("{0}{1}", GET_MEETING, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                        return new ResponseDto<HashSet<MeetingDto>>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE };
                    }
                    if (await _groupRepository.IsGroupExistsInClassAsync(groupId.Value, classId.Value) == null)
                    {
                        _logger.LogWarning("{0}{1}", GET_MEETING, "Group is not exist in class.");
                        return new ResponseDto<HashSet<MeetingDto>>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.ID_NOT_EXIST_MESSAGE };
                    }
                    meetingSet = (await _meetingRepository.FindByGroupIdAsync(groupId.Value, startDate.Value.UtcDateTime, endDate.Value.UtcDateTime)).ToHashSet();
                }
            }
            else
            {
                // Invalid case: classId == 0 && groupId != 0
                _logger.LogWarning("{0}{1}", GET_MEETING, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return new ResponseDto<HashSet<MeetingDto>>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE };
            }

            var meetingDtoSet = meetingSet.Select(m => MapMeetingToDto(m)).ToHashSet();
            _logger.LogInformation("Get meeting in group success.");
            return new ResponseDto<HashSet<MeetingDto>>{ code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE, data = meetingDtoSet };
        }

        public async Task<ResponseDto<MeetingDto>> ScheduleMeetingByLecturerAsync(MeetingDto meetingDto, string userEmail)
        {
            _logger.LogInformation("{0}{1}", SCHEDULING_MEETING_MESSAGE, meetingDto);
            // Check whether the group is in the class of the lecturer
            if (meetingDto.GroupId == 0 || !(await _groupRepository.FindLectureIdOfGroupAsync(meetingDto.GroupId)).Equals(meetingDto.LecturerId))
            {
                _logger.LogWarning("{0}{1}", SCHEDULING_MEETING_MESSAGE, ServiceMessage.FORBIDDEN_MESSAGE);
                return new ResponseDto<MeetingDto>{ code = ServiceStatusCode.FORBIDDEN_STATUS, message = ServiceMessage.FORBIDDEN_MESSAGE };
            }
            if (string.IsNullOrEmpty(meetingDto.Title) || meetingDto.ScheduleTime == null || string.IsNullOrEmpty(meetingDto.Link))
            {
                _logger.LogWarning("{0}{1}", SCHEDULING_MEETING_MESSAGE, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return new ResponseDto<MeetingDto>{ code = ServiceStatusCode.UNAUTHENTICATED_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE };
            }
            meetingDto.Id = 0;
            var meeting = MapDtoToMeeting(meetingDto);
            meeting.Lecturer = new Lecturer { Id = meetingDto.LecturerId };
            meeting.Group = new Group { Id = meetingDto.GroupId };
            meeting.LecturerId = await _lecturerRepository.FindLecturerIdByEmailAsync(userEmail);
            meetingDto.Id = await _meetingRepository.SaveAsync(meeting);
            _logger.LogInformation("{0}{1}", SCHEDULING_MEETING_MESSAGE, ServiceMessage.SUCCESS_MESSAGE);
            return new ResponseDto<MeetingDto>{ code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE, data = meetingDto };
        }

        public async Task<ResponseDto<object>> UpdateMeetingByLecturerAsync(MeetingDto meetingDto, string userEmail)
        {
            _logger.LogInformation("{0}{1}", UPDATE_MEETING_MESSAGE, meetingDto);
            var meeting = await _meetingRepository.FindOneByIdAsync(meetingDto.Id);
            if (!meetingDto.LecturerId.Equals(meeting.Lecturer.Id))
            {
                _logger.LogWarning("{0}{1}", SCHEDULING_MEETING_MESSAGE, ServiceMessage.FORBIDDEN_MESSAGE);
                return new ResponseDto<object>{ code = ServiceStatusCode.FORBIDDEN_STATUS, message = ServiceMessage.FORBIDDEN_MESSAGE };
            }
            if (string.IsNullOrEmpty(meetingDto.Title) || meetingDto.ScheduleTime == null || string.IsNullOrEmpty(meetingDto.Link) || !meetingDto.GroupId.Equals(meeting.Group.Id))
            {
                _logger.LogWarning("{0}{1}", SCHEDULING_MEETING_MESSAGE, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return new ResponseDto<object>{ code = ServiceStatusCode.UNAUTHENTICATED_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE };
            }
            meeting.Feedback = meetingDto.Feedback;
            meeting.Link = meetingDto.Link;
            meeting.ScheduleTime = meetingDto.ScheduleTime;
            meeting.Title = meetingDto.Title;
            meeting.LecturerId = await _lecturerRepository.FindLecturerIdByEmailAsync(userEmail);
            await _meetingRepository.SaveAsync(meeting);
            _logger.LogInformation("{0}{1}", UPDATE_MEETING_MESSAGE, ServiceMessage.SUCCESS_MESSAGE);
            return new ResponseDto<object>{ code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE };
        }

        public async Task<ResponseDto<object>> DeleteMeetingByLecturerAsync(string userEmail, int meetingId)
        {
            _logger.LogInformation("{0}{1}", DELETE_MEETING_MESSAGE, meetingId);
            int lectureId = await _lecturerRepository.FindLecturerIdByEmailAsync(userEmail);
            if (!(await _meetingRepository.FindOneByIdAsync(meetingId)).LecturerId.Equals(lectureId))
            {
                _logger.LogWarning("{0}{1}", SCHEDULING_MEETING_MESSAGE, ServiceMessage.FORBIDDEN_MESSAGE);
                return new ResponseDto<object>{ code = ServiceStatusCode.FORBIDDEN_STATUS, message = ServiceMessage.FORBIDDEN_MESSAGE };
            }
            await _meetingRepository.DeleteByIdAsync(meetingId);
            _logger.LogInformation("{0}{1}", DELETE_MEETING_MESSAGE, ServiceMessage.SUCCESS_MESSAGE);
            return new ResponseDto<object>{ code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE };
        }

        private MeetingDto MapMeetingToDto(Meeting meeting)
        {
            return new MeetingDto
            {
                Id = meeting.Id,
                Title = meeting.Title,
                ScheduleTime = meeting.ScheduleTime.Value,
                Link = meeting.Link,
                Feedback = meeting.Feedback,
                LecturerId = meeting.Lecturer.Id,
                GroupId = meeting.Group.Id
            };
        }

        private Meeting MapDtoToMeeting(MeetingDto meetingDto)
        {
            return new Meeting
            {
                Id = meetingDto.Id,
                Title = meetingDto.Title,
                ScheduleTime = meetingDto.ScheduleTime,
                Link = meetingDto.Link,
                Feedback = meetingDto.Feedback
            };
        }
    }
}
