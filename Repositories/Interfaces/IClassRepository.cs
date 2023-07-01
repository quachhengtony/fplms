using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace Repositories.Interfaces
{
    public interface IClassRepository
    {
        public Task<Class> FindOneByIdAsync(int classId);

        public Task<int> ExistInClassAsync(int studentId, int classId);

        public Task<int> DeleteStudentInClassAsync(int studentId, int classId);

        public Task<int> InsertStudentInClassAsync(int studentId, int classId);

        public Task<string> GetClassEnrollKeyAsync(int classId);

        public Task<List<Class>> GetClassBySearchStrAsync(string searchStr);

        public Task<string> FindLecturerEmailOfClassAsync(int classId);

        public Task<string> GetClassSemesterAsync(int classId);

        public Task<int> FindClassBySemesterAsync(string semesterCode);

        public Task<int> FindClassBySubjectAsync(int subjectId);


    }
}
