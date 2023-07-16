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
    public class MeetingRepository : IMeetingRepository
    {
        private static MeetingRepository instance;
        private static readonly object instanceLock = new object();
        private static FplmsManagementContext dbContext;

        public static MeetingRepository Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        dbContext = new FplmsManagementContext();
                        instance = new MeetingRepository();
                    }
                    return instance;
                }
            }
        }

        public Task<List<Meeting>> FindByGroupIdAsync(int groupId, DateTime startDate, DateTime endDate)
        {
            return dbContext.Meetings.Where(m => m.GroupId == groupId && m.ScheduleTime >= startDate && m.ScheduleTime <= endDate)
                .ToListAsync();
        }

        public Task<List<Meeting>> FindByClassIdAsync(int classId, DateTime startDate, DateTime endDate)
        {
            return dbContext.Meetings.Where(m => m.Group.ClassId == classId && m.ScheduleTime >= startDate && m.ScheduleTime <= endDate)
                .ToListAsync();
        }

        public Task<List<Meeting>> FindByLecturerIdAsync(int lecturerId, DateTime startDate, DateTime endDate)
        {
            return dbContext.Meetings.Where(m => m.LecturerId == lecturerId && m.ScheduleTime >= startDate && m.ScheduleTime <= endDate)
                .ToListAsync();
        }

        public Task<List<Meeting>> FindByStudentIdAsync(int studentId, DateTime startDate, DateTime endDate)
        {
            var res = dbContext.StudentGroups.Where(sg => sg.StudentId == studentId).Select(sg => sg.GroupId).ToList();
            return dbContext.Meetings.Where(m => res.Contains(m.GroupId) && m.ScheduleTime >= startDate && m.ScheduleTime <= endDate)
                .ToListAsync();
        }

        public Task<Meeting> FindOneByIdAsync(int meetingId)
        {
            return dbContext.Meetings.Where(m => m.Id == meetingId).FirstOrDefaultAsync();
        }

        public async Task<bool> ExistsById(int meetingId)
        {
            return await dbContext.Meetings
                .AnyAsync(m => m.Id == meetingId);
        }

        public async Task<int> SaveAsync(Meeting meeting)
        {
            dbContext.Meetings.Add(meeting);
            await dbContext.SaveChangesAsync();
            return meeting.Id;
        }

        public async Task DeleteByIdAsync(int meetingId)
        {
            var meeting = await dbContext.Meetings.FindAsync(meetingId);
            if (meeting != null)
            {
                dbContext.Meetings.Remove(meeting);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
