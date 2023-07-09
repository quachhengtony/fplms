using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using BusinessObjects.DbContexts;
using Repositories.Interfaces;

namespace Repositories
{
    public class ClassRepository : IClassRepository
    {
        private static ClassRepository instance;
        private static readonly object instanceLock = new object();
        private static FplmsManagementContext dbContext;

        public static ClassRepository Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        dbContext = new FplmsManagementContext();
                        instance = new ClassRepository();
                    }
                    return instance;
                }
            }
        }


        public Task<int> DeleteStudentInClassAsync(int studentId, int classId)
        {
            return dbContext.Database.ExecuteSqlRawAsync($"DELETE FROM student_class WHERE STUDENT_id = {studentId} AND CLASS_id = {classId}");
        }

        public Task<int> ExistInClassAsync(int studentId, int classId)
        {
            return dbContext.Classes.Where(c => c.Id == classId)
                .Include(c => c.Students)
                .Where(c => c.Students.Select(s => s.Id).Contains(studentId))
                .Select(c => c.Students.FirstOrDefault() == null ? 0 : studentId)
                .FirstOrDefaultAsync();
        }

        public Task<int> FindClassBySemesterAsync(string semesterCode)
        {
            return dbContext.Classes.Where(c => c.SemesterCode == semesterCode)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();
        }

        public Task<int> FindClassBySubjectAsync(int subjectId)
        {
            return dbContext.Classes.Where(c => c.SubjectId == subjectId)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();
        }

        public Task<string> FindLecturerEmailOfClassAsync(int classId)
        {
            return dbContext.Classes.Where(c => c.Id == classId && c.IsDisable == 0)
                .Include(c => c.Lecturer)
                .Select(c => c.Lecturer.Email)
                .FirstOrDefaultAsync();
        }

        public Task<Class> FindOneByIdAsync(int classId)
        {
            return dbContext.Classes.Where(c => c.Id == classId && c.IsDisable == 0)
                .Include(c => c.Groups).ThenInclude(g => g.Project)
                .Include(c => c.Lecturer)
                .Include(c => c.Students)
                .Include(c => c.SemesterCodeNavigation)
                .FirstOrDefaultAsync();
        }

        public Task<List<Class>> GetClassBySearchStrAsync(string searchStr)
        {
            return dbContext.Classes
                .Where(c => c.Name.Contains(searchStr) && c.IsDisable == 0)
                .Include(c => c.Lecturer)
                .ToListAsync();
        }

        public Task<string> GetClassEnrollKeyAsync(int classId)
        {
            return dbContext.Classes.Where(c => c.Id == classId && c.IsDisable == 0)
                .Select(c => c.EnrollKey)
                .FirstOrDefaultAsync();
        }

        public Task<string> GetClassSemesterAsync(int classId)
        {
            return dbContext.Classes.Where(c => c.Id == classId && c.IsDisable == 0)
                .Select(c => c.SemesterCode)
                .FirstOrDefaultAsync();
        }

        public Task<int> InsertStudentInClassAsync(int studentId, int classId)
        {
            return dbContext.Database.ExecuteSqlRawAsync($"insert into student_class(STUDENT_id, CLASS_id) values ({studentId}, {classId})");
        }

        public async Task Add(Class _class)
        {
            dbContext.Classes.Add(_class);
            dbContext.SaveChanges();
            return;
        }

        public async Task Update(Class _class)
        {
            dbContext.Classes.Update(_class);
            dbContext.SaveChanges();
            return;
        }

        public async Task Delete(Class _class)
        {
            dbContext.Classes.Remove(_class);
            dbContext.SaveChanges();
            return;
        }
    }
}
