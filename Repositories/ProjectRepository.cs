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
        private FplmsManagementContext dbContext;

        public ProjectRepository()
        {
            dbContext = new FplmsManagementContext();
        }

        public async Task<int> ExistsByLecturerId(int lecturerId, int projectId)
        {
            return await dbContext.Projects
                .FromSqlRaw("SELECT * FROM project WHERE id = {1} AND LECTURER_id = {0} AND is_disable = 0", lecturerId, projectId)
                .Select(p => p.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<int> ExistsById(int projectId)
        {
            return await dbContext.Projects
                .FromSqlRaw("SELECT * FROM project WHERE id = {0} ", projectId)
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
                .FromSqlRaw("SELECT * FROM project WHERE LECTURER_id = {0} AND SEMESTER_code = {1} AND is_disable = 0", lecturerId, semesterCode)
                .ToListAsync();

            return projects.ToHashSet();
        }

        public async Task<HashSet<Project>> FindByLecturerId(int lecturerId)
        {
            var projects = await dbContext.Projects
                .FromSqlRaw("SELECT * FROM project WHERE LECTURER_id = {0} AND is_disable = 0", lecturerId)
                .ToListAsync();

            return projects.ToHashSet();
        }

        public async Task DeleteProject(int projectId)
        {
            var projectToDelete = await dbContext.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

            if (projectToDelete != null)
            {
                projectToDelete.IsDisable = 1; // Assuming your column is named "IsDisable" instead of "is_disable"
                await dbContext.SaveChangesAsync();
            }
        }


        public async Task<int> AddAsync(Project project)
        {
            dbContext.Projects.Add(project);
            await dbContext.SaveChangesAsync();
            return project.Id;
        }

        public Task<int> UpdateAsync(Project project)
        {
            dbContext.Projects.Update(project);
            dbContext.SaveChangesAsync();
            return Task.FromResult(project.Id);
        }
    }
}
