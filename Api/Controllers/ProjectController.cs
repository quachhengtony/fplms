using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Api.Dto.Shared;
using Api.Services.Projects;
using Api.Services.Groups;
using Api.Services.Students;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/management/projects")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly IGroupService _groupService;
        private readonly IStudentService _studentService;
        private readonly ILogger<ProjectController> _logger;

        public ProjectController(
            IProjectService projectService,
            IGroupService groupService,
            IStudentService studentService,
            ILogger<ProjectController> logger)
        {
            _projectService = projectService;
            _groupService = groupService;
            _studentService = studentService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<HashSet<ProjectDTO>>> GetAllProjects(
            [FromQuery(Name = "classId")] int? classId,
            [FromQuery(Name = "semesterCode")] string semesterCode,
            [FromQuery(Name = "userRole")] string userRole,
            [FromQuery(Name = "userEmail")] string userEmail)
        {
            if (userRole.Contains("ROLE_LECTURER"))
            {
                var projects = await _projectService.GetProjectByLecturer(semesterCode, classId, userEmail);
                return Ok(projects);
            }
            else if (userRole.Contains("ROLE_STUDENT"))
            {
                var projects = await _projectService.GetProjectFromClassByStudent((int)classId, userEmail);
                return Ok(projects);
            }

            return StatusCode(403, "Not have role access");
        }

        [HttpPut("{projectId}")]
        public async Task<ActionResult> ChooseProject(
            [FromQuery(Name = "userEmail")] string userEmail,
            [FromQuery(Name = "classId")] int classId,
            int projectId)
        {
            var studentId = await _studentService.GetStudentIdByEmail(userEmail);
            await _groupService.ChooseProjectInGroup(classId, projectId, studentId);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<int>> AddProject(
            [FromQuery(Name = "userEmail")] string userEmail,
            [FromBody] ProjectDTO projectDTO)
        {
            return await _projectService.AddProject(projectDTO, userEmail);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateProject(
            [FromQuery(Name = "userEmail")] string userEmail,
            [FromBody] ProjectDTO projectDTO)
        {
            await _projectService.UpdateProject(projectDTO, userEmail);
            return NoContent();
        }

        [HttpDelete("{projectId}")]
        public async Task<ActionResult> DeleteProject(
            [FromQuery(Name = "userEmail")] string userEmail,
            int projectId)
        {
            await _projectService.DeleteProject(projectId, userEmail);
            return NoContent();
        }
    }
}
