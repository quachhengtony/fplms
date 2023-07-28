using BusinessObjects.DbContexts;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class ProgressReportRepository : IProgressReportRepository
    {
        private FplmsManagementContext dbContext;

        public ProgressReportRepository()
        {
            dbContext = new FplmsManagementContext();
        }
        public async Task<HashSet<ProgressReport>> FindByGroup(Group group)
        {
            var reports = await dbContext.ProgressReports
                .Where(pr => pr.Group == group)
                .ToListAsync();
            return reports.ToHashSet();
        }


        public async Task<int> ExistsByIdAndGroupIdAndStudentId(int groupId, int reportId, int studentId)
        {
            return await dbContext.ProgressReports
                .Where(pr => pr.GroupId == groupId && pr.Id == reportId && pr.StudentId == studentId)
                .Select(pr => pr.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<int> ExistsByStudentIdAndGroupIdAndCurDate(int studentId, int groupId, DateTime curDate)
        {
            return await dbContext.ProgressReports
                .Where(pr => pr.StudentId == studentId && pr.GroupId == groupId && pr.ReportTime.Date == curDate)
                .Select(pr => pr.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<HashSet<ProgressReport>> FindByGroupIdAndTimeFilter(int groupId, DateTime startDate, DateTime endDate)
        {
            var reports  = await dbContext.ProgressReports
                .Where(pr => pr.GroupId == groupId && pr.ReportTime >= startDate && pr.ReportTime <= endDate)
                .ToListAsync();
            return reports.ToHashSet();
        }

        public async Task<DateTime> GetDateOfProgressReport(int reportId)
        {
            return await dbContext.ProgressReports
                .Where(pr => pr.Id == reportId)
                .Select(pr => pr.ReportTime)
                .FirstOrDefaultAsync();
        }

        public Task<bool> ExistsById(int reportId)
        {
            return dbContext.ProgressReports.AnyAsync(pr => pr.Id == reportId);
        }

        public async Task<ProgressReport> GetByIdAsync(int reportId)
        {
            return await dbContext.ProgressReports.FindAsync(reportId);
        }

        public async Task AddAsync(ProgressReport progressReport)
        {
            dbContext.ProgressReports.Add(progressReport);
            await dbContext.SaveChangesAsync();
        }
        public async Task UpdateAsync(ProgressReport progressReport)
        {
            dbContext.ProgressReports.Add(progressReport);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int reportId)
        {
            var progressReport = await GetByIdAsync(reportId);
            if (progressReport != null)
            {
                dbContext.ProgressReports.Remove(progressReport);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
