using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Api.Dto.Shared;
using Api.Services.Constant;
using BusinessObjects.Models;
using FPLMS.Api.Dto;
using Repositories.Interfaces;
using System.Threading.Tasks;
using Repositories;

namespace Api.Services.Projects
{
    public class ProjectService : IProjectService
    {
        private readonly ILogger<ProjectService> _logger;
        private readonly IProjectRepository _projectRepository;
        private readonly IClassRepository _classRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ILecturerRepository _lecturerRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly ISemesterRepository _semesterRepository;

        private const string PROJECT_NOT_MANAGE = "Lecturer not manage this project.";
        private const string SUBJECT_NOT_EXIST = "Subject not exist";
        private const string SEMESTER_NOT_EXIST = "Semester not exist";
        private const string GET_PROJECT = "Get project from class: ";
        private const string ADD_PROJECT = "Add project to class: ";
        private const string UPDATE_PROJECT = "Update project in class: ";
        private const string DELETE_PROJECT = "Delete project in class: ";

        public ProjectService(
    ILogger<ProjectService> logger, IProjectRepository projectRepository, IClassRepository classRepository,
    IGroupRepository groupRepository, IStudentRepository studentRepository, ILecturerRepository lecturerRepository,
    ISubjectRepository subjectRepository, ISemesterRepository semesterRepository)
        {
            _logger = logger;
            _projectRepository = projectRepository;
            _classRepository = classRepository;
            _groupRepository = groupRepository;
            _studentRepository = studentRepository;
            _lecturerRepository = lecturerRepository;
            _subjectRepository = subjectRepository;
            _semesterRepository = semesterRepository;
        }

        public async Task<ResponseDto<HashSet<ProjectDto>>> GetProjectFromClassByStudentAsync(int? classId, string? userEmail)
        {
            _logger.LogInformation("GetProjectFromClassByStudent(classId: {0}, userEmail: {1})", classId, userEmail);
            int studentId = await _studentRepository.FindStudentIdByEmail(userEmail);
            if (studentId == null || classId == null)
            {
                _logger.LogWarning("{0}{1}", GET_PROJECT, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return new ResponseDto<HashSet<ProjectDto>>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE };
            }
            if (await _classRepository.ExistInClassAsync(studentId, classId.Value) == 0)
            {
                _logger.LogWarning("{0}{1}", GET_PROJECT, "Student is not in class");
                return new ResponseDto<HashSet<ProjectDto>>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = "Student is not in class" };
            }
            return await GetProjectFromClassAsync(classId.Value);
        }

        public async Task<ResponseDto<HashSet<ProjectDto>>> GetProjectByLecturerAsync(string? semesterCode, int? classId, string userEmail)
        {
            _logger.LogInformation("GetProjectByLecturer(semesterCode: {0}, classId: {1}, userEmail: {2})", semesterCode, classId, userEmail);
            int lecturerId = await _lecturerRepository.FindLecturerIdByEmailAsync(userEmail);
            if (lecturerId == 0)
            {
                _logger.LogWarning("{0}{1}", GET_PROJECT, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return new ResponseDto<HashSet<ProjectDto>>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE };
            }
            if (classId != null)
            {
                if (!await _classRepository.ExistsByIdAsync(classId))
                {
                    _logger.LogWarning("{0}{1}", GET_PROJECT, ServiceMessage.ID_NOT_EXIST_MESSAGE);
                    return new ResponseDto<HashSet<ProjectDto>> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.ID_NOT_EXIST_MESSAGE };
                }
                if (!lecturerId.Equals((await _classRepository.FindOneByIdAsync(classId.Value)).LecturerId))
                {
                    _logger.LogWarning("{0}{1}", GET_PROJECT, "Lecturer not manage class");
                    return new ResponseDto<HashSet<ProjectDto>>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = "Lecturer not manage class" };
                }
                return await GetProjectFromClassAsync(classId.Value);
            }

            if (!string.IsNullOrEmpty(semesterCode))
            {
                return await GetAllProjectInSemesterByLecturerAsync(lecturerId, semesterCode);
            }
            return await GetAllProjectByLecturerAsync(lecturerId);
        }

        public async Task<ResponseDto<HashSet<ProjectDto>>> GetProjectFromClassAsync(int classId)
        {
            _logger.LogInformation("GetProjectFromClass(classId: {0})", classId);

            if (await _classRepository.FindOneByIdAsync(classId) == null)
            {
                _logger.LogWarning("Get project from class: {0}", ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return new ResponseDto<HashSet<ProjectDto>> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE };
            }
            var classEntity = await _classRepository.FindOneByIdAsync(classId);
            var projectSet = await _projectRepository
                .FindBySubjectIdAndLecturerIdAndSemester(classEntity.SubjectId, classEntity.LecturerId, classEntity.SemesterCode);
            var ProjectDtoSet = projectSet
            .Select(projectEntity => MapToProjectDto(projectEntity))
                .ToHashSet();
            _logger.LogInformation("Get project from class success");
            return new ResponseDto<HashSet<ProjectDto>> { code = ServiceStatusCode.OK_STATUS , message = ServiceMessage.SUCCESS_MESSAGE , data = ProjectDtoSet };
        }

        public async Task<ResponseDto<HashSet<ProjectDto>>> GetAllProjectInSemesterByLecturerAsync(int lecturerId, string semesterCode)
        {
            _logger.LogInformation("GetAllProjectInSemesterByLecturer(lecturerId: {0}, semesterCode: {1})", lecturerId, semesterCode);

            if (!await _semesterRepository.ExistsById(semesterCode))
            {
                _logger.LogWarning("{0}{1}", GET_PROJECT, ServiceMessage.ID_NOT_EXIST_MESSAGE);
                return new ResponseDto<HashSet<ProjectDto>>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.ID_NOT_EXIST_MESSAGE };
            }
            var projectSet = await _projectRepository.FindByLecturerIdAndSemester(lecturerId, semesterCode);
            var ProjectDtoSet = projectSet
            .Select(projectEntity => MapToProjectDto(projectEntity))
                .ToHashSet();
            _logger.LogInformation("Get project from class success");
            return new ResponseDto<HashSet<ProjectDto>> { code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE, data = ProjectDtoSet };
        }

        public async Task<ResponseDto<HashSet<ProjectDto>>> GetAllProjectByLecturerAsync(int lecturerId)
        {
            _logger.LogInformation("GetAllProjectByLecturer(lecturerId: {0})", lecturerId);

            var projectSet = await _projectRepository.FindByLecturerId(lecturerId);
            var ProjectDtoSet = projectSet
            .Select(projectEntity => MapToProjectDto(projectEntity))
                .ToHashSet();
            _logger.LogInformation("Get project from class success");
            return new ResponseDto<HashSet<ProjectDto>> { code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE, data = ProjectDtoSet };
        }

        public async Task<ResponseDto<int>> AddProjectAsync(ProjectDto ProjectDto, string userEmail)
        {
            _logger.LogInformation("AddProject(ProjectDto: {0}, userEmail: {1})", ProjectDto, userEmail);
            int lecturerId = await _lecturerRepository.FindLecturerIdByEmailAsync(userEmail);
            if (lecturerId == 0 || ProjectDto == null || ProjectDto.SubjectId == 0 || string.IsNullOrEmpty(ProjectDto.SemesterCode))
            {
                _logger.LogWarning("{0}{1}", ADD_PROJECT, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return new ResponseDto<int>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE };
            }
            if (!await _subjectRepository.ExistsById((int)ProjectDto.SubjectId))
            {
                _logger.LogWarning("{0}{1}", ADD_PROJECT, SUBJECT_NOT_EXIST);
                return new ResponseDto<int>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = SUBJECT_NOT_EXIST };
            }
            if (!await _semesterRepository.ExistsById(ProjectDto.SemesterCode))
            {
                _logger.LogWarning("{0}{1}", ADD_PROJECT, SEMESTER_NOT_EXIST);
                return new ResponseDto<int>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = SEMESTER_NOT_EXIST };
            }
            var project = MapToProject(ProjectDto);
            project.Lecturer = await _lecturerRepository.FindOneByEmailAsync(userEmail);
            int id = (await _projectRepository.SaveAsync(project));
            _logger.LogInformation("Add project success.");
            return new ResponseDto<int> { code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE, data = id };
        }

        public async Task<ResponseDto<object>> UpdateProjectAsync(ProjectDto ProjectDto, string userEmail)
        {
            _logger.LogInformation("UpdateProject(ProjectDto: {0}, userEmail: {1})", ProjectDto, userEmail);
            int lecturerId = await _lecturerRepository.FindLecturerIdByEmailAsync(userEmail);
            if (lecturerId == 0 || ProjectDto == null)
            {
                _logger.LogWarning("{0}{1}", UPDATE_PROJECT, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE };
            }
            if (!await _subjectRepository.ExistsById((int)(ProjectDto.SubjectId)))
            {
                _logger.LogWarning("{0}{1}", UPDATE_PROJECT, SUBJECT_NOT_EXIST);
                return new ResponseDto<object>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = SUBJECT_NOT_EXIST };
            }
            if (!await _semesterRepository.ExistsById(ProjectDto.SemesterCode))
            {
                _logger.LogWarning("{0}{1}", UPDATE_PROJECT, SEMESTER_NOT_EXIST);
                return new ResponseDto<object>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = SEMESTER_NOT_EXIST };
            }
            if (await _projectRepository.ExistsByLecturerId(lecturerId, ProjectDto.Id) == 0)
            {
                _logger.LogWarning("{0}{1}", UPDATE_PROJECT, PROJECT_NOT_MANAGE);
                return new ResponseDto<object>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = PROJECT_NOT_MANAGE };
            }
            var project = MapToProject(ProjectDto);
            project.Lecturer = await _lecturerRepository.FindOneByEmailAsync(userEmail);
            await _projectRepository.SaveAsync(project);
            _logger.LogInformation("Update project success.");
            return new ResponseDto<object> { code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE}; 
        }

        public async Task<ResponseDto<object>> DeleteProjectAsync(int projectId, string userEmail)
        {
            _logger.LogInformation("DeleteProject(projectId: {0}, userEmail: {1})", projectId, userEmail);
            int lecturerId = await _lecturerRepository.FindLecturerIdByEmailAsync(userEmail);
            if (lecturerId == 0)
            {
                _logger.LogWarning("{0}{1}", DELETE_PROJECT, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE };
            }
            if (await _projectRepository.ExistsById(projectId) != 0)
            {
                _logger.LogWarning("{0}{1}", DELETE_PROJECT, "Project not exist");
                return new ResponseDto<object>{ code = ServiceStatusCode.BAD_REQUEST_STATUS, message = "Project not exist" };
            }
            if (await _projectRepository.ExistsByLecturerId(lecturerId, projectId) == 0)
            {
                _logger.LogWarning("{0}{1}", DELETE_PROJECT, PROJECT_NOT_MANAGE);
                return new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = PROJECT_NOT_MANAGE };
            }
            if (await _groupRepository.ExistByProjectAsync(projectId) != 0)
            {
                _logger.LogWarning("{0}{1}", DELETE_PROJECT, "At least one group used this project.");
                return new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = "At least one group used this project." };
            }
            await _projectRepository.DeleteProject(projectId);
            _logger.LogInformation("Delete project success.");
            return new ResponseDto<object> { code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE };
        }

        private static ProjectDto MapToProjectDto(Project projectEntity)
        {
            return new ProjectDto
            {
                Id = projectEntity.Id,
                Name = projectEntity.Name,
                Problem = projectEntity.Problem,
                Actors = projectEntity.Actors,
                Requirements = projectEntity.Requirements,
                Theme = projectEntity.Theme,
                SubjectId = projectEntity.SubjectId.Value,
                SemesterCode = projectEntity.SemesterCode,
                Context = projectEntity.Context
                // Map other properties
            };
        }

        private static Project MapToProject(ProjectDto ProjectDto)
        {
            return new Project
            {
                Id = ProjectDto.Id,
                Name = ProjectDto.Name,
                Problem = ProjectDto.Problem,
                Actors = ProjectDto.Actors,
                Requirements = ProjectDto.Requirements,
                Theme = ProjectDto.Theme,
                SubjectId = ProjectDto.SubjectId,
                SemesterCode = ProjectDto.SemesterCode,
                Context = ProjectDto.Context

                // Map other properties
            };
        }
    }
}
