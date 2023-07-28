using BusinessObjects.DbContexts;
using Repositories.Contracts;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace Api.Repositories
{
    public class StudentRepository : RepositoryBase<Student>, IStudentRepository
    {
        public StudentRepository(FplmsManagementContext repositoryContext) : base(repositoryContext)
        {

        }

        public void CreateStudent(Student student)
        {
            Create(student);
        }

        public void DeleteStudent(Student student)
        {
            Delete(student);
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await FindAll().ToListAsync();

        }

        public async Task<Student> GetStudentByIdAsync(int studentId)
        {
            return await FindByCondition(student => student.Id.Equals(studentId)).FirstOrDefaultAsync();
        }

        public async Task<Student> GetStudentByEmailAsync(string studentEmail)
        {
            return await FindByCondition(student => student.Email.Equals(studentEmail)).FirstOrDefaultAsync();
        }

        public void UpdateStudent(Student student)
        {
            Update(student);
        }
    }
}