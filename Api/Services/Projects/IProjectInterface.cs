using Api.Dto.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Services.Projects
{
    public interface IProjectService
    {
        Task<HashSet<ProjectDTO>> GetProjectFromClassByStudent(int classId, string userEmail);

        Task<HashSet<ProjectDTO>> GetProjectByLecturer(string semesterCode, int? classId, string userEmail);

        Task<HashSet<ProjectDTO>> GetProjectFromClass(int classId);

        Task<HashSet<ProjectDTO>> GetAllProjectInSemesterByLecturer(int lecturerId, string semesterCode);

        Task<HashSet<ProjectDTO>> GetAllProjectByLecturer(int lecturerId);

        Task<int> AddProject(ProjectDTO projectDTO, string userEmail);

        Task UpdateProject(ProjectDTO projectDTO, string userEmail);

        Task DeleteProject(int projectId, string userEmail);
    }
}
