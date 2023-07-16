using BusinessObjects.DbContexts;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private static ProjectRepository instance;
        private static readonly object instanceLock = new object();
        private static FplmsManagementContext dbContext;

        public static ProjectRepository Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        dbContext = new FplmsManagementContext();
                        instance = new ProjectRepository();
                    }
                    return instance;
                }
            }
        }

        public async Task<int> ExistsByLecturerId(int lecturerId, int projectId)
        {
            return await dbContext.Projects
                .FromSqlRaw("SELECT * FROM PROJECT WHERE id = {1} AND LECTURER_id = {0} AND is_disable = 0", lecturerId, projectId)
                .Select(p => p.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<int> ExistsById(int projectId)
        {
            return await dbContext.Projects
                .FromSqlRaw("SELECT * FROM PROJECT WHERE id = {1} ", projectId)
                .Select(p => p.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<HashSet<Project>> FindBySubjectIdAndLecturerIdAndSemester(int subjectId, int lecturerId, string semesterCode)
        {
            var projects = await dbContext.Projects
                .FromSqlRaw("SELECT * FROM project WHERE SUBJECT_id = {0} AND LECTURER_id = {1} AND SEMESTER_code = {2} AND is_disable = 0", subjectId, lecturerId, semesterCode)
                .ToListAsync();

            return projects.ToHashSet();
        }

        public async Task<HashSet<Project>> FindByLecturerIdAndSemester(int lecturerId, string semesterCode)
        {
            var projects = await dbContext.Projects
                .FromSqlRaw("SELECT * FROM PROJECT WHERE LECTURER_id = {0} AND SEMESTER_code = {1} AND is_disable = 0", lecturerId, semesterCode)
                .ToListAsync();

            return projects.ToHashSet();
        }

        public async Task<HashSet<Project>> FindByLecturerId(int lecturerId)
        {
            var projects = await dbContext.Projects
                .FromSqlRaw("SELECT * FROM PROJECT WHERE LECTURER_id = {0} AND is_disable = 0", lecturerId)
                .ToListAsync();

            return projects.ToHashSet();
        }

        public async Task DeleteProject(int projectId)
        {
            await dbContext.Database.ExecuteSqlInterpolatedAsync($"UPDATE PROJECT SET is_disable = 1 WHERE id = {projectId}");
        }


        public async Task<int> SaveAsync(Project project)
        {
            dbContext.Projects.Add(project);
            await dbContext.SaveChangesAsync();
            return project.Id;
        }
    }
}
