using BusinessObjects.Models;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IProjectRepository
    {
        Task<int> ExistsByLecturerId(int lecturerId, int projectId);
        Task<int> ExistsById(int projectId);
        Task<HashSet<Project>> FindBySubjectIdAndLecturerIdAndSemester(int subjectId, int lecturerId, string semesterCode);
        Task<HashSet<Project>> FindByLecturerIdAndSemester(int lecturerId, string semesterCode);
        Task<HashSet<Project>> FindByLecturerId(int lecturerId);
        Task DeleteProject(int projectId);
        bool ExistsById(int projectId);
        Task<int> SaveAsync(Project project);
    }
}
