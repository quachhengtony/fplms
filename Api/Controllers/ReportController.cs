using Api.Dto.Request;
using Api.Dto.Shared;
using Api.Services.Reports;
using FPLMS.Api.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using Api.Services.Students;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/management")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly IStudentService _studentService;

        public ReportController(IReportService reportService, IStudentService studentService)
        {
            _reportService = reportService;
            _studentService = studentService;
        }

        [HttpGet("cycle-reports")]
        public async Task<ActionResult<ResponseDto<HashSet<CycleReportDTO>>>> GetCycleReport(
            [FromQuery] int? classId,
            [FromQuery] int? groupId)
        {
            var userEmail = User.FindFirstValue("email");
            var userRole = User.FindFirstValue("role");

            if (userRole.Contains("LECTURER"))
            {
                var response = await _reportService.GetCycleReportInGroupByLecturerAsync(groupId, userEmail);
                return Ok(response);
            }

            if (userRole.Contains("STUDENT"))
            {
                var response = await _reportService.GetCycleReportInGroupByStudentAsync(groupId, userEmail);
                return Ok(response);
            }

            return StatusCode(403, "Not have role access");
        }

        [HttpGet("cycle-reports/{reportId}")]
        public async Task<ActionResult<ResponseDto<CycleReportDTO>>> GetCycleReportById(int reportId)
        {
            var userEmail = User.FindFirstValue("email");
            var userRole = User.FindFirstValue("role");

            if (userRole.Contains("LECTURER"))
            {
                var response = await _reportService.GetCycleReportDetailByLecturerAsync(userEmail, reportId);
                return Ok(response);
            }

            if (userRole.Contains("STUDENT"))
            {
                var response = await _reportService.GetCycleReportDetailByStudentAsync(userEmail, reportId);
                return Ok(response);
            }

            return StatusCode(403, "Not have role access");
        }

        [HttpPost("cycle-reports")]
        public async Task<ActionResult<ResponseDto<CycleReportDTO>>> AddCycleReport(
            [FromBody] CreateCycleReportRequest createCycleReportRequest)
        {
            var userEmail = User.FindFirstValue("email");
            var studentId = await _studentService.GetStudentIdByEmail(userEmail);
            var response = await _reportService.AddCycleReportAsync(createCycleReportRequest, studentId);
            return Ok(response);
        }

        [HttpPut("cycle-reports")]
        public async Task<ActionResult<ResponseDto<CycleReportDTO>>> UpdateCycleReport(
            [FromBody] UpdateCycleReportRequest updateCycleReportRequest)
        {
            var userEmail = User.FindFirstValue("email");
            var studentId = await _studentService.GetStudentIdByEmail(userEmail);
            var response = await _reportService.UpdateCycleReport(updateCycleReportRequest, studentId);
            return Ok(response);
        }

        [HttpDelete("cycle-reports/{reportId}")]
        public async Task<ActionResult<ResponseDto<object>>> DeleteCycleReport(int groupId, int reportId)
        {
            var userEmail = User.FindFirstValue("email");
            var studentId = await _studentService.GetStudentIdByEmail(userEmail);
            var response = await _reportService.DeleteCycleReport(groupId, reportId, studentId);
            return Ok(response);
        }

        [HttpGet("progress-reports")]
        public async Task<ActionResult<ResponseDto<HashSet<ProgressReportDTO>>>> GetProgressReportFromGroup(
            [FromQuery] int classId,
            [FromQuery] int groupId,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            var userEmail = User.FindFirstValue("email");
            var userRole = User.FindFirstValue("role");

            if (userRole.Contains("LECTURER"))
            {
                var response = await _reportService.GetProgressReportInGroupByLecturerAsync(
                    classId, groupId, startDate.Value, endDate.Value, userEmail);
                return Ok(response);
            }

            if (userRole.Contains("STUDENT"))
            {
                var response = await _reportService.GetProgressReportInGroupByStudentAsync(
                    classId, groupId, startDate.Value, endDate.Value, userEmail);
                return Ok(response);
            }

            return StatusCode(403, "Not have role access");
        }

        [HttpGet("progress-reports/{reportId}")]
        public async Task<ActionResult<ResponseDto<ProgressReportDTO>>> GetProgressReportById(int reportId)
        {
            var userEmail = User.FindFirstValue("email");
            var userRole = User.FindFirstValue("role");

            if (userRole.Contains("LECTURER"))
            {
                var response = await _reportService.GetProgressReportDetailByLecturerAsync(userEmail, reportId);
                return Ok(response);
            }

            if (userRole.Contains("STUDENT"))
            {
                var response = await _reportService.GetProgressReportDetailByStudentAsync(userEmail, reportId);
                return Ok(response);
            }

            return StatusCode(403, "Not have role access");
        }

        [HttpPost("progress-reports")]
        public async Task<ActionResult<ResponseDto<object>>> AddProgressReport(
            [FromBody] CreateProgressReportRequest createProgressReportRequest)
        {
            var userEmail = User.FindFirstValue("email");
            var studentId = await _studentService.GetStudentIdByEmail(userEmail);
            var response = await _reportService.AddProgressReportAsync(createProgressReportRequest, studentId);
            return Ok(response);
        }

        [HttpPut("progress-reports")]
        public async Task<ActionResult<ResponseDto<object>>> UpdateProgressReport(
            [FromBody] UpdateProgressReportRequest updateProgressReportRequest)
        {
            var userEmail = User.FindFirstValue("email");
            var studentId = await _studentService.GetStudentIdByEmail(userEmail);
            var response = await _reportService.UpdateProgressReportAsync(updateProgressReportRequest, studentId);
            return Ok(response);
        }

        [HttpDelete("progress-reports/{reportId}")]
        public async Task<ActionResult<ResponseDto<object>>> DeleteProgressReport(int groupId, int reportId)
        {
            var userEmail = User.FindFirstValue("email");
            var studentId = await _studentService.GetStudentIdByEmail(userEmail);
            var response = await _reportService.DeleteProgressReportAsync(groupId, reportId, studentId);
            return Ok(response);
        }
    }
}
