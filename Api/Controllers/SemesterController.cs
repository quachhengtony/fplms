using Api.Dto.Shared.plms.ManagementService.Model.DTO;
using Api.Services.Semesters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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

        [HttpGet]
        public async Task<ActionResult<HashSet<SemesterDTO>>> GetSemester([FromQuery] string code)
        {
            var semesters = await _semesterService.GetSemester(code);
            return Ok(semesters);
        }

        [HttpPost]
        public async Task<IActionResult> AddSemester([FromBody] SemesterDTO semesterDto)
        {
            await _semesterService.AddSemester(semesterDto);
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSemester([FromBody] SemesterDTO semesterDto)
        {
            await _semesterService.UpdateSemester(semesterDto);
            return NoContent();
        }

        [HttpDelete("{code}")]
        public async Task<IActionResult> DeleteSemester(string code)
        {
            await _semesterService.DeleteSemester(code);
            return NoContent();
        }
    }
}