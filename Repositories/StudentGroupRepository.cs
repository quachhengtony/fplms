using BusinessObjects.DbContexts;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Repositories
{
    public class StudentGroupRepository :IStudentGroupRepository
    {
        private static StudentGroupRepository? instance;
        private static readonly object instanceLock = new object();
        private static FplmsManagementContext? dbContext;

        public static StudentGroupRepository Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        dbContext = new FplmsManagementContext();
                        instance = new StudentGroupRepository();
                    }
                    return instance;
                }
            }
        }

        public async Task DeleteStudentInGroup(int studentId, int classId)
        {
            await dbContext.Database.ExecuteSqlRawAsync("DELETE FROM STUDENT_GROUP WHERE STUDENT_id = {0} AND CLASS_id = {1}", studentId, classId);
        }

        public async Task DeleteAllStudentInGroup(int groupId)
        {
            await dbContext.Database.ExecuteSqlRawAsync("DELETE FROM STUDENT_GROUP WHERE GROUP_id = {0}", groupId);
        }

        public async Task UpdateStudentGroup(int studentId, int classId, int groupNumber)
        {
            await dbContext.Database.ExecuteSqlRawAsync("UPDATE STUDENT_GROUP SET GROUP_id = (SELECT id FROM [GROUP] WHERE CLASS_id = {0} AND number = {1}) WHERE STUDENT_id = {2} AND CLASS_id = {0}", classId, groupNumber, studentId);
        }

        public async Task<int> GetCurrentNumberOfMemberInGroup(int groupId)
        {
            return await dbContext.StudentGroups.CountAsync(sg => sg.GroupId == groupId);
        }

        public async Task AddStudentInGroup(int studentId, int groupId, int classId, int isLeader)
        {
            await dbContext.Database.ExecuteSqlRawAsync("INSERT INTO STUDENT_GROUP (STUDENT_id, GROUP_id, CLASS_id, is_leader) VALUES ({0}, {1}, {2}, {3})", studentId, groupId, classId, isLeader);
        }

        public async Task<int> FindStudentLeaderRoleInClass(int studentId, int classId)
        {
            return (await dbContext.StudentGroups.FirstOrDefaultAsync(sg => sg.StudentId == studentId && sg.ClassId == classId))?.IsLeader ?? 0;
        }

        public async Task<int> FindLeaderInGroup(int groupId)
        {
            return (await dbContext.StudentGroups.FirstOrDefaultAsync(sg => sg.GroupId == groupId && sg.IsLeader == 1))?.StudentId ?? 0;
        }

        public async Task<int> IsStudentExistInGroup(int groupId, int studentId)
        {
            return await dbContext.StudentGroups.AnyAsync(sg => sg.GroupId == groupId && sg.StudentId == studentId) ? 1 : 0;
        }

        public async Task UpdateGroupLeader(int groupId, int studentId, int isLeader)
        {
            await dbContext.Database.ExecuteSqlRawAsync("UPDATE STUDENT_GROUP SET is_leader = {0} WHERE GROUP_id = {1} AND STUDENT_id = {2}", isLeader, groupId, studentId);
        }

        public async Task<int> ChooseRandomGroupMember(int groupId)
        {
            return await dbContext.StudentGroups.Where(sg => sg.GroupId == groupId && sg.IsLeader == 0).Select(sg => sg.StudentId).FirstOrDefaultAsync();
        }

        public async Task AddRandomGroupLeader(int groupId, int leaderId)
        {
            await dbContext.Database.ExecuteSqlRawAsync("UPDATE STUDENT_GROUP SET is_leader = 1 WHERE GROUP_id = {0} AND STUDENT_id = {1}", groupId, leaderId);
        }
    }
}
