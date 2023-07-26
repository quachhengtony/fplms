using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessObjects.Models;
using BusinessObjects.Models;

namespace Repositories.Contracts
{
    public interface IStudentRepository : IRepositoryBase<Student>
    {
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<Student> GetStudentByIdAsync(int studentId);
        Task<Student> GetStudentByEmailAsync(string studentEmail);
        void CreateStudent(Student student);
        void UpdateStudent(Student student);
        void DeleteStudent(Student student);
    }
}