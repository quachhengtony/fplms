using Api.Dto.Shared.plms.ManagementService.Model.DTO;
using Api.Services.Semesters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/management/semesters")]
    public class SemesterController : ControllerBase
    {
        private readonly ISemesterService _semesterService;

        public SemesterController(ISemesterService semesterService)
        {
            _semesterService = semesterService;
        }

        [HttpGet, Authorize(Roles = "Lecturer,Student,Admin")]
        public async Task<ActionResult<HashSet<SemesterDTO>>> GetSemester([FromQuery] string? code)
        {
            if (code == null)
                code = "";
            var semesters = await _semesterService.GetSemester(code);
            return Ok(semesters);
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddSemester([FromBody] SemesterDTO semesterDto)
        {
            await _semesterService.AddSemester(semesterDto);
            return NoContent();
        }

        [HttpPut, Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSemester([FromBody] SemesterDTO semesterDto)
        {
            await _semesterService.UpdateSemester(semesterDto);
            return NoContent();
        }

        [HttpDelete("{code}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSemester(string code)
        {
            await _semesterService.DeleteSemester(code);
            return NoContent();
        }
    }
}