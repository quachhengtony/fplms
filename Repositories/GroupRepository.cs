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
    public class GroupRepository : IGroupRepository
    {
        private FplmsManagementContext dbContext;

        public GroupRepository()
        {
            dbContext = new FplmsManagementContext();
        }

        public Task<int> ExistByProjectAsync(int projectId)
        {
            return dbContext.Groups.Where(g => g.ProjectId == projectId && g.IsDisable == 0)
                .Select(g => g.Id)
                .FirstOrDefaultAsync();
        }

        public Task<int> FindGroupByStudentIdAndClassIdAsync(int studentId, int classId)
        {
            return dbContext.StudentGroups.Where(sg => sg.StudentId == studentId && sg.ClassId == classId)
                .Select(sg => sg.GroupId)
                .FirstOrDefaultAsync();
        }

        public Task<int> FindGroupNumberAsync(int groupId, int classId)
        {
            return dbContext.Groups.Where(g => g.Id == groupId && g.ClassId == classId)
                .Select(g => g.Number.Value)
                .FirstOrDefaultAsync();
        }

        public Task<int> FindLectureIdOfGroupAsync(int groupId)
        {
            return dbContext.Groups.Where(g => g.Id == groupId)
                .Include(g => g.Class)
                .Select(g => g.Class.LecturerId)
                .FirstOrDefaultAsync();
        }

        public Task<Group> FindOneByIdAsync(int groupId)
        {
            return dbContext.Groups.Where(g => g.Id == groupId)
                .Include(g => g.Class).ThenInclude(cl => cl.SemesterCodeNavigation)
                .Include(g => g.Project)
                .Include(g => g.Meetings)
                .Include(g => g.CycleReports)
                .Include(g => g.ProgressReports)
                .Include(g => g.StudentGroups).ThenInclude(sg => sg.Student)
                .FirstOrDefaultAsync();
        }

        public Task<int> GetGroupLimitNumberAsync(int groupId)
        {
            return dbContext.Groups.Where(g => g.Id == groupId && g.IsDisable == 0)
                .Select(g => g.MemberQuantity ?? 0 ) 
                .FirstOrDefaultAsync();
        }

        public Task<int> GetMaxGroupNumberAsync(int classId)
        {
            return dbContext.Groups.Where(g => g.ClassId == classId)
                .DefaultIfEmpty()
                .MaxAsync(g => g.Number ?? 0 );
        }

        public Task<int> IsEnrollTimeOverAsync(int groupId, DateTime currentTime)
        {
            return dbContext.Groups.Where(g => g.Id == groupId && g.IsDisable == 0 && g.EnrollTime > currentTime)
                .Select(g => g.Id)
                .FirstOrDefaultAsync();
        }

        public Task<int> IsGroupDisableAsync(int groupId)
        {
            return dbContext.Groups.Where(g => g.Id == groupId)
                .Select(g => Convert.ToInt32(g.IsDisable))
                .FirstOrDefaultAsync();
        }

        public Task<int> IsGroupExistsInClassAsync(int groupId, int classId)
        {
            return dbContext.Groups.Where(g => g.Id == groupId && g.ClassId == classId)
                .Select(g => g.Id)
                .FirstOrDefaultAsync();
        }

        public Task<int> SetGroupDisableAsync(int groupId)
        {
            dbContext.Database.ExecuteSqlRawAsync($"update `group` set is_disable = 1 where id = {groupId}");
            dbContext.SaveChanges();
            return Task.FromResult(1);
        }

        public Task<int> SetGroupEnableAsync(int groupId)
        {
            dbContext.Database.ExecuteSqlRawAsync($"update `group` set is_disable = 0 where id = {groupId}");
            dbContext.SaveChanges();
            return Task.FromResult(1);

        }

        public Task<int> UpdateProjectInGroupAsync(int groupId, int projectId)
        {
            return dbContext.Database.ExecuteSqlRawAsync($"update `group` set PROJECT_id = {projectId} where id = {groupId}");
        }

        public async Task Add(Group group)
        {
            dbContext.Groups.Add(group);
            dbContext.SaveChanges();
            return;
        }

        public async Task Update(Group group)
        {
            dbContext.Groups.Update(group);
            dbContext.SaveChanges();
            return;
        }

        public async Task Delete(Group group)
        {
            dbContext.Groups.Remove(group);
            dbContext.SaveChanges();
            return;
        }

        public async Task<bool> ExistsById(int groupId)
        {
            return await dbContext.Groups.AnyAsync(g => g.Id == groupId);
        }
        public async Task<Group> GetGroupIdByNumberAndClassIdAsync(int number, int classId)
        {
            Group group = await dbContext.Groups
                .Where(g => g.Number == number && g.ClassId == classId && g.IsDisable == 0)
                .FirstOrDefaultAsync();

            return group;
        }
    }
}
