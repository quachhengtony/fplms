using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using FPLMS.Api.Dto;
using Api.Dto.Shared;
using Api.Dto.Request;

namespace Api.Services.Reports
{
    public interface IReportService
    {
        public Task<ResponseDto<CycleReportDTO>> GetCycleReportDetailByLecturerAsync(string userEmail, int reportId);
        public Task<ResponseDto<CycleReportDTO>> GetCycleReportDetailByStudentAsync(string userEmail, int reportId);
        public Task<ResponseDto<ProgressReportDTO>> GetProgressReportDetailByLecturerAsync(string userEmail, int reportId);
        public Task<ResponseDto<ProgressReportDTO>> GetProgressReportDetailByStudentAsync(string userEmail, int reportId);
        public Task<ResponseDto<HashSet<CycleReportDTO>>> GetCycleReportInGroupByLecturerAsync(int? groupId, string userEmail);
        public Task<ResponseDto<HashSet<CycleReportDTO>>> GetCycleReportInGroupByStudentAsync(int? groupId, string userEmail);
        public Task<ResponseDto<HashSet<ProgressReportDTO>>> GetProgressReportInGroupByLecturerAsync(int classId, int groupId, DateTime startDate, DateTime endDate, string userEmail);
        public Task<ResponseDto<HashSet<ProgressReportDTO>>> GetProgressReportInGroupByStudentAsync(int classId, int groupId, DateTime startDate, DateTime endDate, string userEmail);
        public Task<ResponseDto<HashSet<ProgressReportDTO>>> GetProgressReportInGroup(int classId, int groupId, DateTime? startDate, DateTime? endDate);
        public Task<ResponseDto<CycleReportDTO>> AddCycleReportAsync(CreateCycleReportRequest reportRequest, int leaderId);
        public Task<ResponseDto<CycleReportDTO>> UpdateCycleReport(UpdateCycleReportRequest reportRequest, int leaderId);
        public Task<ResponseDto<object>> DeleteCycleReport(int groupId, int reportId, int leaderId);
        public Task<ResponseDto<object>> AddProgressReportAsync(CreateProgressReportRequest reportRequest, int studentId);
        public Task<ResponseDto<object>> UpdateProgressReportAsync(UpdateProgressReportRequest reportRequest, int studentId);
        public Task<ResponseDto<object>> DeleteProgressReportAsync(int groupId, int reportId, int studentId);
        public Task<ResponseDto<HashSet<CycleReportDTO>>> GetCycleReportInClassByLecturer(int classId, string userEmail);
    }
}
