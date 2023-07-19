using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Api.Dto.Shared;
using Api.Services.Projects;
using Api.Services.Groups;
using Api.Services.Students;
using FPLMS.Api.Dto;
using FPLMS.Api.Enum;
using Microsoft.AspNetCore.Authorization;
using System.Data;

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

        [HttpGet, Authorize(Roles = "Lecturer,Student")]
        public Task<ResponseDto<HashSet<ProjectDto>>> GetAllProjects(
            [FromQuery(Name = "classId")] int? classId,
            [FromQuery(Name = "semesterCode")] string? semesterCode)
        {
            var userEmail = (string)HttpContext.Items["userEmail"]!;
            var userRole = (string)HttpContext.Items["userRole"]!;
            if (userRole.Contains(RoleTypes.Lecturer))
            {
                var projects = _projectService.GetProjectByLecturerAsync(semesterCode, classId, userEmail).Result;
                return Task.FromResult(projects);
            }
            else if (userRole.Contains(RoleTypes.Student))
            {
                var projects = _projectService.GetProjectFromClassByStudentAsync(classId, userEmail);
                return projects;
            }

            return null;
        }

        [HttpPut("{projectId}"), Authorize(Roles = "Student")]
        public async Task<ResponseDto<object>> ChooseProject(

            [FromQuery(Name = "classId")] int classId,
            int projectId)
        {
            var userEmail = (string)HttpContext.Items["userEmail"]!;
            var userRole = (string)HttpContext.Items["userRole"]!;
            var studentId = await _studentService.GetStudentIdByEmail(userEmail);
            return await _groupService.ChooseProjectInGroup(classId, projectId, studentId);
        }

        [HttpPost, Authorize(Roles = "Lecturer")]
        public async Task<ActionResult<ResponseDto<int>>> AddProject(

            [FromBody] ProjectDto ProjectDto)
        {
            var userEmail = (string)HttpContext.Items["userEmail"]!;
            var userRole = (string)HttpContext.Items["userRole"]!;
            return await _projectService.AddProjectAsync(ProjectDto, userEmail);
        }

        [HttpPut, Authorize(Roles = "Lecturer")]
        public async Task<ResponseDto<object>> UpdateProject(

            [FromBody] ProjectDto ProjectDto)
        {
            var userEmail = (string)HttpContext.Items["userEmail"]!;
            var userRole = (string)HttpContext.Items["userRole"]!;
            return await _projectService.UpdateProjectAsync(ProjectDto, userEmail);
        }

        [HttpDelete("{projectId}"), Authorize(Roles = "Lecturer")]
        public async Task<ResponseDto<object>> DeleteProject(

            int projectId)
        {
            var userEmail = (string)HttpContext.Items["userEmail"]!;
            var userRole = (string)HttpContext.Items["userRole"]!;
            return await _projectService.DeleteProjectAsync(projectId, userEmail);
        }
    }
}
