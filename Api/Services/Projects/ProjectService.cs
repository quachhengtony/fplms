using Api.Dto.Shared;
using Microsoft.Extensions.Logging;
using Repositories.Interfaces;
using Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using BusinessObjects.Models;
using System.Linq;

namespace Api.Services.Projects
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IClassRepository _classRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ILecturerRepository _lecturerRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly ISemesterRepository _semesterRepository;
        private readonly ILogger<ProjectService> _logger;

        private static readonly string PROJECT_NOT_MANAGE = "Lecturer not manage this project.";
        private static readonly string SUBJECT_NOT_EXIST = "Subject not exist";
        private static readonly string SEMESTER_NOT_EXIST = "Semester not exist";
        private static readonly string GET_PROJECT = "Get project from class: ";
        private static readonly string ADD_PROJECT = "Add project to class: ";
        private static readonly string UPDATE_PROJECT = "Update project in class: ";
        private static readonly string DELETE_PROJECT = "Delete project in class: ";

        public ProjectService(
            IProjectRepository projectRepository,
            IClassRepository classRepository,
            IGroupRepository groupRepository,
            IStudentRepository studentRepository,
            ILecturerRepository lecturerRepository,
            ISubjectRepository subjectRepository,
            ISemesterRepository semesterRepository,
            ILogger<ProjectService> logger)
        {
            _projectRepository = projectRepository;
            _classRepository = classRepository;
            _groupRepository = groupRepository;
            _studentRepository = studentRepository;
            _lecturerRepository = lecturerRepository;
            _subjectRepository = subjectRepository;
            _semesterRepository = semesterRepository;
            _logger = logger;
        }

        public async Task<HashSet<ProjectDTO>> GetProjectFromClassByStudent(int classId, string userEmail)
        {
            _logger.LogInformation("GetProjectFromClassByStudent(classId: {classId}, userEmail: {userEmail})", classId, userEmail);

            var studentId = await _studentRepository.FindStudentIdByEmail(userEmail);
            if (studentId == null || classId == null)
            {
                _logger.LogWarning("{0}{1}", GET_PROJECT, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                throw new ArgumentException(ServiceMessage.INVALID_ARGUMENT_MESSAGE);
            }

            if (await _classRepository.ExistInClassAsync(studentId, classId) == null)
            {
                _logger.LogWarning("{0}{1}", GET_PROJECT, "Student is not in class");
                throw new ArgumentException("Student is not in class");
            }

            return await GetProjectFromClass(classId);
        }

        public async Task<HashSet<ProjectDTO>> GetProjectByLecturer(string semesterCode, int? classId, string userEmail)
        {
            _logger.LogInformation("GetProjectByLecturer(semesterCode: {semesterCode}, classId: {classId}, userEmail: {userEmail})", semesterCode, classId, userEmail);

            var lecturerId = await _lecturerRepository.FindLecturerIdByEmailAsync(userEmail);
            if (lecturerId == null)
            {
                _logger.LogWarning("{0}{1}", GET_PROJECT, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                throw new ArgumentException(ServiceMessage.INVALID_ARGUMENT_MESSAGE);
            }

            if (classId != null)
            {
                if (!await _classRepository.ExistsByIdAsync(classId))
                {
                    _logger.LogWarning("{0}{1}", GET_PROJECT, ServiceMessage.ID_NOT_EXIST_MESSAGE);
                    throw new ArgumentException(ServiceMessage.ID_NOT_EXIST_MESSAGE);
                }

                if (!lecturerId.Equals((await _classRepository.FindOneByIdAsync((int)classId)).LecturerId))
                {
                    _logger.LogWarning("{0}{1}", GET_PROJECT, "Lecturer not manage class");
                    throw new ArgumentException("Lecturer not manage class");
                }

                return await GetProjectFromClass((int)classId);
            }

            if (semesterCode != null)
            {
                return await GetAllProjectInSemesterByLecturer(lecturerId, semesterCode);
            }

            return await GetAllProjectByLecturer(lecturerId);
        }

        public async Task<HashSet<ProjectDTO>> GetProjectFromClass(int classId)
        {
            _logger.LogInformation("GetProjectFromClass(classId: {classId})", classId);

            if (_classRepository.FindOneByIdAsync(classId) == null)
            {
                _logger.LogWarning("Get project from class: {0}", ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                throw new ArgumentException(ServiceMessage.INVALID_ARGUMENT_MESSAGE);
            }
            var classEntity = await _classRepository.FindOneByIdAsync(classId);

            var projectSet = await _projectRepository.FindBySubjectIdAndLecturerIdAndSemester(
                classEntity.SubjectId,
                classEntity.LecturerId,
                classEntity.SemesterCode);

            var projectDtoSet = projectSet.Select(projectEntity => MapToProjectDTO(projectEntity)).ToHashSet();

            _logger.LogInformation("Get project from class success");
            return projectDtoSet;
        }

        public async Task<HashSet<ProjectDTO>> GetAllProjectInSemesterByLecturer(int lecturerId, string semesterCode)
        {
            _logger.LogInformation("GetAllProjectInSemesterByLecturer(lecturerId: {lecturerId}, semesterCode: {semesterCode})", lecturerId, semesterCode);

            if (!await _semesterRepository.ExistsByIdAsync(semesterCode))
            {
                _logger.LogWarning("{0}{1}", GET_PROJECT, ServiceMessage.ID_NOT_EXIST_MESSAGE);
                throw new ArgumentException(ServiceMessage.ID_NOT_EXIST_MESSAGE);
            }

            var projectSet = await _projectRepository.FindByLecturerIdAndSemester(lecturerId, semesterCode);
            var projectDtoSet = projectSet.Select(projectEntity => MapToProjectDTO(projectEntity)).ToHashSet();

            _logger.LogInformation("Get project from class success");
            return projectDtoSet;
        }

        public async Task<HashSet<ProjectDTO>> GetAllProjectByLecturer(int lecturerId)
        {
            _logger.LogInformation("GetAllProjectByLecturer(lecturerId: {lecturerId})", lecturerId);

            var projectSet = (await _projectRepository
                .FindByLecturerId(lecturerId))
                .Select(projectEntity => MapToProjectDTO(projectEntity))
                .ToHashSet();

            _logger.LogInformation("Get project from class success");
            return projectSet;
        }

        public async Task<int> AddProject(ProjectDTO projectDTO, string userEmail)
        {
            _logger.LogInformation("AddProject(projectDTO: {projectDTO}, userEmail: {userEmail})", projectDTO, userEmail);

            var lecturerId = await _lecturerRepository.FindLecturerIdByEmailAsync(userEmail);
            if (lecturerId == null || projectDTO == null || projectDTO.SubjectId == null || projectDTO.SemesterCode == null)
            {
                _logger.LogWarning("{0}{1}", ADD_PROJECT, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                throw new ArgumentException(ServiceMessage.INVALID_ARGUMENT_MESSAGE);
            }

            if (!await _subjectRepository.ExistsById(projectDTO.SubjectId))
            {
                _logger.LogWarning("{0}{1}", ADD_PROJECT, SUBJECT_NOT_EXIST);
                throw new ArgumentException(SUBJECT_NOT_EXIST);
            }

            if (!await _semesterRepository.ExistsByIdAsync(projectDTO.SemesterCode))
            {
                _logger.LogWarning("{0}{1}", ADD_PROJECT, SEMESTER_NOT_EXIST);
                throw new ArgumentException(SEMESTER_NOT_EXIST);
            }

            var project = MapToProjectEntity(projectDTO);
            project.Lecturer = await _lecturerRepository.FindOneByEmailAsync(userEmail);
            var id = await _projectRepository.SaveAsync(project);

            _logger.LogInformation("Add project success.");
            return id;
        }

        public async Task UpdateProject(ProjectDTO projectDTO, string userEmail)
        {
            _logger.LogInformation("UpdateProject(projectDTO: {projectDTO}, userEmail: {userEmail})", projectDTO, userEmail);

            var lecturerId = await _lecturerRepository.FindLecturerIdByEmailAsync(userEmail);
            if (lecturerId == null || projectDTO == null)
            {
                _logger.LogWarning("{0}{1}", UPDATE_PROJECT, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                throw new ArgumentException(ServiceMessage.INVALID_ARGUMENT_MESSAGE);
            }

            if (!await _subjectRepository.ExistsById(projectDTO.SubjectId))
            {
                _logger.LogWarning("{0}{1}", UPDATE_PROJECT, SUBJECT_NOT_EXIST);
                throw new ArgumentException(SUBJECT_NOT_EXIST);
            }

            if (!await _semesterRepository.ExistsByIdAsync(projectDTO.SemesterCode))
            {
                _logger.LogWarning("{0}{1}", UPDATE_PROJECT, SEMESTER_NOT_EXIST);
                throw new ArgumentException(SEMESTER_NOT_EXIST);
            }

            if (_projectRepository.ExistsByLecturerId(lecturerId, projectDTO.Id) == null)
            {
                _logger.LogWarning("{0}{1}", UPDATE_PROJECT, PROJECT_NOT_MANAGE);
                throw new ArgumentException(PROJECT_NOT_MANAGE);
            }

            var project = MapToProjectEntity(projectDTO);
            project.Lecturer = await _lecturerRepository.FindOneByEmailAsync(userEmail);
            await _projectRepository.SaveAsync(project);

            _logger.LogInformation("Update project success.");
        }

        public async Task DeleteProject(int projectId, string userEmail)
        {
            _logger.LogInformation("DeleteProject(projectId: {projectId}, userEmail: {userEmail})", projectId, userEmail);

            var lecturerId = await _lecturerRepository.FindLecturerIdByEmailAsync(userEmail);
            if (lecturerId == null)
            {
                _logger.LogWarning("{0}{1}", DELETE_PROJECT, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                throw new ArgumentException(ServiceMessage.INVALID_ARGUMENT_MESSAGE);
            }

            if (!_projectRepository.ExistsById(projectId))
            {
                _logger.LogWarning("{0}{1}", DELETE_PROJECT, "Project not exist");
                throw new ArgumentException("Project not exist");
            }

            if (_projectRepository.ExistsByLecturerId(lecturerId, projectId) == null)
            {
                _logger.LogWarning("{0}{1}", DELETE_PROJECT, PROJECT_NOT_MANAGE);
                throw new ArgumentException(PROJECT_NOT_MANAGE);
            }

            if (_groupRepository.ExistByProjectAsync(projectId) != null)
            {
                _logger.LogWarning("{0}{1}", DELETE_PROJECT, "At least one group used this project.");
                throw new ArgumentException("At least one group used this project.");
            }

            await _projectRepository.DeleteProject(projectId);

            _logger.LogInformation("Delete project success.");
        }

        private Project MapToProjectEntity(ProjectDTO projectDTO)
        {
            return new Project
            {
                Id = projectDTO.Id,
                Theme = projectDTO.Theme,
                Name = projectDTO.Name,
                Problem = projectDTO.Problem,
                Context = projectDTO.Context,
                Actors = projectDTO.Actors,
                Requirements = projectDTO.Requirements,
                SubjectId = projectDTO.SubjectId,
                SemesterCode = projectDTO.SemesterCode,
                // Map other properties as needed
            };
        }

        private ProjectDTO MapToProjectDTO(Project projectEntity)
        {
            return new ProjectDTO
            {
                Id = projectEntity.Id,
                Theme = projectEntity.Theme,
                Name = projectEntity.Name,
                Problem = projectEntity.Problem,
                Context = projectEntity.Context,
                Actors = projectEntity.Actors,
                Requirements = projectEntity.Requirements,
                SubjectId = (int)projectEntity.SubjectId,
                SemesterCode = projectEntity.SemesterCode,
                // Map other properties as needed
            };
        }
    }
}
