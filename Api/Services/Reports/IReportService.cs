using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Api.Services.Reports
{
    public interface IReportService
    {
        Task<Set<CycleReportDTO>> GetCycleReportInGroup(int groupId);
        Task<CycleReportDTO> AddCycleReport(CreateCycleReportRequest reportRequest, int leaderId);
        Task<CycleReportDTO> UpdateCycleReport(UpdateCycleReportRequest reportRequest, int leaderId);
        Task DeleteCycleReport(int groupId, int reportId, int leaderId);
        Task<CycleReportDTO> FeedbackCycleReport(FeedbackCycleReportRequest feedbackCycleReportRequest, string userEmail);
        Task<ProgressReportDTO> GetProgressReportDetailByLecturer(string userEmail, int reportId);
        Task<ProgressReportDTO> GetProgressReportDetailByStudent(string userEmail, int reportId);
        Task<Set<ProgressReportDTO>> GetProgressReportInGroupByStudent(int classId, int groupId, DateTime startDate, DateTime endDate, string userEmail);
        Task<Set<ProgressReportDTO>> GetProgressReportInGroupByLecturer(int classId, int groupId, DateTime startDate, DateTime endDate, string userEmail);
        Task<Set<ProgressReportDTO>> GetProgressReportInGroup(int classId, int groupId, DateTime startDate, DateTime endDate);
        Task AddProgressReport(CreateProgressReportRequest reportRequest, int studentId);
        Task UpdateProgressReport(UpdateProgressReportRequest reportRequest, int studentId);
        Task DeleteProgressReport(int groupId, int reportId, int studentId);
        Task<int?> GetCurrentCycle(int groupId);
        bool CompareDate(DateTime date1, DateTime date2);
    }
}
