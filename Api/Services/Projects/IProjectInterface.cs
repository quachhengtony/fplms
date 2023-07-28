using Api.Dto.Shared;
using FPLMS.Api.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Services.Projects
{
    public interface IProjectService
    {
        Task<ResponseDto<HashSet<ProjectDto>>> GetProjectFromClassByStudentAsync(int? classId, string userEmail);
        Task<ResponseDto<HashSet<ProjectDto>>> GetProjectByLecturerAsync(string? semesterCode, int? classId, string userEmail);
        Task<ResponseDto<HashSet<ProjectDto>>> GetProjectFromClassAsync(int classId);
        Task<ResponseDto<HashSet<ProjectDto>>> GetAllProjectInSemesterByLecturerAsync(int lecturerId, string semesterCode);
        Task<ResponseDto<HashSet<ProjectDto>>> GetAllProjectByLecturerAsync(int lecturerId);
        Task<ResponseDto<int>> AddProjectAsync(ProjectDto projectDto, string userEmail);
        Task<ResponseDto<object>> UpdateProjectAsync(ProjectDto projectDto, string userEmail);
        Task<ResponseDto<object>> DeleteProjectAsync(int projectId, string userEmail);
    }

}
