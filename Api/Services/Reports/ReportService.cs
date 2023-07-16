using Api.Dto.Request;
using Api.Dto.Shared;
using Api.Services.Constant;
using BusinessObjects.Models;
using FPLMS.Api.Dto;
using Microsoft.Extensions.Logging;
using Repositories;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using static Google.Apis.Requests.BatchRequest;

namespace Api.Services.Reports
{
    public class ReportService : IReportService
    {
        private readonly ILogger<ReportService> _logger;
        private readonly ICycleReportRepository _cycleReportRepository;
        private readonly IClassRepository _classRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IStudentGroupRepository _studentGroupRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ILecturerRepository _lecturerRepository;
        private readonly IProgressReportRepository _progressReportRepository;


        private const string REPORT_NOT_IN_GROUP = "Report is not belong to this group.";
        private const string NOT_IN_GROUP = "Student not in group.";
        private const string NOT_A_LEADER = "Not a leader.";
        private const string NOT_IN_CYCLE = "Not in cycle";
        private const string CYCLE_REPORT_EXISTS = "This cycle has report already";
        private const string LECTURER_NOT_MANAGE = "Lecturer not manage.";
        private const string GROUP_DISABLE = "Group is disabled.";
        private const string CREATE_CYCLE_REPORT = "Create cycle report: ";
        private const string UPDATE_CYCLE_REPORT = "Update cycle report: ";
        private const string DELETE_CYCLE_REPORT = "Delete cycle report: ";
        private const string CREATE_PROGRESS_REPORT = "Create progress report: ";
        private const string UPDATE_PROGRESS_REPORT = "Update progress report: ";
        private const string DELETE_PROGRESS_REPORT = "Delete progress report: ";
        private const string GET_CYCLE_REPORT = "Get cycle report: ";
        private const string GET_PROGRESS_REPORT = "Get progress report: ";
        private const string FEEDBACK_CYCLE_REPORT = "Feedback cycle report: ";
        public ReportService(
            ILogger<ReportService> logger, IClassRepository classRepo)
        {
            _logger = logger;
            _cycleReportRepository = CycleReportRepository.Instance;
            _classRepository = classRepo;
            _groupRepository = GroupRepository.Instance;
            _studentGroupRepository = StudentGroupRepository.Instance;
            _studentRepository = StudentRepository.Instance;
            _lecturerRepository = LecturerRepository.Instance;
            _progressReportRepository = ProgressReportRepository.Instance;
        }

        private CycleReportDTO MapToCycleReportDTO(CycleReport cycleReport)
        {
            return new CycleReportDTO
            {
                Id = cycleReport.Id,
                Title = cycleReport.Title,
                Content = cycleReport.Content,
                CycleNumber = cycleReport.CycleNumber,
                Feedback = cycleReport.Feedback,
                ResourceLink = cycleReport.ResourceLink,
                Mark = cycleReport.Mark.Value,
                GroupId = cycleReport.Group.Id
                // Map other properties here
            };
        }

        private ProgressReportDTO MapToProgressReportDTO(ProgressReport progressReport)
        {
            return new ProgressReportDTO
            {
                Id = progressReport.Id,
                Title = progressReport.Title,
                Content = progressReport.Content,
                ReportTime = progressReport.ReportTime,
                GroupId = progressReport.Group.Id,
                StudentId = progressReport.Student.Id,
                // Map other properties here
            };
        }

        public async Task<ResponseDto<CycleReportDTO>> GetCycleReportDetailByLecturerAsync(string userEmail, int reportId)
        {
            _logger.LogInformation("GetCycleReportDetailByLecturerAsync(reportId: {reportId}, userEmail: {userEmail})", reportId, userEmail);
            int? lecturerId = await _lecturerRepository.FindLecturerIdByEmailAsync(userEmail);
            if (lecturerId == null || reportId <= 0)
            {
                _logger.LogWarning("{GET_CYCLE_REPORT}{INVALID_ARGUMENT_MESSAGE}", GET_CYCLE_REPORT, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return new ResponseDto<CycleReportDTO>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE };
            }

            if (!await _cycleReportRepository.ExistsById(reportId))
            {
                _logger.LogWarning("{GET_CYCLE_REPORT}{ID_NOT_EXIST_MESSAGE}", GET_CYCLE_REPORT, ServiceMessage.ID_NOT_EXIST_MESSAGE);
                return new ResponseDto<CycleReportDTO>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.ID_NOT_EXIST_MESSAGE };
            }

            CycleReport cycleReport = await _cycleReportRepository.GetByIdAsync(reportId);

            if (!lecturerId.Equals(cycleReport.Group.Class.Lecturer.Id))
            {
                _logger.LogWarning("{GET_CYCLE_REPORT}{LECTURER_NOT_MANAGE}", GET_CYCLE_REPORT, LECTURER_NOT_MANAGE);
                return new ResponseDto<CycleReportDTO>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = LECTURER_NOT_MANAGE };
            }

            CycleReportDTO cycleReportDTO = MapToCycleReportDTO(cycleReport);
            _logger.LogInformation("Get cycle report detail success");
            return new ResponseDto<CycleReportDTO>{ code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE, data = cycleReportDTO };
        }

        public async Task<ResponseDto<CycleReportDTO>> GetCycleReportDetailByStudentAsync(string userEmail, int reportId)
        {
            _logger.LogInformation("GetCycleReportDetailByStudentAsync(reportId: {reportId}, userEmail: {userEmail})", reportId, userEmail);
            int? studentId = await _studentRepository.FindStudentIdByEmail(userEmail);
            if (studentId == null || reportId <= 0)
            {
                _logger.LogWarning("{GET_CYCLE_REPORT}{INVALID_ARGUMENT_MESSAGE}", GET_CYCLE_REPORT, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return new ResponseDto<CycleReportDTO>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE };
            }

            if (!await _cycleReportRepository.ExistsById(reportId))
            {
                _logger.LogWarning("{GET_CYCLE_REPORT}{ID_NOT_EXIST_MESSAGE}", GET_CYCLE_REPORT, ServiceMessage.ID_NOT_EXIST_MESSAGE);
                return new ResponseDto<CycleReportDTO>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.ID_NOT_EXIST_MESSAGE };
            }

            CycleReport cycleReport = await _cycleReportRepository.GetByIdAsync(reportId);

            if (await _studentGroupRepository.IsStudentExistInGroup(cycleReport.Group.Id, (int)studentId) == 0)
            {
                _logger.LogWarning("{GET_CYCLE_REPORT}{NOT_IN_GROUP}", GET_CYCLE_REPORT, NOT_IN_GROUP);
                return new ResponseDto<CycleReportDTO>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = NOT_IN_GROUP };
            }

            CycleReportDTO cycleReportDTO = MapToCycleReportDTO(cycleReport);
            _logger.LogInformation("Get cycle report detail success");
            return new ResponseDto<CycleReportDTO>{ code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE, data = cycleReportDTO };
        }

        public async Task<ResponseDto<ProgressReportDTO>> GetProgressReportDetailByLecturerAsync(string userEmail, int reportId)
        {
            _logger.LogInformation("GetProgressReportDetailByLecturerAsync(reportId: {reportId}, userEmail: {userEmail})", reportId, userEmail);
            int? lecturerId = await _lecturerRepository.FindLecturerIdByEmailAsync(userEmail);
            if (lecturerId == null || reportId <= 0)
            {
                _logger.LogWarning("{GET_PROGRESS_REPORT}{INVALID_ARGUMENT_MESSAGE}", GET_PROGRESS_REPORT, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return new ResponseDto<ProgressReportDTO>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE };
            }

            if (!await _progressReportRepository.ExistsById(reportId))
            {
                _logger.LogWarning("{GET_PROGRESS_REPORT}{ID_NOT_EXIST_MESSAGE}", GET_PROGRESS_REPORT, ServiceMessage.ID_NOT_EXIST_MESSAGE);
                return new ResponseDto<ProgressReportDTO>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.ID_NOT_EXIST_MESSAGE };
            }

            ProgressReport progressReport = await _progressReportRepository.GetByIdAsync(reportId);

            if (!lecturerId.Equals(progressReport.Group.Class.Lecturer.Id))
            {
                _logger.LogWarning("{GET_PROGRESS_REPORT}{LECTURER_NOT_MANAGE}", GET_PROGRESS_REPORT, LECTURER_NOT_MANAGE);
                return new ResponseDto<ProgressReportDTO>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = LECTURER_NOT_MANAGE };
            }

            ProgressReportDTO progressReportDTO = MapToProgressReportDTO(progressReport);
            _logger.LogInformation("Get progress report detail success");
            return new ResponseDto<ProgressReportDTO>{ code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE, data = progressReportDTO };
        }

        public async Task<ResponseDto<ProgressReportDTO>> GetProgressReportDetailByStudentAsync(string userEmail, int reportId)
        {
            _logger.LogInformation("GetProgressReportDetailByStudentAsync(reportId: {reportId}, userEmail: {userEmail})", reportId, userEmail);
            int? studentId = await _studentRepository.FindStudentIdByEmail(userEmail);
            if (studentId <= 0 || reportId <= 0)
            {
                _logger.LogWarning("{GET_PROGRESS_REPORT}{INVALID_ARGUMENT_MESSAGE}", GET_PROGRESS_REPORT, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return new ResponseDto<ProgressReportDTO>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE };
            }

            if (!await _progressReportRepository.ExistsById(reportId))
            {
                _logger.LogWarning("{GET_PROGRESS_REPORT}{ID_NOT_EXIST_MESSAGE}", GET_PROGRESS_REPORT, ServiceMessage.ID_NOT_EXIST_MESSAGE);
                return new ResponseDto<ProgressReportDTO>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.ID_NOT_EXIST_MESSAGE };
            }

            ProgressReport progressReport = await _progressReportRepository.GetByIdAsync(reportId);

            if (await _studentGroupRepository.IsStudentExistInGroup(progressReport.Group.Id, (int)studentId) == 0)
            {
                _logger.LogWarning("{GET_PROGRESS_REPORT}{NOT_IN_GROUP}", GET_PROGRESS_REPORT, NOT_IN_GROUP);
                return new ResponseDto<ProgressReportDTO>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = NOT_IN_GROUP };
            }

            ProgressReportDTO progressReportDTO = MapToProgressReportDTO(progressReport);
            _logger.LogInformation("Get progress report detail success");
            return new ResponseDto<ProgressReportDTO>{ code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE, data = progressReportDTO };
        }

        public async Task<ResponseDto<HashSet<CycleReportDTO>>> GetCycleReportInGroupByLecturerAsync(int? groupId, string userEmail)
        {
            _logger.LogInformation("GetCycleReportInGroupByLecturerAsync( groupId: {groupId}, userEmail: {userEmail})", groupId, userEmail);
            int? lecturerId = await _lecturerRepository.FindLecturerIdByEmailAsync(userEmail);
            if (lecturerId == null || groupId == null || groupId == 0)
            {
                _logger.LogWarning("{GET_CYCLE_REPORT}{INVALID_ARGUMENT_MESSAGE}", GET_CYCLE_REPORT, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return new ResponseDto<HashSet<CycleReportDTO>>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE };
            }

            if (!lecturerId.Equals((await _groupRepository.FindOneByIdAsync((int)groupId)).Class.LecturerId))
            {
                _logger.LogWarning("{GET_CYCLE_REPORT}{LECTURER_NOT_MANAGE}", GET_CYCLE_REPORT, LECTURER_NOT_MANAGE);
                return new ResponseDto<HashSet<CycleReportDTO>>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = LECTURER_NOT_MANAGE };
            }

            return await GetCycleReportInGroup(groupId);
        }

        public async Task<ResponseDto<HashSet<CycleReportDTO>>> GetCycleReportInGroupByStudentAsync(int? groupId, string userEmail)
        {
            _logger.LogInformation("GetCycleReportInGroupByLecturerAsync( groupId: {groupId}, userEmail: {userEmail})", groupId, userEmail);
            int? studentId = await _studentRepository.FindStudentIdByEmail(userEmail);
            if (studentId == null || groupId == null)
            {
                _logger.LogWarning("{GET_CYCLE_REPORT}{INVALID_ARGUMENT_MESSAGE}", GET_CYCLE_REPORT, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return new ResponseDto<HashSet<CycleReportDTO>>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE };
            }

            if (await _studentGroupRepository.IsStudentExistInGroup((int)groupId, (int)studentId) == 0)
            {
                _logger.LogWarning("{GET_CYCLE_REPORT}{NOT_IN_GROUP}", GET_CYCLE_REPORT, NOT_IN_GROUP);
                return new ResponseDto<HashSet<CycleReportDTO>>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = NOT_IN_GROUP };
            }
            return await GetCycleReportInGroup(groupId);
        }

        public async Task<ResponseDto<HashSet<CycleReportDTO>>> GetCycleReportInGroup(int? groupId)
        {
            _logger.LogInformation("GetCycleReportInGroup(groupId: {groupId})", groupId);

            if (groupId == 0 || groupId == null || !(await _groupRepository.ExistsById((int)groupId)))
            {
                _logger.LogWarning("{GET_CYCLE_REPORT}{ServiceMessage.INVALID_ARGUMENT_MESSAGE}");
                return new ResponseDto<HashSet<CycleReportDTO>>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE };
            }

            HashSet<CycleReport> cycleReportSet = (await _cycleReportRepository.FindByGroupAsync(new Group((int)groupId))).ToHashSet();

            HashSet<CycleReportDTO> cycleReportDtoSet = cycleReportSet.Select(cycleReportEntity =>
            {
                CycleReportDTO dto = MapToCycleReportDTO(cycleReportEntity);
                dto.GroupId = cycleReportEntity.Group.Id;
                return dto;
            }).ToHashSet();

            _logger.LogInformation("Get cycle report from group success");
            return new ResponseDto<HashSet<CycleReportDTO>>{ code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE, data = cycleReportDtoSet };
        }

        public async Task<ResponseDto<HashSet<ProgressReportDTO>>> GetProgressReportInGroupByLecturerAsync(int classId, int groupId, DateTime startDate, DateTime endDate, string userEmail)
        {
            _logger.LogInformation("GetProgressReportInGroupByLecturerAsync(classId: {classId}, groupId: {groupId}, startDate: {startDate}, endDate: {endDate}, userEmail: {userEmail})", classId, groupId, startDate, endDate, userEmail);
            int? lecturerId = await _lecturerRepository.FindLecturerIdByEmailAsync(userEmail);
            if (lecturerId == null)
            {
                _logger.LogWarning("{GET_PROGRESS_REPORT}{INVALID_ARGUMENT_MESSAGE}", GET_PROGRESS_REPORT, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return new ResponseDto<HashSet<ProgressReportDTO>>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE };
            }

            if (!lecturerId.Equals((await _classRepository.FindOneByIdAsync(classId)).LecturerId))
            {
                _logger.LogWarning("{GET_PROGRESS_REPORT}{LECTURER_NOT_MANAGE}", GET_PROGRESS_REPORT, LECTURER_NOT_MANAGE);
                return new ResponseDto<HashSet<ProgressReportDTO>>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = LECTURER_NOT_MANAGE };
            }

            return await GetProgressReportInGroup(classId, groupId, startDate, endDate);
        }

        public async Task<ResponseDto<HashSet<ProgressReportDTO>>> GetProgressReportInGroupByStudentAsync(int classId, int groupId, DateTime startDate, DateTime endDate, string userEmail)
        {
            _logger.LogInformation("GetProgressReportInGroupByStudentAsync(classId: {classId}, groupId: {groupId}, startDate: {startDate}, endDate: {endDate}, userEmail: {userEmail})", classId, groupId, startDate, endDate, userEmail);
            int? studentId = await _studentRepository.FindStudentIdByEmail(userEmail);
            if (studentId == null)
            {
                _logger.LogWarning("{GET_PROGRESS_REPORT}{INVALID_ARGUMENT_MESSAGE}", GET_PROGRESS_REPORT, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return new ResponseDto<HashSet<ProgressReportDTO>>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE };
            }

            if (await _studentGroupRepository.IsStudentExistInGroup(groupId, (int)studentId) <= 0)
            {
                _logger.LogWarning("{GET_PROGRESS_REPORT}{NOT_IN_GROUP}", GET_PROGRESS_REPORT, NOT_IN_GROUP);
                return new ResponseDto<HashSet<ProgressReportDTO>>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = NOT_IN_GROUP };
            }

            return await GetProgressReportInGroup(classId, groupId, startDate, endDate);
        }

        public async Task<ResponseDto<HashSet<ProgressReportDTO>>> GetProgressReportInGroup(int classId, int groupId, DateTime? startDate, DateTime? endDate)
        {
            _logger.LogInformation("GetProgressReportInGroup(classId: {classId}, groupId: {groupId}, startDate: {startDate}, endDate: {endDate})", classId, groupId, startDate, endDate);

            if (classId == 0 || groupId == 0 || !(await _classRepository.ExistsByIdAsync(classId)) || !(await _groupRepository.ExistsById(groupId)))
            {
                _logger.LogWarning("{GET_PROGRESS_REPORT}{ServiceMessage.INVALID_ARGUMENT_MESSAGE}");
                return new ResponseDto<HashSet<ProgressReportDTO>>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE };
            }

            if ((await _groupRepository.IsGroupExistsInClassAsync(groupId, classId)) <= 0)
            {
                _logger.LogWarning("{GET_PROGRESS_REPORT}Group is not exist in class.");
                return new ResponseDto<HashSet<ProgressReportDTO>>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = "Group is not exist in class." };
            }

            HashSet<ProgressReport> progressReportSet;
            if (startDate == null || endDate == null)
            {
                progressReportSet = await _progressReportRepository.FindByGroup(new Group(groupId));
            }
            else
            {
                progressReportSet = await _progressReportRepository.FindByGroupIdAndTimeFilter(groupId, startDate.Value, endDate.Value);
            }

            HashSet<ProgressReportDTO> progressReportDtoSet = progressReportSet.Select(progressReportEntity =>
            {
                ProgressReportDTO dto = MapToProgressReportDTO(progressReportEntity);
                dto.GroupId = progressReportEntity.GroupId;
                dto.StudentId = progressReportEntity.StudentId;
                return dto;
            }).ToHashSet();

            _logger.LogInformation("Get progress report from group success");
            return new ResponseDto<HashSet<ProgressReportDTO>> { code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE, data = progressReportDtoSet };
        }

        public async Task<ResponseDto<CycleReportDTO>> AddCycleReportAsync(CreateCycleReportRequest reportRequest, int leaderId)
        {
            _logger.LogInformation("AddCycleReportAsync(reportRequest: {reportRequest}, lecturerId: {lecturerId})", reportRequest, leaderId);
            int groupId = reportRequest.GroupId;
            if (reportRequest == null || groupId <= 0 || leaderId <= 0 || !await _groupRepository.ExistsById(groupId) || !await _studentRepository.ExistsById(leaderId))
            {
                _logger.LogWarning("{CREATE_CYCLE_REPORT}{INVALID_ARGUMENT_MESSAGE}", CREATE_CYCLE_REPORT, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return new ResponseDto<CycleReportDTO>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE };
            }

            if (await _groupRepository.IsGroupDisableAsync(groupId) == 1)
            {
                _logger.LogWarning("{CREATE_CYCLE_REPORT}{GROUP_DISABLE}", CREATE_CYCLE_REPORT, GROUP_DISABLE);
                return new ResponseDto<CycleReportDTO>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = GROUP_DISABLE };
            }

            if (leaderId != await _studentGroupRepository.FindLeaderInGroup(groupId))
            {
                _logger.LogWarning("{CREATE_CYCLE_REPORT}{NOT_A_LEADER}");
                return new ResponseDto<CycleReportDTO>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = NOT_A_LEADER };
            }

            int? currentCycle = await GetCurrentCycle(groupId);
            if (currentCycle == null)
            {
                _logger.LogWarning("{CREATE_CYCLE_REPORT}{NOT_IN_CYCLE}");
                return new ResponseDto<CycleReportDTO>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = NOT_IN_CYCLE };
            }

            if (await _cycleReportRepository.ExistsByGroupAndCycleNumberAsync(new Group(groupId), currentCycle.Value) == 0)
            {
                _logger.LogWarning("{CREATE_CYCLE_REPORT}{CYCLE_REPORT_EXISTS}");
                return new ResponseDto<CycleReportDTO>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = CYCLE_REPORT_EXISTS };
            }

            CycleReport cycleReport = new CycleReport()
            {
                Title = reportRequest.Title,
                Content = reportRequest.Content,
                ResourceLink = reportRequest.ResourceLink,
                GroupId = groupId
            };
            cycleReport.Group = new Group(groupId);
            cycleReport.CycleNumber = currentCycle.Value;

            CycleReportDTO responseEntity = MapToCycleReportDTO(await _cycleReportRepository.SaveAsync(cycleReport));

            _logger.LogInformation("Add cycle report success");
            return new ResponseDto<CycleReportDTO> { code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE, data = responseEntity };
        }

        public async Task<ResponseDto<CycleReportDTO>> UpdateCycleReport(UpdateCycleReportRequest reportRequest, int leaderId)
        {
            CycleReport cycleReport = await _cycleReportRepository.GetByIdAsync(reportRequest.Id);
            int groupId = reportRequest.GroupId;
            _logger.LogInformation("UpdateCycleReport(reportRequest: {}, groupId: {}, leaderId: {})", reportRequest, groupId, leaderId);
            if (reportRequest == null || groupId == null || leaderId == null ||
                !await _groupRepository.ExistsById(groupId) || !await _studentRepository.ExistsById(leaderId) || !await _cycleReportRepository.ExistsById(reportRequest.Id))
            {
                _logger.LogWarning("{}{}", UPDATE_CYCLE_REPORT, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return new ResponseDto<CycleReportDTO>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE };
            }
            if (cycleReport.Feedback != null)
            {
                _logger.LogWarning("{}{}", UPDATE_CYCLE_REPORT, "Feedback is not null");
                return new ResponseDto<CycleReportDTO>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = "Can not update report having feedback" };
            }
            if (await _groupRepository.IsGroupDisableAsync(groupId) == 1)
            {
                _logger.LogWarning("{}{}", UPDATE_CYCLE_REPORT, GROUP_DISABLE);
                return new ResponseDto<CycleReportDTO>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = GROUP_DISABLE };
            }
            if (!leaderId.Equals(await _studentGroupRepository.FindLeaderInGroup(groupId)))
            {
                _logger.LogWarning("{}{}", UPDATE_CYCLE_REPORT, NOT_A_LEADER);
                return new ResponseDto<CycleReportDTO>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = NOT_A_LEADER };
            }
            int? currentCycle = await GetCurrentCycle(groupId);
            if (currentCycle == null || !cycleReport.CycleNumber.Equals(currentCycle))
            {
                _logger.LogWarning("{}{}", UPDATE_CYCLE_REPORT, NOT_IN_CYCLE);
                return new ResponseDto<CycleReportDTO>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = NOT_IN_CYCLE };
            }
            cycleReport.Content = reportRequest.Content;
            cycleReport.Title = reportRequest.Title;
            cycleReport.ResourceLink = reportRequest.ResourceLink;
            CycleReportDTO responseEntity = MapToCycleReportDTO(await _cycleReportRepository.SaveAsync(cycleReport));
            _logger.LogInformation("Update cycle report success");
            return new ResponseDto<CycleReportDTO>{ code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE, data = responseEntity };
        }

        public async Task<ResponseDto<object>> DeleteCycleReport(int groupId, int reportId, int leaderId)
        {
            _logger.LogInformation("DeleteCycleReport(reportId: {}, groupId: {}, leaderId: {})", reportId, groupId, leaderId);

            if (reportId == null || groupId == null || leaderId == null || !await _groupRepository.ExistsById(groupId)
                || !await _studentRepository.ExistsById(leaderId) || !await _cycleReportRepository.ExistsById(reportId))
            {
                _logger.LogWarning("{}{}", DELETE_CYCLE_REPORT, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return new ResponseDto<object>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE };
            }
            if (await _groupRepository.IsGroupDisableAsync(groupId) == 1)
            {
                _logger.LogWarning("{}{}", DELETE_CYCLE_REPORT, GROUP_DISABLE);
                return new ResponseDto<object>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = GROUP_DISABLE };
            }
            if (!leaderId.Equals(_studentGroupRepository.FindLeaderInGroup(groupId)))
            {
                _logger.LogWarning("{}{}", DELETE_CYCLE_REPORT, NOT_A_LEADER);
                return new ResponseDto<object>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = NOT_A_LEADER };
            }
            if (await _cycleReportRepository.GetByIdAndGroupIdAsync(groupId, reportId) == null)
            {
                _logger.LogWarning("{}{}", DELETE_CYCLE_REPORT, REPORT_NOT_IN_GROUP);
                return new ResponseDto<object>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = REPORT_NOT_IN_GROUP };
            }
            if ((await _cycleReportRepository.GetByIdAsync(reportId)).Feedback != null)
            {
                _logger.LogWarning("{}{}", DELETE_CYCLE_REPORT, "Feedback is not null");
                return new ResponseDto<object>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = "Can not delete report having feedback" };
            }
            _cycleReportRepository.DeleteAsync(new CycleReport(reportId));
            _logger.LogInformation("Delete cycle report success.");
            return new ResponseDto<object>{ code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE };
        }

        public async Task<ResponseDto<object>> AddProgressReportAsync(CreateProgressReportRequest reportRequest, int studentId)
        {
            _logger.LogInformation("AddProgressReportAsync(reportRequest: {reportRequest}, studentId: {studentId})", reportRequest, studentId);
            int groupId = reportRequest.GroupId;
            if (reportRequest == null || groupId <= 0 || studentId <= 0 || !await _groupRepository.ExistsById(groupId) || !await _studentRepository.ExistsById(studentId))
            {
                _logger.LogWarning("{CREATE_PROGRESS_REPORT}{INVALID_ARGUMENT_MESSAGE}", CREATE_PROGRESS_REPORT, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return new ResponseDto<object>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE };
            }

            if (await _groupRepository.IsGroupDisableAsync(groupId) == 1)
            {
                _logger.LogWarning("{CREATE_PROGRESS_REPORT}{GROUP_DISABLE}", CREATE_PROGRESS_REPORT, GROUP_DISABLE);
                return new ResponseDto<object>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = GROUP_DISABLE };
            }

            if (await _studentGroupRepository.IsStudentExistInGroup(groupId, studentId) == 0)
            {
                _logger.LogWarning("{CREATE_PROGRESS_REPORT}{NOT_IN_GROUP}", CREATE_PROGRESS_REPORT, NOT_IN_GROUP);
                return new ResponseDto<object>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = NOT_IN_GROUP };
            }

            if (await _progressReportRepository.ExistsByStudentIdAndGroupIdAndCurDate(studentId, groupId, DateTime.UtcNow.Date) == 0)
            {
                _logger.LogWarning("{CREATE_PROGRESS_REPORT}{PROGRESS_REPORT_EXIST}", CREATE_PROGRESS_REPORT, "Student only allow to send progress report once a day.");
                return new ResponseDto<object>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = "Student only allow to send progress report once a day." };
            }

            ProgressReport progressReport = new ProgressReport
            {
                Title = reportRequest.Title,
                Content = reportRequest.Content,
                GroupId = reportRequest.GroupId,
                ReportTime = DateTime.UtcNow.Date,
                Group = await _groupRepository.FindOneByIdAsync(groupId),
                Student = await _studentRepository.FindOneById(studentId)
            };

            await _progressReportRepository.SaveAsync(progressReport);
            _logger.LogInformation("Add progress report success");
            return new ResponseDto<object>{ code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE };
        }

        public async Task<ResponseDto<object>> UpdateProgressReportAsync(UpdateProgressReportRequest reportRequest, int studentId)
        {
            _logger.LogInformation("UpdateProgressReportAsync(reportRequest: {reportRequest}, studentId: {studentId})", reportRequest, studentId);
            int groupId = reportRequest.GroupId;
            if (reportRequest == null || groupId <= 0 || studentId <= 0 || !await _groupRepository.ExistsById(groupId) || !await _studentRepository.ExistsById(studentId) || !await _progressReportRepository.ExistsById(reportRequest.Id))
            {
                _logger.LogWarning("{UPDATE_PROGRESS_REPORT}{INVALID_ARGUMENT_MESSAGE}", UPDATE_PROGRESS_REPORT, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return new ResponseDto<object>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE };
            }

            if (await _groupRepository.IsGroupDisableAsync(groupId) == 1)
            {
                _logger.LogWarning("{UPDATE_PROGRESS_REPORT}{GROUP_DISABLE}", UPDATE_PROGRESS_REPORT, GROUP_DISABLE);
                return new ResponseDto<object>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = GROUP_DISABLE };
            }

            if (await _studentGroupRepository.IsStudentExistInGroup(groupId, studentId) <= 0)
            {
                _logger.LogWarning("{UPDATE_PROGRESS_REPORT}{NOT_IN_GROUP}", UPDATE_PROGRESS_REPORT, NOT_IN_GROUP);
                return new ResponseDto<object>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = NOT_IN_GROUP };
            }

            if (!CompareDate(DateTime.UtcNow.Date, await _progressReportRepository.GetDateOfProgressReport(reportRequest.Id)))
            {
                _logger.LogWarning("{UPDATE_PROGRESS_REPORT}{STUDENT_NOT_ALLOW_UPDATE}", UPDATE_PROGRESS_REPORT, "Student not allow to update submitted report.");
                return new ResponseDto<object>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = "Student not allow to update submitted report." };
            }

            ProgressReport progressReport = new ProgressReport
            {
                Id = reportRequest.Id,
                Title = reportRequest.Title,
                Content = reportRequest.Content,
                GroupId = reportRequest.GroupId,
                ReportTime = DateTime.UtcNow.Date,
                Group = await _groupRepository.FindOneByIdAsync(groupId),
                Student = await _studentRepository.FindOneById(studentId)
            };

            await _progressReportRepository.SaveAsync(progressReport);
            _logger.LogInformation("Update progress report success");
            return new ResponseDto<object>{ code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE };
        }

        public async Task<ResponseDto<object>> DeleteProgressReportAsync(int groupId, int reportId, int studentId)
        {
            _logger.LogInformation("DeleteProgressReportAsync(reportId: {reportId}, groupId: {groupId}, studentId: {studentId})", reportId, groupId, studentId);
            if (reportId <= 0 || groupId <= 0 || studentId <= 0 || !await _groupRepository.ExistsById(groupId) || !await _studentRepository.ExistsById(studentId) || !await _progressReportRepository.ExistsById(reportId))
            {
                _logger.LogWarning("{DELETE_PROGRESS_REPORT}{INVALID_ARGUMENT_MESSAGE}", DELETE_PROGRESS_REPORT, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return new ResponseDto<object>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE };
            }

            if (await _studentGroupRepository.IsStudentExistInGroup(groupId, studentId) >= 0)
            {
                _logger.LogWarning("{DELETE_PROGRESS_REPORT}{NOT_IN_GROUP}", DELETE_PROGRESS_REPORT, NOT_IN_GROUP);
                return new ResponseDto<object>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = NOT_IN_GROUP };
            }

            if (await _groupRepository.IsGroupDisableAsync(groupId) == 1)
            {
                _logger.LogWarning("{DELETE_PROGRESS_REPORT}{GROUP_DISABLE}", DELETE_PROGRESS_REPORT, GROUP_DISABLE);
                return new ResponseDto<object>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = GROUP_DISABLE };
            }

            if (!CompareDate(DateTime.UtcNow.Date, await _progressReportRepository.GetDateOfProgressReport(reportId)))
            {
                _logger.LogWarning("{DELETE_PROGRESS_REPORT}{STUDENT_NOT_ALLOW_DELETE}", DELETE_PROGRESS_REPORT, "Student not allow to delete submitted report.");
                return new ResponseDto<object>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = "Student not allow to delete submitted report." };
            }

            await _progressReportRepository.DeleteAsync(reportId);
            _logger.LogInformation("Delete progress report success.");
            return new ResponseDto<object>{ code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE };
        }

        public bool CompareDate(DateTime date1, DateTime date2)
        {
            return date1.Date == date2.Date;
        }
        public async Task<int?> GetCurrentCycle(int groupId)
        {
            Class classEntity = (await _groupRepository.FindOneByIdAsync(groupId)).Class;
            int cycleDuration = (int)classEntity.CycleDuration;
            Semester semester = classEntity.SemesterCodeNavigation;
            DateTime currentDate = DateTime.Now;
            if (currentDate < semester.StartDate || currentDate > semester.EndDate)
            {
                // Not in semester
                return null;
            }
            TimeSpan timeSpan = (TimeSpan)(currentDate - semester.StartDate);
            int totalDays = timeSpan.Days;
            int currentCycle = (totalDays / cycleDuration) + 1;
            return currentCycle;
        }
    }
}