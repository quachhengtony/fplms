using Api.Dto.Shared;
using BusinessObjects.Models;
using Microsoft.Extensions.Logging;
using Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace Api.Services.Reports
{
    public class ReportService : IReportService
    {
        private readonly ILogger<ReportService> _logger;
        private readonly IGroupRepository _groupRepository;
        private readonly ICycleReportRepository _cycleReportRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IStudentGroupRepository _studentGroupRepository;
        private readonly ILecturerRepository _lecturerRepository;
        private readonly IClassRepository _classRepository;
        private readonly IProgressReportRepository _progressReportRepository;
        private const string REPORT_NOT_IN_GROUP = "Report is not belong to this group.";
        private const string NOT_IN_GROUP = "Student not in group.";
        private const string NOT_A_LEADER = "Not a leader.";
        private const string NOT_IN_CYCLE = "Not in cycle";
        private const string CYCLE_REPORT_EXISTS = "This cycle has report already";
        private const string LECTURER_NOT_MANAGE = "Lecturer not manage.";
        private const string GROUP_DISABLE = "Group is disable.";
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
            ILogger<ReportService> logger,
            IGroupRepository groupRepository,
            ICycleReportRepository cycleReportRepository,
            IStudentRepository studentRepository,
            IStudentGroupRepository studentGroupRepository,
            ILecturerRepository lecturerRepository,
            IClassRepository classRepository,
            IProgressReportRepository progressReportRepository)
        {
            _logger = logger;
            _groupRepository = groupRepository;
            _cycleReportRepository = cycleReportRepository;
            _studentRepository = studentRepository;
            _studentGroupRepository = studentGroupRepository;
            _lecturerRepository = lecturerRepository;
            _classRepository = classRepository;
            _progressReportRepository = progressReportRepository;
        }

        public async Task<HashSet<CycleReportDTO>> GetCycleReportInGroup(int groupId)
        {
            _logger.LogInformation("GetCycleReportInGroup(groupId: {0})", groupId);

            if (groupId == null || !await _groupRepository.ExistsById(groupId))
            {
                _logger.LogWarning("{0}{1}", GET_CYCLE_REPORT, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return null;
            }

            var cycleReportSet = await _cycleReportRepository.FindByGroupAsync(new Group(groupId));

            var cycleReportDtoSet = cycleReportSet.Select(cycleReportEntity => MapCycleReportToDTO(cycleReportEntity, groupId)).ToHashSet();

            _logger.LogInformation("Get cycle report from group success");
            return cycleReportDtoSet;
        }

        private CycleReportDTO MapCycleReportToDTO(CycleReport cycleReportEntity, int groupId)
        {
            return new CycleReportDTO
            {
                Id = cycleReportEntity.Id,
                Title = cycleReportEntity.Title,
                Content = cycleReportEntity.Content,
                ResourceLink = cycleReportEntity.ResourceLink,
                GroupId = groupId
            };
        }

        public async Task<CycleReportDTO> AddCycleReport(CreateCycleReportRequest reportRequest, int leaderId)
        {
            var groupId = reportRequest.GroupId;
            _logger.LogInformation("AddCycleReport(reportRequest: {0}, leaderId: {1})", reportRequest, leaderId);

            if (reportRequest == null || groupId == null || leaderId == null ||
                !await _groupRepository.ExistsById(groupId) || !await _studentRepository.ExistsById(leaderId))
            {
                _logger.LogWarning("{0}{1}", CREATE_CYCLE_REPORT, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return null;
            }

            if (await _groupRepository.IsGroupDisableAsync(groupId) == 1)
            {
                _logger.LogWarning("{0}{1}", CREATE_CYCLE_REPORT, GROUP_DISABLE);
                return null;
            }

            if (!leaderId.Equals(await _studentGroupRepository.FindLeaderInGroup(groupId)))
            {
                _logger.LogWarning("{0}{1}", CREATE_CYCLE_REPORT, NOT_A_LEADER);
                return null;
            }

            var currentCycle = await GetCurrentCycle(groupId);
            if (currentCycle == null)
            {
                _logger.LogWarning("{0}{1}", CREATE_CYCLE_REPORT, NOT_IN_CYCLE);
                return null;
            }

            if (await _cycleReportRepository.ExistsByGroupAndCycleNumberAsync(new Group(groupId), currentCycle))
            {
                _logger.LogWarning("{0}{1}", CREATE_CYCLE_REPORT, CYCLE_REPORT_EXISTS);
                return null;
            }

            var cycleReport = new CycleReport
            {
                Title = reportRequest.Title,
                Content = reportRequest.Content,
                ResourceLink = reportRequest.ResourceLink,
                Group = new Group(groupId),
                CycleNumber = currentCycle
            };

            var savedCycleReport = await _cycleReportRepository.Save(cycleReport);

            var responseEntity = MapCycleReportToDTO(savedCycleReport, groupId);

            _logger.LogInformation("Add cycle report success");
            return responseEntity;
        }

        public async Task<CycleReportDTO> UpdateCycleReport(UpdateCycleReportRequest reportRequest, int leaderId)
        {
            var cycleReport = await _cycleReportRepository.GetById(reportRequest.Id);
            var groupId = reportRequest.GroupId;
            _logger.LogInformation("UpdateCycleReport(reportRequest: {0}, groupId: {1}, leaderId: {2})", reportRequest, groupId, leaderId);

            if (reportRequest == null || groupId == null || leaderId == null ||
                !await _groupRepository.ExistsById(groupId) || !await _studentRepository.ExistsById(leaderId) || !await _cycleReportRepository.ExistsById(reportRequest.Id))
            {
                _logger.LogWarning("{0}{1}", UPDATE_CYCLE_REPORT, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return null;
            }

            if (cycleReport.Feedback != null)
            {
                _logger.LogWarning("{0}{1}", UPDATE_CYCLE_REPORT, "Feedback is not null");
                return null;
            }

            if (await _groupRepository.IsGroupDisableAsync(groupId) == 1)
            {
                _logger.LogWarning("{0}{1}", UPDATE_CYCLE_REPORT, GROUP_DISABLE);
                return null;
            }

            if (!leaderId.Equals(await _studentGroupRepository.FindLeaderInGroup(groupId)))
            {
                _logger.LogWarning("{0}{1}", UPDATE_CYCLE_REPORT, NOT_A_LEADER);
                return null;
            }

            var currentCycle = await GetCurrentCycle(groupId);
            if (currentCycle == null || !cycleReport.CycleNumber.Equals(currentCycle))
            {
                _logger.LogWarning("{0}{1}", UPDATE_CYCLE_REPORT, NOT_IN_CYCLE);
                return null;
            }

            cycleReport.Title = reportRequest.Title;
            cycleReport.Content = reportRequest.Content;
            cycleReport.ResourceLink = reportRequest.ResourceLink;

            var savedCycleReport = await _cycleReportRepository.Save(cycleReport);

            var responseEntity = MapCycleReportToDTO(savedCycleReport, groupId);

            _logger.LogInformation("Update cycle report success");
            return responseEntity;
        }

        public async Task DeleteCycleReport(int groupId, int reportId, int leaderId)
        {
            _logger.LogInformation("DeleteCycleReport(reportId: {0}, groupId: {1}, leaderId: {2})", reportId, groupId, leaderId);

            if (reportId == null || groupId == null || leaderId == null || !await _groupRepository.ExistsById(groupId)
                || !await _studentRepository.ExistsById(leaderId) || !await _cycleReportRepository.ExistsById(reportId))
            {
                _logger.LogWarning("{0}{1}", DELETE_CYCLE_REPORT, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return;
            }

            if (!leaderId.Equals(await _studentGroupRepository.FindLeaderInGroup(groupId)))
            {
                _logger.LogWarning("{0}{1}", DELETE_CYCLE_REPORT, NOT_A_LEADER);
                return;
            }

            if (_groupRepository.IsGroupDisable(groupId) == 1)
            {
                _logger.LogWarning("{0}{1}", DELETE_CYCLE_REPORT, GROUP_DISABLE);
                return;
            }

            if (_cycleReportRepository.GetByIdAndGroupId(groupId, reportId) == null)
            {
                _logger.LogWarning("{0}{1}", DELETE_CYCLE_REPORT, REPORT_NOT_IN_GROUP);
                return;
            }

            if (_cycleReportRepository.GetById(reportId).Feedback != null)
            {
                _logger.LogWarning("{0}{1}", DELETE_CYCLE_REPORT, "Feedback is not null");
                return;
            }

            await _cycleReportRepository.Delete(new CycleReport(reportId));
            _logger.LogInformation("Delete cycle report success.");
        }

        public async Task<CycleReportDTO> FeedbackCycleReport(FeedbackCycleReportRequest feedbackCycleReportRequest, string userEmail)
        {
            _logger.LogInformation("{0}{1}", FEEDBACK_CYCLE_REPORT, feedbackCycleReportRequest);
            var lecturerId = await _lecturerRepository.FindLecturerIdByEmail(userEmail);
            if (lecturerId == null || feedbackCycleReportRequest.GroupId == null || feedbackCycleReportRequest.ReportId == null ||
                !await _groupRepository.ExistsById(feedbackCycleReportRequest.GroupId) || !await _cycleReportRepository.ExistsById(feedbackCycleReportRequest.ReportId))
            {
                _logger.LogWarning("{0}{1}", FEEDBACK_CYCLE_REPORT, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return null;
            }

            if (_groupRepository.IsGroupDisable(feedbackCycleReportRequest.GroupId) == 1)
            {
                _logger.LogWarning("{0}{1}", FEEDBACK_CYCLE_REPORT, GROUP_DISABLE);
                return null;
            }

            if (!lecturerId.Equals(_groupRepository.FindOneById(feedbackCycleReportRequest.GroupId).ClassEntity.Lecturer.Id))
            {
                _logger.LogWarning("{0}{1}", FEEDBACK_CYCLE_REPORT, LECTURER_NOT_MANAGE);
                return null;
            }

            if (_cycleReportRepository.GetByIdAndGroupId(feedbackCycleReportRequest.GroupId, feedbackCycleReportRequest.ReportId) == null)
            {
                _logger.LogWarning("{0}{1}", FEEDBACK_CYCLE_REPORT, REPORT_NOT_IN_GROUP);
                return null;
            }

            await_cycleReportRepository.AddFeedbackAsync(feedbackCycleReportRequest.ReportId, feedbackCycleReportRequest.Feedback, feedbackCycleReportRequest.Mark);

            var responseEntity = MapCycleReportToDTO(await _cycleReportRepository.GetById(feedbackCycleReportRequest.ReportId), feedbackCycleReportRequest.GroupId);

            _logger.LogInformation("Feedback cycle report successful.");
            return responseEntity;
        }

        public async Task<ProgressReportDTO> GetProgressReportDetailByLecturer(string userEmail, int reportId)
        {
            _logger.LogInformation("GetProgressReportDetailByLecturer(reportId: {0}, userEmail: {1})", reportId, userEmail);
            var lecturerId = await _lecturerRepository.FindLecturerIdByEmail(userEmail);
            if (lecturerId == null || reportId == null)
            {
                _logger.LogWarning("{0}{1}", GET_PROGRESS_REPORT, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return null;
            }

            if (!await _progressReportRepository.ExistsById(reportId))
            {
                _logger.LogWarning("{0}{1}", GET_PROGRESS_REPORT, ServiceMessage.ID_NOT_EXIST_MESSAGE);
                return null;
            }

            var progressReport = await _progressReportRepository.GetById(reportId);
            if (!lecturerId.Equals(progressReport.Group.ClassEntity.Lecturer.Id))
            {
                _logger.LogWarning("{0}{1}", GET_PROGRESS_REPORT, LECTURER_NOT_MANAGE);
                return null;
            }

            var progressReportDTO = MapProgressReportToDTO(progressReport);
            _logger.LogInformation("Get progress report detail success");
            return progressReportDTO;
        }

        public async Task<ProgressReportDTO> GetProgressReportDetailByStudent(string userEmail, int reportId)
        {
            _logger.LogInformation("GetProgressReportDetailByStudent(reportId: {0}, userEmail: {1})", reportId, userEmail);
            var studentId = await _studentRepository.FindStudentIdByEmail(userEmail);
            if (studentId == null || reportId == null)
            {
                _logger.LogWarning("{0}{1}", GET_PROGRESS_REPORT, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return null;
            }

            if (!await _progressReportRepository.ExistsById(reportId))
            {
                _logger.LogWarning("{0}{1}", GET_PROGRESS_REPORT, ServiceMessage.ID_NOT_EXIST_MESSAGE);
                return null;
            }

            var progressReport = await _progressReportRepository.GetById(reportId);
            if (_studentGroupRepository.IsStudentExistInGroup(progressReport.Group.Id, studentId) == null)
            {
                _logger.LogWarning("{0}{1}", GET_PROGRESS_REPORT, NOT_IN_GROUP);
                return null;
            }

            var progressReportDTO = MapProgressReportToDTO(progressReport);
            _logger.LogInformation("Get progress report detail success");
            return progressReportDTO;
        }

        public async Task<HashSet<ProgressReportDTO>> GetProgressReportInGroupByStudent(int classId, int groupId, DateTime startDate, DateTime endDate, string userEmail)
        {
            _logger.LogInformation("GetProgressReportInGroupByStudent(classId: {0}, groupId: {1}, startDate: {2}, endDate: {3}, userEmail: {4})", classId, groupId, startDate, endDate, userEmail);
            var studentId = await _studentRepository.FindStudentIdByEmail(userEmail);
            if (studentId == null)
            {
                _logger.LogWarning("{0}{1}", GET_PROGRESS_REPORT, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return null;
            }

            if (_studentGroupRepository.IsStudentExistInGroup(groupId, studentId) == null)
            {
                _logger.LogWarning("{0}{1}", GET_PROGRESS_REPORT, "Student not in group");
                return null;
            }

            return await GetProgressReportInGroup(classId, groupId, startDate, endDate);
        }

        public async Task<HashSet<ProgressReportDTO>> GetProgressReportInGroupByLecturer(int classId, int groupId, DateTime startDate, DateTime endDate, string userEmail)
        {
            _logger.LogInformation("GetProgressReportInGroupByLecturer(classId: {0}, groupId: {1}, startDate: {2}, endDate: {3}, userEmail: {4})", classId, groupId, startDate, endDate, userEmail);
            var lecturerId = await _lecturerRepository.FindLecturerIdByEmail(userEmail);
            if (lecturerId == null || !_groupRepository.IsLecturerManageGroup(lecturerId, groupId))
            {
                _logger.LogWarning("{0}{1}", GET_PROGRESS_REPORT, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return null;
            }

            return await GetProgressReportInGroup(classId, groupId, startDate, endDate);
        }

        public async Task<HashSet<ProgressReportDTO>> GetProgressReportInGroup(int classId, int groupId, DateTime startDate, DateTime endDate)
        {
            _logger.LogInformation("GetProgressReportInGroup(classId: {0}, groupId: {1}, startDate: {2}, endDate: {3})", classId, groupId, startDate, endDate);

            if (classId == null || groupId == null || !_classRepository.ExistsById(classId) ||
                !_groupRepository.ExistsById(groupId))
            {
                _logger.LogWarning("{0}{1}", GET_PROGRESS_REPORT, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return null;
            }

            if (await _groupRepository.IsGroupExistsInClassAsync(groupId, classId) == null)
            {
                _logger.LogWarning("{0}{1}", GET_PROGRESS_REPORT, "Group is not exist in class.");
                return null;
            }

            HashSet<ProgressReport> progressReportSet;
            if (startDate == null || endDate == null)
            {
                progressReportSet = await _progressReportRepository.FindByGroup(new Group(groupId));
            }
            else
            {
                progressReportSet = await _progressReportRepository.FindByGroupIdAndTimeFilter(groupId, startDate, endDate);
            }

            var progressReportDtoSet = progressReportSet.Select(progressReportEntity =>
                MapProgressReportToDTO(progressReportEntity)).ToHashSet();

            _logger.LogInformation("Get progress report from group success");
            return progressReportDtoSet;
        }

        private ProgressReportDTO MapProgressReportToDTO(ProgressReport progressReportEntity)
        {
            return new ProgressReportDTO
            {
                Id = progressReportEntity.Id,
                Content = progressReportEntity.Content,
                ReportTime = progressReportEntity.ReportTime,
                GroupId = progressReportEntity.Group.Id,
                StudentId = progressReportEntity.Student.Id
            };
        }

        public async Task AddProgressReport(CreateProgressReportRequest reportRequest, int studentId)
        {
            var groupId = reportRequest.GroupId;
            _logger.LogInformation("AddProgressReport(reportRequest: {0}, groupId: {1}, studentId: {2})", reportRequest, groupId, studentId);

            if (reportRequest == null || groupId == null || studentId == null ||
                !await _groupRepository.IsGroupExistsInClassAsync(groupId) || !_studentRepository.ExistsById(studentId))
            {
                _logger.LogWarning("{0}{1}", CREATE_PROGRESS_REPORT, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return;
            }

            if (_groupRepository.IsGroupDisable(groupId) == 1)
            {
                _logger.LogWarning("{0}{1}", CREATE_PROGRESS_REPORT, GROUP_DISABLE);
                return;
            }

            if (_studentGroupRepository.IsStudentExistInGroup(groupId, studentId) == null)
            {
                _logger.LogWarning("{0}{1}", CREATE_PROGRESS_REPORT, NOT_IN_GROUP);
                return;
            }

            if (await _progressReportRepository.ExistsByStudentIdAndGroupIdAndCurDate(studentId, groupId, DateTime.Now))
            {
                _logger.LogWarning("{0}{1}", CREATE_PROGRESS_REPORT, "Student only allow to send progress report once a day.");
                return;
            }

            var progressReport = new ProgressReport
            {
                Content = reportRequest.Content,
                ReportTime = DateTime.Now,
                Group = new Group(groupId),
                Student = new Student(studentId)
            };

            await _progressReportRepository.Save(progressReport);

            _logger.LogInformation("Add progress report success");
        }

        public async Task UpdateProgressReport(UpdateProgressReportRequest reportRequest, int studentId)
        {
            var groupId = reportRequest.GroupId;
            _logger.LogInformation("UpdateProgressReport(reportRequest: {0}, groupId: {1}, studentId: {2})", reportRequest, groupId, studentId);

            if (reportRequest == null || groupId == null || studentId == null ||
                !await _groupRepository.IsGroupExistsInClassAsync(groupId) || !_studentRepository.ExistsById(studentId) ||
                !await _progressReportRepository.ExistsById(reportRequest.Id))
            {
                _logger.LogWarning("{0}{1}", UPDATE_PROGRESS_REPORT, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return;
            }

            if (await _groupRepository.IsGroupExistsInClassAsync(groupId) == 1)
            {
                _logger.LogWarning("{0}{1}", UPDATE_PROGRESS_REPORT, GROUP_DISABLE);
                return;
            }

            if (_studentGroupRepository.IsStudentExistInGroup(groupId, studentId) == null)
            {
                _logger.LogWarning("{0}{1}", UPDATE_PROGRESS_REPORT, NOT_IN_GROUP);
                return;
            }

            if (!CompareDate(DateTime.Now, await _progressReportRepository.GetDateOfProgressReport(reportRequest.Id)))
            {
                _logger.LogWarning("{0}{1}", UPDATE_PROGRESS_REPORT, "Student not allow to update submitted report.");
                return;
            }

            var progressReport = new ProgressReport
            {
                Id = reportRequest.Id,
                Content = reportRequest.Content,
                Group = new Group(groupId),
                Student = new Student(studentId)
            };

            await _progressReportRepository.Save(progressReport);

            _logger.LogInformation("Update progress report success");
        }

        public async Task DeleteProgressReport(int groupId, int reportId, int studentId)
        {
            _logger.LogInformation("DeleteProgressReport(reportId: {0}, groupId: {1}, studentId: {2})", reportId, groupId, studentId);

            if (reportId == null || groupId == null || studentId == null || !_groupRepository.ExistsById(groupId)
                || !_studentRepository.ExistsById(studentId) || !await _progressReportRepository.ExistsById(reportId))
            {
                _logger.LogWarning("{0}{1}", DELETE_PROGRESS_REPORT, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return;
            }

            if (!_studentId.Equals(await _studentGroupRepository.IsStudentExistInGroup(groupId, studentId)))
            {
                _logger.LogWarning("{0}{1}", DELETE_PROGRESS_REPORT, NOT_IN_GROUP);
                return;
            }

            if (await _groupRepository.IsGroupDisableAsync(groupId) == 1)
            {
                _logger.LogWarning("{0}{1}", DELETE_PROGRESS_REPORT, GROUP_DISABLE);
                return;
            }

            if (_progressReportRepository.GetByIdAndGroupIdAndStudentId(groupId, reportId, studentId) == null)
            {
                _logger.LogWarning("{0}{1}", DELETE_PROGRESS_REPORT, "Report is not belong to this student.");
                return;
            }

            if (!CompareDate(DateTime.Now, await _progressReportRepository.GetDateOfProgressReport(reportId)))
            {
                _logger.LogWarning("{0}{1}", UPDATE_PROGRESS_REPORT, "Student not allow to delete submitted report.");
                return;
            }

            await _progressReportRepository.Delete(new ProgressReport(reportId));

            _logger.LogInformation("Delete progress report success.");
        }

        public async Task<int?> GetCurrentCycle(int groupId)
        {
            var classEntity = (await _groupRepository.FindOneByIdAsync(groupId)).Class;
            var cycleDuration = classEntity.CycleDuration;
            var semester = classEntity.SemesterCodeNavigation;
            var currentDate = DateTime.Now;
            if (currentDate < semester.StartDate || currentDate > semester.EndDate)
            {
                // not in semester
                return null;
            }

            var currentCycle = (int)((currentDate - semester.StartDate).TotalDays / (cycleDuration * 24 * 60 * 60)) + 1;
            return currentCycle + 1;
        }

        public bool CompareDate(DateTime date1, DateTime date2)
        {
            return date1.Date.Equals(date2.Date);
        }
    }
}
