using BusinessObjects.DbContexts;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private FplmsManagementContext dbContext;

        public StudentRepository()
        {
            dbContext = new FplmsManagementContext();
        }

        public async Task<Group> FindGroupByStudentIdAndClassId(int studentId, int classId)
        {
            return await dbContext.Groups.FromSqlRaw("SELECT * FROM `group` WHERE id = (SELECT GROUP_id FROM student_group WHERE STUDENT_id = {0} AND CLASS_id = {1})", studentId, classId).FirstOrDefaultAsync();
        }

        public async Task<int> FindStudentIdByEmail(string email)
        {
            var student = await dbContext.Students.FirstOrDefaultAsync(s => s.Email == email);
            return student?.Id ?? 0;
        }

        public async Task<Student> FindOneById(int studentId)
        {
            return await dbContext.Students.FindAsync(studentId);
        }

        public async Task<bool> ExistsByEmail(string email)
        {
            return await dbContext.Students.AnyAsync(s => s.Email == email);
        }

        public async Task<bool> ExistsById(int studentId)
        {
            return await dbContext.Students.AnyAsync(s => s.Id == studentId);
        }
    }
}
